using UnityEngine;
using Unity.Entities;

public class AIControllerAuthoring : MonoBehaviour
{
    public AIData Data;

    class Baker : Baker<AIControllerAuthoring>
    {
        public override void Bake(AIControllerAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, authoring.Data);
        }
    }
}