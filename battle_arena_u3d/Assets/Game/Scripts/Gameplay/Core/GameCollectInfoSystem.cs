using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;


[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial class GameCollectInfoSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entity _infoEntity = SystemAPI.GetSingletonEntity<GameInfo>();
        var infoLookUp = SystemAPI.GetComponentLookup<GameInfo>();
        var ecb = new EntityCommandBuffer(Allocator.Temp);
        foreach (var (characterData, tag, entity) in SystemAPI.Query<CharacterData, DeadTag>().WithNone<DisableTag>().WithEntityAccess())
        {
            var gameInfo = infoLookUp[_infoEntity];
            if (characterData.Type == CharacterType.EnemyMelee)
                gameInfo.MeleeDead++;
            else
                gameInfo.RangeDead++;
            ecb.SetComponent<GameInfo>(_infoEntity, gameInfo);
        }
        ecb.Playback(EntityManager);
        ecb.Dispose();
    }
}