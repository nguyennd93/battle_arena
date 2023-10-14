using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

namespace Gameplay
{
    public class FollowAuthoring : MonoBehaviour
    {
        [SerializeField] GameObject _targetObject;

        public GameObject Target { get { return _targetObject; } }
    }

    public class FollowAuthoringBaker : Baker<FollowAuthoring>
    {
        public override void Bake(FollowAuthoring authoring)
        {
            var playerEntity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent<FollowEntity>(playerEntity);
        }
    }
}