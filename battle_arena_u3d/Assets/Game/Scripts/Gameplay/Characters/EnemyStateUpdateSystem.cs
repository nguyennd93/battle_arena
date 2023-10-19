using Unity.Collections;
using Unity.Entities;


[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial class EnemyStateUpdateSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = SystemAPI.Time.DeltaTime;
        var ecb = new EntityCommandBuffer(Allocator.Temp);
        foreach (var (state, entity) in SystemAPI.Query<RefRW<CharacterState>>().WithNone<DeadTag, PlayerTag>().WithEntityAccess())
        {
            state.ValueRW.IntervalAttack -= deltaTime;
            state.ValueRW.IntervalSkill -= deltaTime;
        }
        ecb.Playback(EntityManager);
        ecb.Dispose();
    }
}