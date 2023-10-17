using System;
using Unity.Entities;
using Unity.Physics.Authoring;

[Serializable]
public struct AIController : IComponentData
{
    public float DistanceCanAttack;
}