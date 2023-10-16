using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class GlobalGravityAuthoring : MonoBehaviour
{
    public float3 Gravity;

    class Baker : Baker<GlobalGravityAuthoring>
    {
        public override void Bake(GlobalGravityAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new GlobalGravityData { Gravity = authoring.Gravity });
        }
    }
}