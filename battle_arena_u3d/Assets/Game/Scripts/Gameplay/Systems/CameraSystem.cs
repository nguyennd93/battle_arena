using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Gameplay
{
    public partial class CameraSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var playerEntity = SystemAPI.GetSingletonEntity<PlayerTag>();
            var position = SystemAPI.GetComponent<LocalToWorld>(playerEntity).Value.c3;

            GCamera.Instance.TargetCamara.transform.position = new Vector3(position.x, position.y, position.z); ;
        }
    }
}