using System.Collections;
using System.Collections.Generic;
using GPUECSAnimationBaker.Engine.AnimatorSystem;
using Unity.CharacterController;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public struct AttackState : ICharacterState
{
    public void OnStateEnter(StateType previousState, ref ThirdPersonCharacterUpdateContext context, ref KinematicCharacterUpdateContext baseContext, in ThirdPersonCharacterAspect aspect)
    {
        aspect.AnimationAspect.RunAnimation(2, 1f, 1f, 0, 0.3f);

        if (aspect.CharacterData.ValueRO.Type == CharacterType.EnemyRange && aspect.StateData.ValueRW.IntervalAttack <= 0f)
        {
            var entity = context.EndFrameECB.Instantiate(context.ChunkIndex, aspect.ProjectilePrefab.ValueRO.Prefab);
            context.EndFrameECB.SetComponent(context.ChunkIndex, entity, new LocalTransform()
            {
                Position = aspect.Transform.ValueRO.Position + new float3(0f, 1.7f, 0f),
                Scale = 0.4f
            });
            context.EndFrameECB.AddComponent(context.ChunkIndex, entity, new ProjectileData()
            {
                Direction = float3.zero,
                Speed = aspect.ProjectilePrefab.ValueRO.Speed
            });
        }


        aspect.StateData.ValueRW.IntervalAttack = aspect.StateData.ValueRW.AttackRate;
    }

    public void OnStateExit(StateType nextState, ref ThirdPersonCharacterUpdateContext context, ref KinematicCharacterUpdateContext baseContext, in ThirdPersonCharacterAspect aspect)
    {
    }

    public void OnStatePhysicsUpdate(ref ThirdPersonCharacterUpdateContext context, ref KinematicCharacterUpdateContext baseContext, in ThirdPersonCharacterAspect aspect)
    {
        // GpuEcsAnimatorStateComponent animState = aspect.AnimationState.ValueRO;

        if (aspect.AnimationAspect.IsStop())
        {
            ref CharacterStateMachine stateMachine = ref aspect.StateMachine.ValueRW;
            stateMachine.TransitionToState(StateType.Move, ref context, ref baseContext, in aspect);
        }
    }

    public void GetMoveVectorFromPlayerInput(in ThirdPersonPlayerInputs inputs, quaternion cameraRotation, out float3 moveVector)
    {
        ThirdPersonCharacterAspect.GetCommonMoveVectorFromPlayerInput(in inputs, cameraRotation, out moveVector);
    }
}
