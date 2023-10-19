using System.Collections;
using System.Collections.Generic;
using Unity.CharacterController;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;


public class CharacterDataAuthoring : MonoBehaviour
{
    public CharacterData Data;

    class Baker : Baker<CharacterDataAuthoring>
    {
        public override void Bake(CharacterDataAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, authoring.Data);
            AddComponent(entity, new CharacterState()
            {
                IntervalSkill = 0f,
                IntervalAttack = 0f,
                Dead = false,
                Attack = false,
            });
            AddBuffer<ReceiveDamageElementData>(entity);
            AddBuffer<SentDamageElementData>(entity);
        }
    }
}