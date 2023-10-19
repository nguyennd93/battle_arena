using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(LateSimulationSystemGroup))]
public partial class GameInfoUpdateSystem : SystemBase
{
    bool _forceUpdate = false;
    bool _forceReset = false;

    protected override void OnUpdate()
    {
        foreach (var (gameInfo, config, forceRest) in SystemAPI.Query<GameInfo, RefRW<GameConfig>, RefRW<ForceResetGame>>())
        {
            if (GameFlow.Instance != null)
            {
                GameFlow.Instance.OnGameInfoChanged(gameInfo);
            }

            if (!_forceUpdate && GameFlow.Instance != null)
            {
                _forceUpdate = true;
                config.ValueRW.EnemyPerTurn = GameFlow.Instance.TurnAmount;
                config.ValueRW.IntervalSpawn = GameFlow.Instance.Interval;
            }

            if (_forceReset)
            {
                _forceReset = false;
                forceRest.ValueRW.ForceReset = true;
            }
        }
    }

    public void ForceUpdate()
    {
        _forceUpdate = true;
    }

    public void ForceReset()
    {

    }
}