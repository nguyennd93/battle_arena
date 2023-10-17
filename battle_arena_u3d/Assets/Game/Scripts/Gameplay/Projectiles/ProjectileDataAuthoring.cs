using System.Collections;
using System.Collections.Generic;
using Unity.CharacterController;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;


[System.Serializable]
public class ProjectileDataAuthoring : MonoBehaviour
{
    public GameObject Prefab;
    public float Speed;

    class Baker : Baker<ProjectileDataAuthoring>
    {
        public override void Bake(ProjectileDataAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new ProjectilePrefabData {
                Prefab = GetEntity(authoring.Prefab, TransformUsageFlags.None),
                Speed = authoring.Speed
            });
        }
    }
}