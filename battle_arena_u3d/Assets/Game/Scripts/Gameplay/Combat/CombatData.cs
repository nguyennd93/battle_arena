using System.Collections;
using System.Collections.Generic;
using Unity.CharacterController;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct EffectData : IComponentData
{
    public float Lifetime;
}

public struct ProjectileData : IComponentData
{
    public int Damage;
    public float Speed;
    public float3 Direction;
    public float Lifetime;
}

public struct ReceiveDamageElementData : IBufferElementData
{
    public int Damage;
}

public struct SentDamageElementData : IBufferElementData
{
    public float3 Raidus;
    public int Damage;
    public float3 Direct;
}

