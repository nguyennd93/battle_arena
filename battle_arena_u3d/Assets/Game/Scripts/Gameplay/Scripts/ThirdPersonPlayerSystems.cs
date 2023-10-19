using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Unity.Physics.Systems;
using Unity.CharacterController;

[UpdateInGroup(typeof(InitializationSystemGroup))]
public partial class ThirdPersonPlayerInputsSystem : SystemBase
{
    private ControlActions _controlActions;

    protected override void OnCreate()
    {
        RequireForUpdate<FixedTickSystem.Singleton>();
        RequireForUpdate(SystemAPI.QueryBuilder().WithAll<ThirdPersonPlayer, ThirdPersonPlayerInputs>().Build());

        _controlActions = new ControlActions();
    }

    protected override void OnStartRunning()
    {
        _controlActions.Enable();
    }

    protected override void OnStopRunning()
    {
        _controlActions.Disable();
    }

    protected override void OnUpdate()
    {
        uint fixedTick = SystemAPI.GetSingleton<FixedTickSystem.Singleton>().Tick;

        foreach (var (playerInputs, player) in SystemAPI.Query<RefRW<ThirdPersonPlayerInputs>, ThirdPersonPlayer>())
        {
            playerInputs.ValueRW.MoveInput = Vector2.ClampMagnitude(_controlActions.Controller.Movement.ReadValue<Vector2>(), 1f);
            playerInputs.ValueRW.CameraLookInput = Vector2.ClampMagnitude(_controlActions.Controller.Look.ReadValue<Vector2>(), 1f);
            playerInputs.ValueRW.CameraZoomInput = 1f;
            playerInputs.ValueRW.AttackPressed = _controlActions.Controller.Attack.ReadValue<float>() > 0f;
            playerInputs.ValueRW.SkillPressed = _controlActions.Controller.Skill.ReadValue<float>() > 0f;
        }
    }
}

/// <summary>
/// Apply inputs that need to be read at a variable rate
/// </summary>
[UpdateInGroup(typeof(SimulationSystemGroup), OrderFirst = true)]
[UpdateBefore(typeof(FixedStepSimulationSystemGroup))]
[BurstCompile]
public partial struct ThirdPersonPlayerVariableStepControlSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate(SystemAPI.QueryBuilder().WithAll<ThirdPersonPlayer, ThirdPersonPlayerInputs>().Build());
    }

    public void OnDestroy(ref SystemState state)
    { }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (playerInputs, player) in SystemAPI.Query<ThirdPersonPlayerInputs, ThirdPersonPlayer>().WithAll<Simulate>())
        {
            if (SystemAPI.HasComponent<OrbitCameraControl>(player.ControlledCamera))
            {
                OrbitCameraControl cameraControl = SystemAPI.GetComponent<OrbitCameraControl>(player.ControlledCamera);

                cameraControl.FollowedCharacterEntity = player.ControlledCharacter;
                cameraControl.Look = playerInputs.CameraLookInput;
                cameraControl.Zoom = playerInputs.CameraZoomInput;

                SystemAPI.SetComponent(player.ControlledCamera, cameraControl);
            }
        }
    }
}

/// <summary>
/// Apply inputs that need to be read at a fixed rate.
/// It is necessary to handle this as part of the fixed step group, in case your framerate is lower than the fixed step rate.
/// </summary>
[UpdateInGroup(typeof(FixedStepSimulationSystemGroup), OrderFirst = true)]
[BurstCompile]
public partial struct ThirdPersonPlayerFixedStepControlSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<FixedTickSystem.Singleton>();
        state.RequireForUpdate(SystemAPI.QueryBuilder().WithAll<ThirdPersonPlayer, ThirdPersonPlayerInputs>().Build());
    }

    public void OnDestroy(ref SystemState state)
    { }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        uint fixedTick = SystemAPI.GetSingleton<FixedTickSystem.Singleton>().Tick;

        foreach (var (playerInputs, player) in SystemAPI.Query<RefRW<ThirdPersonPlayerInputs>, ThirdPersonPlayer>().WithAll<Simulate>())
        {
            if (SystemAPI.HasComponent<ThirdPersonCharacterControl>(player.ControlledCharacter) && SystemAPI.HasComponent<CharacterStateMachine>(player.ControlledCharacter))
            {
                ThirdPersonCharacterControl characterControl = SystemAPI.GetComponent<ThirdPersonCharacterControl>(player.ControlledCharacter);
                CharacterStateMachine stateMachine = SystemAPI.GetComponent<CharacterStateMachine>(player.ControlledCharacter);

                float3 characterUp = MathUtilities.GetUpFromRotation(SystemAPI.GetComponent<LocalTransform>(player.ControlledCharacter).Rotation);

                // Get camera rotation data, since our movement is relative to it
                quaternion cameraRotation = quaternion.identity;
                if (SystemAPI.HasComponent<LocalTransform>(player.ControlledCamera))
                {
                    cameraRotation = SystemAPI.GetComponent<LocalTransform>(player.ControlledCamera).Rotation;
                }

                stateMachine.GetMoveVectorFromPlayerInput(stateMachine.CurrentState, in playerInputs.ValueRO, cameraRotation, out characterControl.MoveVector);

                characterControl.Attack = playerInputs.ValueRW.AttackPressed;
                characterControl.Skill = playerInputs.ValueRW.SkillPressed;
                SystemAPI.SetComponent(player.ControlledCharacter, characterControl);
            }
        }
    }
}