using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class GameCamera : IComponentData
{

}

[System.Serializable]
public struct CinematicCameraControl : IComponentData
{
    public Entity FollowedEntity;
}