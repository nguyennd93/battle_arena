using GPUECSAnimationBaker.Engine.AnimatorSystem;
using Unity.Entities;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class EnemySpawnerAuthoring : MonoBehaviour
{
    public CharacterPrefabInfo[] EnemyPrefabs;
    public int EnemyPerTurn;
    public float Interval;
    public float MinRadius;
    public float MaxRadius;

    public class Baker : Baker<EnemySpawnerAuthoring>
    {
        public override void Bake(EnemySpawnerAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new EnemySpawnerData()
            {
                EnemyPerTurn = authoring.EnemyPerTurn,
                Interval = authoring.Interval,
                CurrentTime = authoring.Interval,
                MinRadius = authoring.MinRadius,
                MaxRadius = authoring.MaxRadius,
                Random = Random.CreateFromIndex((uint)Mathf.RoundToInt(Time.time))
            });

            DynamicBuffer<EnemyPrefabBufferElement> enemyPrefabs = AddBuffer<EnemyPrefabBufferElement>(entity);
            foreach (var prefabInfo in authoring.EnemyPrefabs)
            {
                enemyPrefabs.Add(new EnemyPrefabBufferElement()
                {
                    Type = prefabInfo.Type,
                    PrefabEntity = GetEntity(prefabInfo.Prefab, prefabInfo.Prefab.GetComponent<GpuEcsAnimatorBehaviour>().transformUsageFlags)
                });
            }
        }
    }
}