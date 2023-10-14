using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Gameplay
{
    public class FollowEntity : MonoBehaviour
    {
        [SerializeField] Entity _targetEntity;
        [SerializeField] float3 _offset;

        EntityManager _manager;


        void Awake()
        {
            _manager = World.DefaultGameObjectInjectionWorld.EntityManager;
        }

        void LateUpdate()
        {
            if (_targetEntity == null) return;
            var entPos = _manager.GetComponentData<LocalTransform>(_targetEntity);
            transform.position = entPos.Position + _offset;
        }
    }
}