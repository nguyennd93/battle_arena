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
                Damage = aspect.CharacterData.ValueRO.Damage,
                Direction = aspect.WorldTransform.ValueRO.Forward,
                Speed = context.GameConfig.ProjectileSpeed,
                Lifetime = context.GameConfig.ProjectileLifetime
            });
        }
        else
        {
            if (aspect.CharacterData.ValueRO.Type == CharacterType.Main)
            {
                var entity = context.EndFrameFCB.Instantiate(context.ChunkIndex, context.GameResource.PrefabPlayerAttack);
                context.EndFrameFCB.SetComponent(context.ChunkIndex, entity, new LocalTransform()
                {
                    Position = aspect.Transform.ValueRO.Position + new float3(0f, 1.5f, 0f),
                    Rotation = quaternion.LookRotation(aspect.WorldTransform.ValueRO.Forward, new float3(0f, 1f, 0f)),
                    Scale = 1f,
                });
                context.EndFrameFCB.AddComponent<EffectData>(context.ChunkIndex, entity, new EffectData()
                {
                    Lifetime = context.GameConfig.AttackLifetime
                });
            }
            aspect.SentDamageBuffers.Add(new SentDamageElementData()
            {
                Damage = aspect.CharacterData.ValueRO.Damage,
                Direct = aspect.WorldTransform.ValueRO.Forward
            });
        }
        aspect.StateData.ValueRW.IntervalAttack = aspect.CharacterData.ValueRW.AttackRate;
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
