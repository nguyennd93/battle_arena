using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Gameplay
{
    [Serializable]
    public class CharacterData : IComponentData
    {
        public GameObject MeshPrefab;
    }
}