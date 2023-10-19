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
        aspect.StateData.ValueRW.Attack = false;

        if (aspect.CharacterData.ValueRO.Type == CharacterType.EnemyRange)
        {
            var entity = context.EndFrameFCB.Instantiate(context.ChunkIndex, context.GameResource.PrefabEnemyProjectile);
            context.EndFrameFCB.SetComponent(context.ChunkIndex, entity, new LocalTransform()
            {
                Position = aspect.Transform.ValueRO.Position + new float3(0f, 1.5f, 0f),
                Scale = 1f
            });
            context.EndFrameFCB.AddComponent(context.ChunkIndex, entity, new ProjectileData()
            {
                Direction = aspect.CharacterComponent.ValueRO.Direction,
                Speed = context.GameConfig.ProjectileSpeed,
                Lifetime = context.GameConfig.ProjectileLifetime
            });
        }
        else if (aspect.CharacterData.ValueRO.Type == CharacterType.Main)
        {
            var entity = context.EndFrameFCB.Instantiate(context.ChunkIndex, context.GameResource.PrefabPlayerAttack);
            context.EndFrameFCB.SetComponent(context.ChunkIndex, entity, new LocalTransform()
            {
                Position = aspect.Transform.ValueRO.Position + new float3(0f, 1.5f, 0f),
                Rotation = quaternion.LookRotation(new float3(aspect.CharacterComponent.ValueRO.Direction.x, 0f, aspect.CharacterComponent.ValueRO.Direction.z), new float3(0f, 1f, 0f)),
                Scale = 1f,
            });
            context.EndFrameFCB.AddComponent<SkillData>(context.ChunkIndex, entity, new SkillData()
            {
                Lifetime = context.GameConfig.AttackLifetime
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
