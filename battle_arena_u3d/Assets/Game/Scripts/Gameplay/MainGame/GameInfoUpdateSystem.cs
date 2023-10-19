using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial class GameInfoUpdateSystem : SystemBase
{
    protected override void OnUpdate()
    {
        foreach (var gameInfo in SystemAPI.Query<GameInfo>())
        {
            if (GameFlow.Instance != null)
            {
                GameFlow.Instance.OnGameInfoChanged(gameInfo);
            }
        }
    }
}
