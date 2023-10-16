using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

namespace Gameplay
{
    public struct PlayerInputs : IComponentData
    {
        public float2 Move;
    }
}