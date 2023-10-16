using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Gameplay
{
    public enum EnemyType
    {
        Melee,
        Range
    }

    [System.Serializable]
    public class EnemyPrefanInfo
    {
        public EnemyType Type;
        public GameObject Prefab;
    }

    public struct EnemyData : IComponentData
    {
        public EnemyType Type;
        public float Speed;
        public int Damage;
        public int Coin;
    }
}