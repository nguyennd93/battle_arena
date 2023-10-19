using System;
using Unity.Entities;
using Unity.Physics.Authoring;

[Serializable]
public struct AIData : IComponentData
{
    public float AttackRange;
}