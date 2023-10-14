using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

namespace Gameplay
{
    public struct MoveInput : IComponentData
    {
        public float2 Value;
    }

    public struct PlayerTag : IComponentData
    {

    }
}