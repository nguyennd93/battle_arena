using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial struct EnemyReceiveDamageSystem : ISystem
{
    EntityQuery _enemyQuery;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        EntityQueryBuilder builder = new EntityQueryBuilder(Allocator.Temp).WithAll<CharacterData, CharacterState, LocalTransform>().WithNone<PlayerTag>();
        _enemyQuery = state.GetEntityQuery(builder);
        state.RequireForUpdate(_enemyQuery);
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var playerEntity = SystemAPI.GetSingletonEntity<PlayerTag>();
        var position = SystemAPI.GetComponent<LocalTransform>(playerEntity).Position;
        var damage = SystemAPI.GetComponent<CharacterData>(playerEntity).Damage;
        var aspect = SystemAPI.GetAspect<ThirdPersonCharacterAspect>(playerEntity);
        // var skillDamages = SystemAPI.GetBuffer<SkillDamageBufferElementData>(playerEntity);

        state.Dependency = new EnemyUpdateHPJob()
        {
            // Skills = skillDamages,
            Target = position,
            Damage = damage,
            PlayerAction = aspect.StateMachine.ValueRO.CurrentState,
            Direction = aspect.WorldTransform.ValueRO.Forward,
            AttackRange = aspect.CharacterData.ValueRO.AttackRange
        }.ScheduleParallel(_enemyQuery, state.Dependency);
    }

    [BurstCompile]
    private partial struct EnemyUpdateHPJob : IJobEntity
    {
        // [ReadOnly] public DynamicBuffer<SkillDamageBufferElementData> Skills;
        public float3 Direction;
        public float3 Target;
        public float AttackRange;

        public int Damage;
        public StateType PlayerAction;

        public void Execute(ref CharacterData characterData, ref CharacterState state, ref LocalTransform transform)
        {
            int damageOnSkill = 0;
            float rangeBonus = 0f;
            bool haveSkill = false;

            var enemyDirect = math.normalize(Target - transform.Position);
            if (math.distance(Target, transform.Position) <= math.max(AttackRange, rangeBonus) && IsSameDirection(enemyDirect.xz, Direction.xz) && (PlayerAction == StateType.Attack || haveSkill))
            {
                if (characterData.HP > 0f)
                    characterData.HP -= Damage + damageOnSkill;
                else
                {
                    characterData.HP = 0;
                    state.Dead = true;
                }
            }
        }

        public bool IsSameDirection(float2 begin, float2 end)
        {
            return Vector2.Angle(begin, end) > 100f;
        }
    }
}