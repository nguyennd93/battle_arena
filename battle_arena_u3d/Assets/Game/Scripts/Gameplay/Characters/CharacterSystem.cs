using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Gameplay
{
    [AlwaysSynchronizeSystem]
    [UpdateInGroup(typeof(SimulationSystemGroup), OrderLast = true)]
    [UpdateAfter(typeof(EndSimulationEntityCommandBufferSystem))]
    public partial class CharacterSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            
        }
    }
}