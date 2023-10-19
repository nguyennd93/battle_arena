using System.Collections;
using System.Collections.Generic;
using Unity.CharacterController;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct ProjectileData : IComponentData
{
    public float Speed;
    public float3 Direction;
    public float Lifetime;
}