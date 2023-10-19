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
using UnityEngine.VFX;

[BurstCompile]
[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial struct ProjectileMovingSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<ProjectileData>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        BeginSimulationEntityCommandBufferSystem.Singleton ecbSystem = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
        EntityCommandBuffer.ParallelWriter ecb = ecbSystem.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter();

        var position = SystemAPI.GetComponent<LocalTransform>(SystemAPI.GetSingletonEntity<PlayerTag>()).Position;
        new MovingJob()
        {
            ecb = ecb,
            PlayerPosition = position,
            DeltaTime = SystemAPI.Time.DeltaTime
        }.Run();
    }

    [BurstCompile]
    partial struct MovingJob : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter ecb;
        public float3 PlayerPosition;
        public float DeltaTime;

        public void Execute(ref LocalTransform transform, ref ProjectileData data, Entity entity, [ChunkIndexInQuery] int sortKey)
        {
            transform.Position.xz += data.Direction.xz * data.Speed * DeltaTime;
            data.Lifetime -= DeltaTime;
            if (math.distance(transform.Position, PlayerPosition) < 1.6f || data.Lifetime < 0f)
                ecb.DestroyEntity(sortKey, entity);
        }
    }
}