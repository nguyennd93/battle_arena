using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class EnemyTagAuthoring : MonoBehaviour
{
    class Baker : Baker<EnemyTagAuthoring>
    {
        public override void Bake(EnemyTagAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new EnemyTag());
        }
    }
}