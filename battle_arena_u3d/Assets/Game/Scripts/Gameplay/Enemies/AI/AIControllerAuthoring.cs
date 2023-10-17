using UnityEngine;
using Unity.Entities;

public class AIControllerAuthoring : MonoBehaviour
{
    public AIController AIController;

    class Baker : Baker<AIControllerAuthoring>
    {
        public override void Bake(AIControllerAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, authoring.AIController);
        }
    }
}