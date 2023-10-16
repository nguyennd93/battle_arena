using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using UnityEngine;

namespace Gameplay
{
    [UpdateInGroup(typeof(SimulationSystemGroup), OrderLast = true)]
    [BurstCompile]
    public partial struct PlayerControllerSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate(SystemAPI.QueryBuilder().WithAll<MainPlayer, PlayerInputs>().Build());
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (playerInputs, player) in SystemAPI.Query<PlayerInputs, MainPlayer>().WithAll<Simulate>())
            {
                if (SystemAPI.HasComponent<CinematicCameraControl>(player.Camera))
                {
                    CinematicCameraControl cameraControl = SystemAPI.GetComponent<CinematicCameraControl>(player.Camera);
                    cameraControl.FollowedEntity = player.Character;
                    SystemAPI.SetComponent(player.Camera, cameraControl);
                }
            }
        }
    }
}