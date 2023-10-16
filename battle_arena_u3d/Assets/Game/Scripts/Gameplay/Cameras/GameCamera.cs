using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Gameplay
{
    public class GameCamera : IComponentData
    {

    }

    [System.Serializable]
    public struct CinematicCameraControl : IComponentData
    {
        public Entity FollowedEntity;
    }
}
