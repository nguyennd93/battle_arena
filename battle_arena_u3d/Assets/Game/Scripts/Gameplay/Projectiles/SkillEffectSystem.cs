// using System.Collections;
// using System.Collections.Generic;
// using Unity.Burst;
// using Unity.Burst.Intrinsics;
// using Unity.CharacterController;
// using Unity.Collections;
// using Unity.Entities;
// using Unity.Mathematics;
// using Unity.Transforms;
// using UnityEngine;
// using UnityEngine.VFX;

// [BurstCompile]
// [UpdateInGroup(typeof(SimulationSystemGroup))]
// public partial struct SkillEffectSystem : ISystem
// {
//     [BurstCompile]
//     public void OnCreate(ref SystemState state)
//     {
//         state.RequireForUpdate<SkillData>();
//     }

//     [BurstCompile]
//     public void OnUpdate(ref SystemState state)
//     {
//         BeginSimulationEntityCommandBufferSystem.Singleton ecbSystem = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
//         EntityCommandBuffer.ParallelWriter ecb = ecbSystem.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter();
//         new UpdateJob()
//         {
//             ecb = ecb,
//             DeltaTime = SystemAPI.Time.DeltaTime
//         }.Run();
//     }

//     [BurstCompile]
//     partial struct UpdateJob : IJobEntity
//     {
//         public EntityCommandBuffer.ParallelWriter ecb;
//         public float DeltaTime;

//         public void Execute(ref SkillData data, Entity entity, [ChunkIndexInQuery] int sortKey)
//         {
//             data.Lifetime -= DeltaTime;
//             if (data.Lifetime <= 0f)
//                 ecb.DestroyEntity(sortKey, entity);
//         }
//     }
// }