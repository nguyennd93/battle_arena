using System.Diagnostics;
using GPUECSAnimationBaker.Engine.AnimatorSystem;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Gameplay
{
    [BurstCompile]
    public partial struct TestSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {

        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {

        }
    }
}