using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Gameplay
{
    [UpdateInGroup(typeof(InitializationSystemGroup), OrderLast = true)]
    public partial class PlayerInputsSystem : SystemBase
    {
        private ControlActions _controlActions;

        protected override void OnCreate()
        {
            RequireForUpdate(SystemAPI.QueryBuilder().WithAll<MainPlayer, PlayerInputs>().Build());
            _controlActions = new ControlActions();
        }

        protected override void OnStartRunning()
        {
            _controlActions.Enable();
        }

        protected override void OnStopRunning()
        {
            _controlActions.Disable();
        }

        protected override void OnUpdate()
        {
            foreach (var (playerInputs, player) in SystemAPI.Query<RefRW<PlayerInputs>, MainPlayer>())
            {
                playerInputs.ValueRW.Move = Vector2.ClampMagnitude(_controlActions.Controller.Movement.ReadValue<Vector2>(), 1f);
            }
        }
    }
}