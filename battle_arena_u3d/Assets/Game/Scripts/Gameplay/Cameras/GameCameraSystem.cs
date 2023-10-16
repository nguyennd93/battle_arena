using UnityEngine;
using Unity.Entities;
using Unity.Transforms;

namespace Gameplay
{
    // [UpdateAfter(typeof(PlayerControllerSystem))]
    // public partial class GameCameraSystem : SystemBase
    // {
    //     protected override void OnUpdate()
    //     {
    //         if (SystemAPI.HasSingleton<CinematicCameraControl>())
    //         {
    //             var component = SystemAPI.GetComponent<CinematicCameraControl>(SystemAPI.GetSingletonEntity<CinematicCameraControl>());
    //             //var localToWorld = SystemAPI.GetComponent<LocalToWorld>(component.FollowedEntity);
    //             //CinemachineGameCamera.Instance.Target.SetPositionAndRotation(localToWorld.Position, localToWorld.Rotation);
    //         }
    //     }
    // }
}