using System.Collections;
using System.Collections.Generic;
using Unity.CharacterController;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct ProjectilePrefabData : IComponentData
{
    public Entity Prefab;
    public float Speed;
}

public struct ProjectileData : IComponentData
{
    public float Speed;
    public float3 Direction;
}