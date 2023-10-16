using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Gameplay
{
    public class PlayerAuthoring : MonoBehaviour
    {
        public GameObject Character;
        public GameObject Camera;

        public class Baker : Baker<PlayerAuthoring>
        {
            public override void Bake(PlayerAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new MainPlayer
                {
                    Character = GetEntity(authoring.Character, TransformUsageFlags.Dynamic),
                    Camera = GetEntity(authoring.Camera, TransformUsageFlags.Dynamic),
                });
                AddComponent(entity, new PlayerInputs());
            }
        }
    }
}