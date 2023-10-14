using System.Collections;
using System.Collections.Generic;
using Gameplay;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField] float3 _offset;

    EntityManager _manager;
    Entity _targetEntity;
    EntityQuery _entityQuery;

    void Awake()
    {
        _manager = World.DefaultGameObjectInjectionWorld.EntityManager;
        _entityQuery = _manager.CreateEntityQuery(new EntityQueryDesc
        {
            All = new ComponentType[] {
                typeof(PlayerTag)
            }
        });
    }

    void LateUpdate()
    {
        Debug.LogError(_targetEntity == null ? "Entity is NULL" : "Entity Not null!");
        if (_targetEntity == null)
        {
            _targetEntity = _entityQuery.GetSingletonEntity();
            return;
        }
        var entPos = _manager.GetComponentData<LocalTransform>(_targetEntity);
        transform.position = entPos.Position + _offset;
        transform.rotation = entPos.Rotation;
    }
}
