using Unity.Collections;
using Unity.Entities;
using UnityEngine;


[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial class PlayerStateUpdateSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = SystemAPI.Time.DeltaTime;
        var ecb = new EntityCommandBuffer(Allocator.Temp);
        foreach (var (state, data, entity) in SystemAPI.Query<RefRW<CharacterState>, CharacterData>().WithAll<PlayerTag>().WithEntityAccess())
        {
            state.ValueRW.IntervalAttack -= deltaTime;
            state.ValueRW.IntervalSkill -= deltaTime;

            if (GameFlow.Instance != null)
            {
                GameFlow.Instance.OnUserHP(data.HP, data.TotalHP);
                GameFlow.Instance.OnSkillReload(Mathf.Max(0f, state.ValueRW.IntervalSkill), data.AttackRate);
            }
        }
        ecb.Playback(EntityManager);
        ecb.Dispose();
    }
}