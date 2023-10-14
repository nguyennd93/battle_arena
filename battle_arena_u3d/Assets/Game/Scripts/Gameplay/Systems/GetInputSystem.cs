using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Gameplay
{
    [UpdateInGroup(typeof(InitializationSystemGroup), OrderLast = true)]
    public partial class GetInputSystem : SystemBase
    {
        private ControlActions _controlActions;
        // private Entity _playerEntity;

        protected override void OnCreate()
        {
            RequireForUpdate<MoveInput>();
            RequireForUpdate<PlayerTag>();

            _controlActions = new ControlActions();
        }

        protected override void OnStartRunning()
        {
            _controlActions.Enable();

            // _playerEntity = SystemAPI.GetSingletonEntity<PlayerTag>();
        }

        protected override void OnStopRunning()
        {
            _controlActions.Disable();
            // _playerEntity = Entity.Null;
        }

        protected override void OnUpdate()
        {
            var inputMove = _controlActions.Controller.Movement.ReadValue<Vector2>();
            SystemAPI.SetSingleton(new MoveInput() { Value = inputMove });
        }
    }
}