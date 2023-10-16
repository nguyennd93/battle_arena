using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Gameplay
{
    [System.Serializable]
    public struct GlobalGravityData : IComponentData
    {
        public float3 Gravity;
    }

    [System.Serializable]
    public struct CustomGravity : IComponentData
    {
        public float GravityMultiplier;

        [HideInInspector] public float3 Gravity;
        [HideInInspector] public bool TouchedByNonGlobalGravity;
        [HideInInspector] public Entity CurrentZoneEntity;
        [HideInInspector] public Entity LastZoneEntity;
    }

    [System.Serializable]
    public struct SphericalGravityZone : IComponentData
    {
        public float GravityStrengthAtCenter;
    }
}