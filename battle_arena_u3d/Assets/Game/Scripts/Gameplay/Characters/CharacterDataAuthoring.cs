using System.Collections;
using System.Collections.Generic;
using Unity.CharacterController;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;


public class CharacterDataAuthoring : MonoBehaviour
{
    public CharacterData Data;
    public float AttackRate;

    class Baker : Baker<CharacterDataAuthoring>
    {
        public override void Bake(CharacterDataAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, authoring.Data);
            AddComponent(entity, new CharacterState()
            {
                AttackRate = authoring.AttackRate,
                IntervalAttack = 0f
            });
        }
    }
}