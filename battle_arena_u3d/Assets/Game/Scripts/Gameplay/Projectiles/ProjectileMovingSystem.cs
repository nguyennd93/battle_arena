using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.CharacterController;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;


[BurstCompile]
[UpdateAfter(typeof(ThirdPersonCharacterPhysicsUpdateSystem))]
public partial struct ProjectileMovingSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EndSimulationEntityCommandBufferSystem.Singleton ecbSystem = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
        EntityCommandBuffer.ParallelWriter ecb = ecbSystem.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter();

        var position = SystemAPI.GetComponent<LocalTransform>(SystemAPI.GetSingletonEntity<PlayerTag>()).Position;
        new MoveJob()
        {
            ecb = ecb,
            PlayerPosition = position,
            DeltaTime = SystemAPI.Time.DeltaTime
        }.ScheduleParallel();
    }

    [BurstCompile]
    partial struct MoveJob : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter ecb;
        public float3 PlayerPosition;
        public float DeltaTime;

        public void Execute(ref LocalTransform localTransform, ref ProjectileData data, Entity entity, [ChunkIndexInQuery] int sortKey)
        {
            if (math.distance(data.Direction, float3.zero) <= 0f)
                data.Direction = math.normalize(PlayerPosition - localTransform.Position);
            else
            {
                localTransform.Position.xz += data.Direction.xz * data.Speed * DeltaTime;

                if (math.distance(localTransform.Position, PlayerPosition) <= 1.8f || math.distance(localTransform.Position, float3.zero) >= 20f)
                {
                    ecb.DestroyEntity(sortKey, entity);
                }
            }
        }
    }
}