using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.CharacterController;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial struct AIControllerSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var position = SystemAPI.GetComponent<LocalTransform>(SystemAPI.GetSingletonEntity<PlayerTag>()).Position;
        new QueryJob()
        {
            TargetPosition = position,
            DeltaTime = SystemAPI.Time.DeltaTime
        }.ScheduleParallel();
    }

    [BurstCompile]
    partial struct QueryJob : IJobEntity
    {
        public float3 TargetPosition;
        public float DeltaTime;

        public void Execute(ref ThirdPersonCharacterControl characterControl, ref ForceChangeState aiAction, ref CharacterState aiState, ref CharacterStateMachine machine, ref AIController aiController, ref LocalTransform localTransform)
        {
            aiState.IntervalAttack -= DeltaTime;
            var distance = math.distance(TargetPosition, localTransform.Position);
            if (distance <= aiController.DistanceCanAttack)
            {
                if (aiState.IntervalAttack <= 0f)
                {
                    characterControl.MoveVector = float3.zero;
                    aiAction.Force = true;
                    aiAction.State = StateType.Attack;
                }
                else
                {
                    characterControl.MoveVector = float3.zero;
                }
                localTransform.Rotation = quaternion.LookRotationSafe(math.normalize(TargetPosition - localTransform.Position), math.up());
            }
            else
                characterControl.MoveVector = math.normalizesafe(TargetPosition - localTransform.Position);
        }
    }
}