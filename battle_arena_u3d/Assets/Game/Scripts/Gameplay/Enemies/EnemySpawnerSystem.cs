using System.Diagnostics;
using System.Linq;
using GPUECSAnimationBaker.Engine.AnimatorSystem;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Gameplay
{
    [BurstCompile]
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public partial struct EnemySpawnerSystem : ISystem
    {
        private BufferLookup<GpuEcsAnimationDataBufferElement> gpuEcsAnimationDataBufferLookup;
        private ComponentLookup<LocalTransform> localTransformLookup;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            gpuEcsAnimationDataBufferLookup = state.GetBufferLookup<GpuEcsAnimationDataBufferElement>(isReadOnly: true);
            localTransformLookup = state.GetComponentLookup<LocalTransform>(isReadOnly: true);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            gpuEcsAnimationDataBufferLookup.Update(ref state);
            localTransformLookup.Update(ref state);
            EndSimulationEntityCommandBufferSystem.Singleton ecbSystem = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
            EntityCommandBuffer.ParallelWriter ecb = ecbSystem.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter();
            float deltaTime = SystemAPI.Time.DeltaTime;

            state.Dependency = new EnemySpawnerJob()
            {
                ecb = ecb,
                deltaTime = deltaTime,
                gpuEcsAnimationDataBufferLookup = gpuEcsAnimationDataBufferLookup,
                localTransformLookup = localTransformLookup
            }.ScheduleParallel(state.Dependency);
        }

        [BurstCompile]
        private partial struct EnemySpawnerJob : IJobEntity
        {
            public EntityCommandBuffer.ParallelWriter ecb;
            [ReadOnly] public float deltaTime;
            [ReadOnly] public BufferLookup<GpuEcsAnimationDataBufferElement> gpuEcsAnimationDataBufferLookup;
            [ReadOnly] public ComponentLookup<LocalTransform> localTransformLookup;

            public void Execute(ref EnemySpawnerData spawnData, in DynamicBuffer<EnemyPrefabBufferElement> enemyPrefabs, [ChunkIndexInQuery] int sortKey)
            {
                spawnData.CurrentTime += deltaTime;
                while (spawnData.CurrentTime > spawnData.Interval)
                {
                    spawnData.CurrentTime = 0f;

                    for (int i = 0; i < spawnData.EnemyPerTurn; i++)
                    {
                        float r = spawnData.Random.NextFloat(spawnData.MinRadius, spawnData.MaxRadius);
                        Vector3 randomPos = Quaternion.Euler(0, spawnData.Random.NextFloat(0f, 360f), 0) * Vector3.forward * r;
                        CreateEnemy(ref spawnData, sortKey, randomPos, enemyPrefabs);
                    }
                }
            }

            private Entity CreateEnemy(ref EnemySpawnerData spawnData, int sortKey, float3 baseOffset, in DynamicBuffer<EnemyPrefabBufferElement> enemyPrefabs)
            {
                var prefab = enemyPrefabs[spawnData.Random.NextInt(0, enemyPrefabs.Length)];

                Entity newEnemy = ecb.Instantiate(sortKey, prefab.PrefabEntity);

                // Transform Component
                ecb.SetComponent(sortKey, newEnemy, new LocalTransform()
                {
                    Position = baseOffset,
                    Rotation = quaternion.Euler(0, spawnData.Random.NextFloat(-math.PI, math.PI), 0),
                    Scale = localTransformLookup[prefab.PrefabEntity].Scale
                });

                // Animation Component
                ecb.SetComponent(sortKey, newEnemy, new GpuEcsAnimatorControlComponent()
                {
                    animatorInfo = new AnimatorInfo()
                    {
                        animationID = 0,
                        blendFactor = 0,
                        speedFactor = 1f
                    },
                    startNormalizedTime = 0f,
                    transitionSpeed = 0
                });

                // Enemy Data Component
                bool isRange = prefab.Type == EnemyType.Range;
                ecb.AddComponent(sortKey, newEnemy, new EnemyData()
                {
                    Type = prefab.Type,
                    Speed = isRange ? 2f : 3f,
                    Coin = isRange ? 100 : 200,
                    Damage = isRange ? 50 : 100
                });
                return newEnemy;
            }
        }
    }
}