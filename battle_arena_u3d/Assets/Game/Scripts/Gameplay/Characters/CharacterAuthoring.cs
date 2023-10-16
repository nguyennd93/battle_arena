using System.Collections;
using System.Collections.Generic;
using Unity.CharacterController;
using Unity.Entities;
using UnityEngine;

namespace Gameplay
{
    public class CharacterAuthoring : MonoBehaviour
    {
        [Header("References")]
        public GameObject MeshPrefab;

        public class Baker : Baker<CharacterAuthoring>
        {
            public override void Bake(CharacterAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponentObject(entity, new CharacterData { MeshPrefab = authoring.MeshPrefab });
            }
        }
    }
}