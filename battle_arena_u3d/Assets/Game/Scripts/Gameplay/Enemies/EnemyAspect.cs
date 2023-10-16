using System;
using GPUECSAnimationBaker.Engine.AnimatorSystem;
using Unity.CharacterController;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Authoring;
using Unity.Physics.Extensions;
using Unity.Transforms;
using UnityEngine;

public readonly partial struct EnemyAspect : IAspect
{
    public readonly RefRW<LocalToWorld> Position;
    public readonly RefRW<PhysicsVelocity> Velocity;
    public readonly RefRW<PhysicsMass> Mass;
    public readonly RefRW<EnemyData> EnemyData;
    public readonly RefRW<GpuEcsAnimatorControlComponent> Animation;

    public void ForceMove()
    {
        var position = Position.ValueRO.Position;
        var direct = math.normalize(float3.zero - position);
        // Velocity.ValueRW.ApplyLinearImpulse(Mass.ValueRO, direct * EnemyData.ValueRO.Speed);// direct * EnemyData.ValueRO.Speed;
    }
}