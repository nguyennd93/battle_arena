using System.Diagnostics;
using System.Linq;
using GPUECSAnimationBaker.Engine.AnimatorSystem;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
[UpdateInGroup(typeof(SimulationSystemGroup), OrderFirst = true)]
public partial struct EnemySpawnerSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<GameConfig>();
        state.RequireForUpdate<GameResource>();
        state.RequireForUpdate<EnemySpawnUpdate>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        BeginSimulationEntityCommandBufferSystem.Singleton ecbSystem = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
        EntityCommandBuffer.ParallelWriter ecb = ecbSystem.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter();
        float deltaTime = SystemAPI.Time.DeltaTime;


        state.Dependency = new EnemySpawnerJob()
        {
            ecb = ecb,
            deltaTime = deltaTime
        }.ScheduleParallel(state.Dependency);
    }

    [BurstCompile]
    private partial struct EnemySpawnerJob : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter ecb;

        [ReadOnly] public float deltaTime;

        public void Execute(ref EnemySpawnUpdate spawnUpdate, ref DynamicBuffer<EnemyElementData> enemyBuffer, in GameResource gameResource, in GameConfig gameConfig, ref GameInfo gameInfo, [ChunkIndexInQuery] int sortKey)
        {
            spawnUpdate.CurrentTime += deltaTime;
            while (spawnUpdate.CurrentTime > gameConfig.IntervalSpawn)
            {
                spawnUpdate.CurrentTime = 0f;
                for (int i = 0; i < gameConfig.EnemyPerTurn; i++)
                {
                    float r = spawnUpdate.Random.NextFloat(gameConfig.EnemyRadiusSpawn * 0.7f, gameConfig.EnemyRadiusSpawn);
                    Vector3 randomPos = Quaternion.Euler(0, spawnUpdate.Random.NextFloat(0f, 360f), 0) * Vector3.forward * r;

                    CharacterType type = spawnUpdate.Random.NextBool() ? CharacterType.EnemyMelee : CharacterType.EnemyMelee;
                    var entity = CreateEnemy(type, ref spawnUpdate, in gameResource, sortKey, randomPos);
                    if (type == CharacterType.EnemyMelee)
                        gameInfo.MeleeSpawn++;
                    else
                        gameInfo.RangeSpawn++;

                    enemyBuffer.Add(new EnemyElementData() { Enemy = entity });
                }
            }
        }

        private Entity CreateEnemy(CharacterType type, ref EnemySpawnUpdate spawnUpdate, in GameResource gameResource, int sortKey, float3 baseOffset)
        {
            Entity newEnemy = ecb.Instantiate(sortKey, type == CharacterType.EnemyMelee ? gameResource.PrefabEnemyMelee : gameResource.PrefabEnemyRange);
            ecb.SetComponent(sortKey, newEnemy, new LocalTransform()
            {
                Position = baseOffset,
                Rotation = quaternion.Euler(0, spawnUpdate.Random.NextFloat(-math.PI, math.PI), 0),
                Scale = 0.7f
            });

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
            return newEnemy;
        }
    }
}