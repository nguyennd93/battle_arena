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
public partial class EnemyDestroySystem : SystemBase
{
    protected override void OnCreate()
    {
    }

    protected override void OnUpdate()
    {
        var gameInfoEntity = SystemAPI.GetSingletonEntity<GameConfig>();
        Entities.WithAll<DeadTag>().WithNone<DisableTag, PlayerTag>()
            .WithDeferredPlaybackSystem<EndSimulationEntityCommandBufferSystem>()
            .ForEach((Entity entity, EntityCommandBuffer ecb, ref CharacterData data, ref LocalTransform transform) =>
            {
                ecb.AddComponent<DisableTag>(entity);
                transform.Position = new float3(3000, -500, 3000);
                // ecb.DestroyEntity(entity);
            }).ScheduleParallel();
    }
}
