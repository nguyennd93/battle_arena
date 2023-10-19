

using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(SimulationSystemGroup), OrderLast = true)]
public partial class GameResetSystem : SystemBase
{
    protected override void OnUpdate()
    {
        var ecb = new EntityCommandBuffer(Allocator.Temp);
        foreach (var (forceReset, enemyBuffers, entity) in SystemAPI.Query<RefRW<ForceResetGame>, DynamicBuffer<EnemyElementData>>().WithEntityAccess())
        {
            if (forceReset.ValueRW.ForceReset)
            {
                foreach (var element in enemyBuffers)
                    ecb.DestroyEntity(element.Enemy);
                enemyBuffers.Clear();
                forceReset.ValueRW.ForceReset = false;
            }
        }
        ecb.Playback(EntityManager);
        ecb.Dispose();
    }
}