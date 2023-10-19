using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public enum GameplayState
{
    Init,
    Play,
    Pause
}

[System.Serializable]
public struct GameConfig : IComponentData
{
    [Header("Enemies")]
    public int EnemyPerTurn;
    public int IntervalSpawn;
    public float EnemyRadiusSpawn;
    public float ProjectileSpeed;
    public float ProjectileLifetime;

    [Header("Player")]
    public int MaximumHP;
    public float SkillLifetime;
    public float IntervalSkill;
}

[System.Serializable]
public struct GameInfo : IComponentData
{
    public GameplayState State;
    public int TotalKill;
    public int UserHP;
    public float IntervalSkill;

    public int CountMelee;
    public int CountRange;
}

[System.Serializable]
public struct GameResourceConfig
{
    [Header("Enemies")]
    public GameObject PrefabEnemyMelee;
    public GameObject PrefabEnemyRange;
    public GameObject PrefabEnemyProjectile;

    [Header("Player")]
    public GameObject PrefabPlayerSkill;
    public GameObject PrefabPlayerAttack;
}

public partial struct GameResource : IComponentData
{
    public Entity PrefabEnemyMelee;
    public Entity PrefabEnemyRange;
    public Entity PrefabEnemyProjectile;
    public Entity PrefabPlayerAttack;
    public Entity PrefabPlayerSkill;
}

public partial struct EnemySpawnUpdate : IComponentData
{
    public float CurrentTime;
    public Unity.Mathematics.Random Random;
}