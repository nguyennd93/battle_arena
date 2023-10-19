using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.CharacterController;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Authoring;
using Unity.Physics.Extensions;
using Unity.Physics.Systems;
using Unity.Transforms;
using UnityEngine;
using GPUECSAnimationBaker.Engine.AnimatorSystem;

public struct ThirdPersonCharacterUpdateContext
{
    // Here, you may add additional global data for your character updates, such as ComponentLookups, Singletons, NativeCollections, etc...
    // The data you add here will be accessible in your character updates and all of your character "callbacks".
    public int ChunkIndex;
    public EntityCommandBuffer.ParallelWriter EndFrameFCB;
    public GameResource GameResource;
    public GameConfig GameConfig;

    public void SetChunkIndex(int chunkIndex)
    {
        ChunkIndex = chunkIndex;
    }

    public void OnSystemCreate(ref SystemState state)
    {
        // Get lookups
    }

    public void OnSystemUpdate(ref SystemState state, EntityCommandBuffer endFrameECB, GameResource gameResource, GameConfig gameConfig)
    {
        EndFrameFCB = endFrameECB.AsParallelWriter();
        GameResource = gameResource;
        GameConfig = gameConfig;
    }
}

public readonly partial struct ThirdPersonCharacterAspect : IAspect, IKinematicCharacterProcessor<ThirdPersonCharacterUpdateContext>
{
    public readonly KinematicCharacterAspect CharacterAspect;
    public readonly RefRW<LocalTransform> Transform;
    public readonly RefRW<ThirdPersonCharacterComponent> CharacterComponent;
    public readonly RefRW<ThirdPersonCharacterControl> CharacterControl;
    public readonly GpuEcsAnimatorAspect AnimationAspect;
    public readonly RefRW<CharacterStateMachine> StateMachine;
    public readonly RefRW<CharacterData> CharacterData;
    public readonly RefRW<CharacterState> StateData;
    public readonly Entity CurrentEntity;

    public void PhysicsUpdate(ref ThirdPersonCharacterUpdateContext context, ref KinematicCharacterUpdateContext baseContext)
    {
        ref KinematicCharacterBody characterBody = ref CharacterAspect.CharacterBody.ValueRW;
        ref float3 characterPosition = ref CharacterAspect.LocalTransform.ValueRW.Position;
        ref CharacterStateMachine stateMachine = ref StateMachine.ValueRW;

        if (stateMachine.CurrentState == StateType.Init)
        {
            characterBody.IsGrounded = true;
            stateMachine.TransitionToState(StateType.Move, ref context, ref baseContext, in this);
        }

        if (StateData.ValueRW.Dead && stateMachine.CurrentState != StateType.Die)
            stateMachine.TransitionToState(StateType.Die, ref context, ref baseContext, in this);
        else if (StateData.ValueRW.Attack)
            stateMachine.TransitionToState(StateType.Attack, ref context, ref baseContext, in this);

        stateMachine.OnStatePhysicsUpdate(stateMachine.CurrentState, ref context, ref baseContext, in this);
    }

    public void OnBeginHandlePhysicsUpdate(ref ThirdPersonCharacterUpdateContext context, ref KinematicCharacterUpdateContext baseContext)
    {
        ref KinematicCharacterBody characterBody = ref CharacterAspect.CharacterBody.ValueRW;
        ref float3 characterPosition = ref CharacterAspect.LocalTransform.ValueRW.Position;

        CharacterAspect.Update_Initialize(in this, ref context, ref baseContext, ref characterBody, baseContext.Time.DeltaTime);
        CharacterAspect.Update_ParentMovement(in this, ref context, ref baseContext, ref characterBody, ref characterPosition, characterBody.WasGroundedBeforeCharacterUpdate);
        CharacterAspect.Update_Grounding(in this, ref context, ref baseContext, ref characterBody, ref characterPosition);
    }

    public void OnEndHandlePhysicsUpdate(ref ThirdPersonCharacterUpdateContext context, ref KinematicCharacterUpdateContext baseContext, bool allowMovementAndDecollisions)
    {
        ref ThirdPersonCharacterComponent character = ref CharacterComponent.ValueRW;
        ref KinematicCharacterBody characterBody = ref CharacterAspect.CharacterBody.ValueRW;
        ref float3 characterPosition = ref CharacterAspect.LocalTransform.ValueRW.Position;

        CharacterAspect.Update_MovementAndDecollisions(in this, ref context, ref baseContext, ref characterBody, ref characterPosition);
        CharacterAspect.Update_MovingPlatformDetection(ref baseContext, ref characterBody);
        CharacterAspect.Update_ParentMomentum(ref baseContext, ref characterBody);
        CharacterAspect.Update_ProcessStatefulCharacterHits();
    }

    public void VariableUpdate(ref ThirdPersonCharacterUpdateContext context, ref KinematicCharacterUpdateContext baseContext)
    {
        ref KinematicCharacterBody characterBody = ref CharacterAspect.CharacterBody.ValueRW;
        ref ThirdPersonCharacterComponent characterComponent = ref CharacterComponent.ValueRW;
        ref ThirdPersonCharacterControl characterControl = ref CharacterControl.ValueRW;
        ref quaternion characterRotation = ref CharacterAspect.LocalTransform.ValueRW.Rotation;

        // Add rotation from parent body to the character rotation
        // (this is for allowing a rotating moving platform to rotate your character as well, and handle interpolation properly)
        KinematicCharacterUtilities.AddVariableRateRotationFromFixedRateRotation(ref characterRotation, characterBody.RotationFromParent, baseContext.Time.DeltaTime, characterBody.LastPhysicsUpdateDeltaTime);

        // Rotate towards move direction
        if (math.lengthsq(characterControl.MoveVector) > 0f)
        {
            CharacterControlUtilities.SlerpRotationTowardsDirectionAroundUp(ref characterRotation, baseContext.Time.DeltaTime, math.normalizesafe(characterControl.MoveVector), MathUtilities.GetUpFromRotation(characterRotation), characterComponent.RotationSharpness);
        }
    }

    #region Character Processor Callbacks
    public void UpdateGroundingUp(
        ref ThirdPersonCharacterUpdateContext context,
        ref KinematicCharacterUpdateContext baseContext)
    {
        ref KinematicCharacterBody characterBody = ref CharacterAspect.CharacterBody.ValueRW;

        CharacterAspect.Default_UpdateGroundingUp(ref characterBody);
    }

    public bool CanCollideWithHit(
        ref ThirdPersonCharacterUpdateContext context,
        ref KinematicCharacterUpdateContext baseContext,
        in BasicHit hit)
    {
        ThirdPersonCharacterComponent characterComponent = CharacterComponent.ValueRO;

        if (PhysicsUtilities.HasPhysicsTag(in baseContext.PhysicsWorld, hit.RigidBodyIndex, characterComponent.IgnoreCollisionsTag))
        {
            return false;
        }

        return PhysicsUtilities.IsCollidable(hit.Material);
    }

    public bool IsGroundedOnHit(
        ref ThirdPersonCharacterUpdateContext context,
        ref KinematicCharacterUpdateContext baseContext,
        in BasicHit hit,
        int groundingEvaluationType)
    {
        ThirdPersonCharacterComponent characterComponent = CharacterComponent.ValueRO;

        return CharacterAspect.Default_IsGroundedOnHit(
            in this,
            ref context,
            ref baseContext,
            in hit,
            in characterComponent.StepAndSlopeHandling,
            groundingEvaluationType);
    }

    public void OnMovementHit(
            ref ThirdPersonCharacterUpdateContext context,
            ref KinematicCharacterUpdateContext baseContext,
            ref KinematicCharacterHit hit,
            ref float3 remainingMovementDirection,
            ref float remainingMovementLength,
            float3 originalVelocityDirection,
            float hitDistance)
    {
        ref KinematicCharacterBody characterBody = ref CharacterAspect.CharacterBody.ValueRW;
        ref float3 characterPosition = ref CharacterAspect.LocalTransform.ValueRW.Position;
        ThirdPersonCharacterComponent characterComponent = CharacterComponent.ValueRO;

        CharacterAspect.Default_OnMovementHit(
            in this,
            ref context,
            ref baseContext,
            ref characterBody,
            ref characterPosition,
            ref hit,
            ref remainingMovementDirection,
            ref remainingMovementLength,
            originalVelocityDirection,
            hitDistance,
            characterComponent.StepAndSlopeHandling.StepHandling,
            characterComponent.StepAndSlopeHandling.MaxStepHeight,
            characterComponent.StepAndSlopeHandling.CharacterWidthForStepGroundingCheck);
    }

    public void OverrideDynamicHitMasses(
        ref ThirdPersonCharacterUpdateContext context,
        ref KinematicCharacterUpdateContext baseContext,
        ref PhysicsMass characterMass,
        ref PhysicsMass otherMass,
        BasicHit hit)
    {
        // Custom mass overrides
    }

    public void ProjectVelocityOnHits(
        ref ThirdPersonCharacterUpdateContext context,
        ref KinematicCharacterUpdateContext baseContext,
        ref float3 velocity,
        ref bool characterIsGrounded,
        ref BasicHit characterGroundHit,
        in DynamicBuffer<KinematicVelocityProjectionHit> velocityProjectionHits,
        float3 originalVelocityDirection)
    {
        ThirdPersonCharacterComponent characterComponent = CharacterComponent.ValueRO;

        CharacterAspect.Default_ProjectVelocityOnHits(
            ref velocity,
            ref characterIsGrounded,
            ref characterGroundHit,
            in velocityProjectionHits,
            originalVelocityDirection,
            characterComponent.StepAndSlopeHandling.ConstrainVelocityToGroundPlane);
    }

    public static void GetCommonMoveVectorFromPlayerInput(in ThirdPersonPlayerInputs inputs, quaternion cameraRotation, out float3 moveVector)
    {
        moveVector = (math.mul(cameraRotation, math.right()) * inputs.MoveInput.x) + (math.mul(cameraRotation, math.forward()) * inputs.MoveInput.y);
    }
    #endregion
}
