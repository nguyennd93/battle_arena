

// using System.Collections;
// using System.Collections.Generic;
// using GPUECSAnimationBaker.Engine.AnimatorSystem;
// using Unity.Burst;
// using Unity.CharacterController;
// using Unity.Collections;
// using Unity.Entities;
// using Unity.Mathematics;
// using Unity.Physics;
// using Unity.Transforms;
// using UnityEditor;
// using UnityEngine;


// [BurstCompile]
// [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
// public partial struct CleanerSystem : ISystem
// {
//     EntityQuery _enemyQuery;

//     [BurstCompile]
//     public void OnCreate(ref SystemState state)
//     {
//         EntityQueryBuilder builder = new EntityQueryBuilder(Allocator.Temp)
//             .WithAll<DisableTag>();
//         _enemyQuery = state.GetEntityQuery(builder);
//         state.RequireForUpdate(_enemyQuery);
//     }

//     [BurstCompile]
//     public void OnUpdate(ref SystemState state)
//     {
//         // EndSimulationEntityCommandBufferSystem.Singleton ecbSystem = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
//         // EntityCommandBuffer.ParallelWriter ecb = ecbSystem.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter();

//         // var position = SystemAPI.GetComponent<LocalTransform>(SystemAPI.GetSingletonEntity<PlayerTag>()).Position;
//         // state.Dependency = new DestroyJob()
//         // {
//         //     ECB = ecb
//         // }.ScheduleParallel(_enemyQuery, state.Dependency);
//     }

//     [BurstCompile]
//     partial struct DestroyJob : IJobEntity
//     {
//         public EntityCommandBuffer.ParallelWriter ECB;

//         public void Execute(Entity entity, [ChunkIndexInQuery] int sortKey)
//         {
//             // ECB.RemoveComponent<ThirdPersonCharacterControl>(sortKey, entity);
//             // ECB.RemoveComponent<ThirdPersonCharacterComponent>(sortKey, entity);
//             // ECB.RemoveComponent<StoredKinematicCharacterData>(sortKey, entity);
//             // ECB.RemoveComponent<SentDamageElementData>(sortKey, entity);

//             // ECB.RemoveComponent<SceneTag>(sortKey, entity);
//             // ECB.RemoveComponent<SceneSection>(sortKey, entity);
//             // ECB.RemoveComponent<ReceiveDamageElementData>(sortKey, entity);

//             // ECB.RemoveComponent<PhysicsWorldIndex>(sortKey, entity);
//             // ECB.RemoveComponent<PhysicsVelocity>(sortKey, entity);
//             // ECB.RemoveComponent<PhysicsMass>(sortKey, entity);
//             // ECB.RemoveComponent<PhysicsGravityFactor>(sortKey, entity);
//             // ECB.RemoveComponent<PhysicsCustomTags>(sortKey, entity);
//             // ECB.RemoveComponent<PhysicsColliderKeyEntityPair>(sortKey, entity);
//             // ECB.RemoveComponent<PhysicsCollider>(sortKey, entity);
//             // ECB.RemoveComponent<LinkedEntityGroup>(sortKey, entity);
//             // ECB.RemoveComponent<KinematicVelocityProjectionHit>(sortKey, entity);
//             // ECB.RemoveComponent<KinematicCharacterProperties>(sortKey, entity);
//             // ECB.RemoveComponent<KinematicCharacterBody>(sortKey, entity);
//             // ECB.RemoveComponent<CharacterInterpolation>(sortKey, entity);

//             // // Animaiton
//             // ECB.RemoveComponent<GpuEcsCurrentAttachmentAnchorBufferElement>(sortKey, entity);
//             // ECB.RemoveComponent<GpuEcsAttachmentAnchorDataBufferElement>(sortKey, entity);
//             // ECB.RemoveComponent<GpuEcsAnimatorTransitionInfoComponent>(sortKey, entity);
//             // ECB.RemoveComponent<GpuEcsAnimatorStateComponent>(sortKey, entity);
//             // ECB.RemoveComponent<GpuEcsAnimatorShaderDataComponent>(sortKey, entity);
//             // ECB.RemoveComponent<GpuEcsAnimatorEventBufferElement>(sortKey, entity);
//             // ECB.RemoveComponent<GpuEcsAnimatorControlStateComponent>(sortKey, entity);
//             // ECB.RemoveComponent<GpuEcsAnimatorControlComponent>(sortKey, entity);
//             // ECB.RemoveComponent<GpuEcsAnimationEventOccurenceBufferElement>(sortKey, entity);
//             // ECB.RemoveComponent<GpuEcsAnimationDataComponent>(sortKey, entity);
//             // ECB.RemoveComponent<GpuEcsAnimationDataBufferElement>(sortKey, entity);
//             // ECB.RemoveComponent<GpuEcsAnimatorStateComponent>(sortKey, entity);
//             // ECB.RemoveComponent<GpuEcsAnimatorControlComponent>(sortKey, entity);
//             // ECB.RemoveComponent<GpuEcsAnimationDataComponent>(sortKey, entity);
//             // ECB.RemoveComponent<GpuEcsAnimationDataBufferElement>(sortKey, entity);
//         }
//     }
// }