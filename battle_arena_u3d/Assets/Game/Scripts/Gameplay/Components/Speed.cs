using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

namespace Gameplay
{
    public struct Speed : IComponentData
    {
        public float Value;
    }
}