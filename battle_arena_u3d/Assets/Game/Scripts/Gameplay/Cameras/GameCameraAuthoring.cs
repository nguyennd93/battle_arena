using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.Entities;

public class GameCameraAuthoring : MonoBehaviour
{
    public GameObject Follower;

    public class Baker : Baker<GameCameraAuthoring>
    {
        public override void Bake(GameCameraAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new CinematicCameraControl() { FollowedEntity = GetEntity(authoring.Follower, TransformUsageFlags.None) });
        }
    }
}
