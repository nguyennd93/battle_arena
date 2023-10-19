using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Burst;
using Unity.CharacterController;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;


[UpdateInGroup(typeof(SimulationSystemGroup), OrderLast = true)]
public partial struct EnemyDestroySystem : ISystem
{
    EntityQuery _enemyQuery;

    public void OnCreate(ref SystemState state)
    {
        EntityQueryBuilder builder = new EntityQueryBuilder(Allocator.Temp).WithAll<DeadTag>();
        _enemyQuery = state.GetEntityQuery(builder);
        state.RequireForUpdate(_enemyQuery);
    }

    public void OnUpdate(ref SystemState state)
    {
        EndSimulationEntityCommandBufferSystem.Singleton ecbSystem = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
        EntityCommandBuffer.ParallelWriter ecb = ecbSystem.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter();
        
        
        new EnemyDestroyJob()
        {
            ecb = ecb
        }.Run(_enemyQuery);
    }

    private partial struct EnemyDestroyJob : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter ecb;

        [ReadOnly] public float deltaTime;

        public void Execute(Entity entity, [ChunkIndexInQuery] int sortKey)
        {
            ecb.DestroyEntity(sortKey, entity);
        }
    }
}