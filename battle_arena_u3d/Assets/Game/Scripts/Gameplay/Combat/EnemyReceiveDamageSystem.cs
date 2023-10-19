using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial class EnemyReceiveDamageSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entity playerEntity = SystemAPI.GetSingletonEntity<PlayerTag>();
        var playerPosition = SystemAPI.GetComponent<LocalTransform>(playerEntity).Position;
        var playerAttackRange = SystemAPI.GetComponent<CharacterData>(playerEntity).AttackRange;

        DynamicBuffer<SentDamageElementData> playerSentDamages = SystemAPI.GetBuffer<SentDamageElementData>(playerEntity);

        var ecb = new EntityCommandBuffer(Allocator.Temp);
        foreach (var (enemyReceiveDamages, data, transform, entity) in SystemAPI.Query<DynamicBuffer<ReceiveDamageElementData>, CharacterData, LocalToWorld>().WithNone<DeadTag, PlayerTag>().WithEntityAccess())
        {
            int totalDamage = 0;
            foreach (var damageElement in playerSentDamages)
            {
                var distance = math.distance(playerPosition, transform.Position);
                var enemyDirect = math.normalize(playerPosition - transform.Position);
                if (damageElement.Radius > 0f && distance < damageElement.Radius)
                {
                    totalDamage += damageElement.Damage;
                }
                else if (distance <= playerAttackRange && IsSameDirection(enemyDirect.xz, damageElement.Direct.xz))
                {
                    totalDamage += damageElement.Damage;
                }
            }
            enemyReceiveDamages.Add(new ReceiveDamageElementData() { Damage = totalDamage });
        }
        ecb.Playback(EntityManager);
        ecb.Dispose();

        playerSentDamages.Clear();
    }

    public bool IsSameDirection(float2 begin, float2 end)
    {
        return Vector2.Angle(begin, end) > 130f;
    }
}