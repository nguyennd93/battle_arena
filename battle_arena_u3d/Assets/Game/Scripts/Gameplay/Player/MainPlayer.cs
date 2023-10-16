using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Gameplay
{
    [System.Serializable]
    public struct MainPlayer : IComponentData
    {
        public Entity Character;
        public Entity Camera;
    }
}