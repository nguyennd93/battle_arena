using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;


[UpdateInGroup(typeof(SimulationSystemGroup), OrderLast = true)]
public partial class PlayerReceiveDamageSystem : SystemBase
{
    protected override void OnUpdate()
    {
        var ecb = new EntityCommandBuffer(Allocator.Temp);
        foreach (var (buffer, state, data, entity) in SystemAPI.Query<DynamicBuffer<ReceiveDamageElementData>, RefRW<CharacterState>, RefRW<CharacterData>>().WithNone<DeadTag>().WithEntityAccess())
        {
            int totalDamage = 0;
            foreach (var item in buffer)
                totalDamage += item.Damage;

            data.ValueRW.HP = Mathf.Max(0, data.ValueRW.HP - totalDamage);
            buffer.Clear();

            if (data.ValueRW.HP <= 0)
            {
                state.ValueRW.Dead = true;
            }
        }
        ecb.Playback(EntityManager);
        ecb.Dispose();
    }
}