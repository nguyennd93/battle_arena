using System.Collections;
using System.Collections.Generic;
using Unity.CharacterController;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;


public enum CharacterType
{
    Main,
    EnemyMelee,
    EnemyRange
}

public struct PlayerTag : IComponentData { }
public struct EnemyTag : IComponentData { }

[System.Serializable]
public struct CharacterData : IComponentData
{
    public CharacterType Type;
    public int HP;
    public float Speed;
    public int Damage;
    public int Coin;

    public static CharacterData Default()
    {
        return new CharacterData()
        {
            Type = CharacterType.EnemyMelee,
            HP = 100,
            Speed = 3,
            Damage = 10,
            Coin = 100
        };
    }
}


[System.Serializable]
public class CharacterPrefabInfo
{
    public CharacterType Type;
    public GameObject Prefab;
    public GameObject PrefabProjectile;
}