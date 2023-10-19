using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class GameControllerAuthoring : MonoBehaviour
{
    [SerializeField] public GameConfig Config;
    [SerializeField] public GameResourceConfig Resource;

    class Baker : Baker<GameControllerAuthoring>
    {
        public override void Bake(GameControllerAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent<GameConfig>(entity, authoring.Config);
            AddComponent<GameResource>(entity, new GameResource()
            {
                PrefabEnemyMelee = GetEntity(authoring.Resource.PrefabEnemyMelee, TransformUsageFlags.Dynamic),
                PrefabEnemyRange = GetEntity(authoring.Resource.PrefabEnemyRange, TransformUsageFlags.Dynamic),
                PrefabEnemyProjectile = GetEntity(authoring.Resource.PrefabEnemyProjectile, TransformUsageFlags.Dynamic),
                PrefabPlayerAttack = GetEntity(authoring.Resource.PrefabPlayerAttack, TransformUsageFlags.Dynamic),
                PrefabPlayerSkill = GetEntity(authoring.Resource.PrefabPlayerSkill, TransformUsageFlags.Dynamic)
            });
            AddComponent<EnemySpawnUpdate>(entity, new EnemySpawnUpdate()
            {
                CurrentTime = authoring.Config.IntervalSpawn,
                Random = Unity.Mathematics.Random.CreateFromIndex((uint)Mathf.RoundToInt(Time.time))
            });
            AddComponent<GameInfo>(entity, new GameInfo()
            {
                State = GameplayState.Init,
                TotalKill = 0,
                UserHP = authoring.Config.MaximumHP
            });
        }
    }
}
