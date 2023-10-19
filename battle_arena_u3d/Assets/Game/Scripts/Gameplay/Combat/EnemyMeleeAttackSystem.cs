using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial class EnemyMeleeAttackSystem : SystemBase
{
    private Entity _playerEntity;

    protected override void OnUpdate()
    {
        _playerEntity = SystemAPI.GetSingletonEntity<PlayerTag>();
        var playerPos = SystemAPI.GetComponent<LocalTransform>(_playerEntity).Position;
        DynamicBuffer<ReceiveDamageElementData> playerReceiveDamages = SystemAPI.GetBuffer<ReceiveDamageElementData>(_playerEntity);

        var ecb = new EntityCommandBuffer(Allocator.Temp);
        foreach (var (enemyDamages, chaState, data, transform, entity) in SystemAPI.Query<DynamicBuffer<SentDamageElementData>, RefRW<CharacterState>, CharacterData, LocalTransform>().WithNone<DeadTag>().WithEntityAccess())
        {
            foreach (var enemyDamage in enemyDamages)
            {
                var playerDirect = math.normalize(transform.Position - playerPos);
                if (math.distance(transform.Position, playerPos) <= data.AttackRange && IsSameDirection(enemyDamage.Direct.xz, playerDirect.xz))
                {
                    playerReceiveDamages.Add(new ReceiveDamageElementData()
                    {
                        Damage = enemyDamage.Damage
                    });
                }
            }
            enemyDamages.Clear();
        }
        ecb.Playback(EntityManager);
        ecb.Dispose();
    }

    public bool IsSameDirection(float2 begin, float2 end)
    {
        return Vector2.Angle(begin, end) > 100f;
    }
}