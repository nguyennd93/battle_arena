using Unity.Entities;
using Unity.Mathematics;

namespace Gameplay
{
    public struct EnemySpawnerData : IComponentData
    {
        public int EnemyPerTurn;
        public float Interval;
        public float CurrentTime;
        public float MinRadius;
        public float MaxRadius;
        public Random Random;
    }

    public struct EnemyPrefabBufferElement : IBufferElementData
    {
        public Entity PrefabEntity;
        public EnemyType Type;
    }
}