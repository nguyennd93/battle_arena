using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public enum EnemyType
    {
        Melee = 0,
        Range = 1,
        None
    }

    [System.Serializable]
    public class EnemyConfig
    {
        public EnemyType Type;
        public GameObject Prefab;
        public int Gold;
        public int EXP;
    }
}