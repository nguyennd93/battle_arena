using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Gameplay
{
    public class PlayerAuthoring : MonoBehaviour
    {
        [SerializeField] float _speed;

        public float Speed { get { return _speed; } }
    }

    public class PlayerAuthoringBaker : Baker<PlayerAuthoring>
    {
        public override void Bake(PlayerAuthoring authoring)
        {
            var playerEntity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent<Speed>(playerEntity, new Speed() {
                Value = authoring.Speed
            });
            AddComponent<MoveInput>(playerEntity);
            AddComponent<PlayerTag>(playerEntity);
        }
    }
}