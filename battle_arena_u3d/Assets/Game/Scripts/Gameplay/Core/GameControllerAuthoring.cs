using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class GameControllerAuthoring : MonoBehaviour
{
    [SerializeField] public GameConfig Config;
    [SerializeField] public GameResourceConfig Resource;

    void Awake()
    {
#if !UNITY_EDITOR
        Config.EnemyPerTurn = PlayerPrefs.GetInt("KEY_AMOUNT", 10);
        Config.IntervalSpawn = PlayerPrefs.GetInt("KEY_INTERVAL", 1);
#endif
    }

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
                MeleeSpawn = 0,
                RangeSpawn = 0,
                MeleeDead = 0,
                RangeDead = 0
            });
            AddComponent<ForceResetGame>(entity, new ForceResetGame() { ForceReset = false });
            AddBuffer<EnemyElementData>(entity);
        }
    }
}
