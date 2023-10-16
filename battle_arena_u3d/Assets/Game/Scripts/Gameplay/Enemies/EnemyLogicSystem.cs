using GPUECSAnimationBaker.Engine.AnimatorSystem;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;

namespace Gameplay
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateAfter(typeof(EnemySpawnerSystem))]
    public partial struct EnemyLogicSystem : ISystem
    {
        // protected override void OnUpdate(ref State)
        // {
        //     foreach (var enemyAspect in SystemAPI.Query<EnemyAspect>())
        //     {
        //         enemyAspect.ForceMove();
        //     }
        // }
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var enemyAspect in SystemAPI.Query<EnemyAspect>())
            {
                enemyAspect.ForceMove();
            }
            // EndSimulationEntityCommandBufferSystem.Singleton ecbSystem = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
            // EntityCommandBuffer.ParallelWriter ecb = ecbSystem.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter();


        }
    }
}