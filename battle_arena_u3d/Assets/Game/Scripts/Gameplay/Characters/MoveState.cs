using System.Collections;
using System.Collections.Generic;
using GPUECSAnimationBaker.Engine.AnimatorSystem;
using Unity.CharacterController;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;

public struct MoveState : ICharacterState
{
    public void OnStateEnter(StateType previousState, ref ThirdPersonCharacterUpdateContext context, ref KinematicCharacterUpdateContext baseContext, in ThirdPersonCharacterAspect aspect)
    {
        aspect.AnimationAspect.RunAnimation(1, 1f, 1f, 0, 0.3f);
    }

    public void OnStateExit(StateType nextState, ref ThirdPersonCharacterUpdateContext context, ref KinematicCharacterUpdateContext baseContext, in ThirdPersonCharacterAspect aspect)
    {
    }

    public void OnStatePhysicsUpdate(ref ThirdPersonCharacterUpdateContext context, ref KinematicCharacterUpdateContext baseContext, in ThirdPersonCharacterAspect aspect)
    {
        float deltaTime = baseContext.Time.DeltaTime;
        float elapsedTime = (float)baseContext.Time.ElapsedTime;
        ref KinematicCharacterBody characterBody = ref aspect.CharacterAspect.CharacterBody.ValueRW;
        ref ThirdPersonCharacterComponent character = ref aspect.CharacterComponent.ValueRW;
        ref ThirdPersonCharacterControl characterControl = ref aspect.CharacterControl.ValueRW;

        aspect.OnBeginHandlePhysicsUpdate(ref context, ref baseContext);

        // Rotate move input and velocity to take into account parent rotation
        if (characterBody.ParentEntity != Entity.Null)
        {
            characterControl.MoveVector = math.rotate(characterBody.RotationFromParent, characterControl.MoveVector);
            characterBody.RelativeVelocity = math.rotate(characterBody.RotationFromParent, characterBody.RelativeVelocity);
        }

        if (characterBody.IsGrounded)
        {
            // Move on ground
            float3 targetVelocity = characterControl.MoveVector * character.GroundMaxSpeed;
            CharacterControlUtilities.StandardGroundMove_Interpolated(ref characterBody.RelativeVelocity, targetVelocity, character.GroundedMovementSharpness, deltaTime, characterBody.GroundingUp, characterBody.GroundHit.Normal);
            
            aspect.AnimationAspect.RunAnimation(math.length(characterControl.MoveVector) > 0 ? 1 : 0);
            if (characterControl.Attack)
            {
                ref CharacterStateMachine stateMachine = ref aspect.StateMachine.ValueRW;
                stateMachine.TransitionToState(StateType.Attack, ref context, ref baseContext, in aspect);
            }
        }
        else
        {
            float3 airAcceleration = characterControl.MoveVector * character.AirAcceleration;
            if (math.lengthsq(airAcceleration) > 0f)
            {
                float3 tmpVelocity = characterBody.RelativeVelocity;
                CharacterControlUtilities.StandardAirMove(ref characterBody.RelativeVelocity, airAcceleration, character.AirMaxSpeed, characterBody.GroundingUp, deltaTime, false);

                // Cancel air acceleration from input if we would hit a non-grounded surface (prevents air-climbing slopes at high air accelerations)
                if (aspect.CharacterAspect.MovementWouldHitNonGroundedObstruction(in aspect, ref context, ref baseContext, characterBody.RelativeVelocity * deltaTime, out ColliderCastHit hit))
                {
                    characterBody.RelativeVelocity = tmpVelocity;
                }
            }

            CharacterControlUtilities.AccelerateVelocity(ref characterBody.RelativeVelocity, character.Gravity, deltaTime);
            CharacterControlUtilities.ApplyDragToVelocity(ref characterBody.RelativeVelocity, deltaTime, character.AirDrag);
        }

        aspect.OnEndHandlePhysicsUpdate(ref context, ref baseContext, true);

        DetectTransitions(ref context, ref baseContext, in aspect);
    }

    public void GetMoveVectorFromPlayerInput(in ThirdPersonPlayerInputs inputs, quaternion cameraRotation, out float3 moveVector)
    {
        ThirdPersonCharacterAspect.GetCommonMoveVectorFromPlayerInput(in inputs, cameraRotation, out moveVector);
    }

    public bool DetectTransitions(ref ThirdPersonCharacterUpdateContext context, ref KinematicCharacterUpdateContext baseContext, in ThirdPersonCharacterAspect aspect)
    {
        ref KinematicCharacterBody characterBody = ref aspect.CharacterAspect.CharacterBody.ValueRW;
        ref ThirdPersonCharacterControl characterControl = ref aspect.CharacterControl.ValueRW;
        ref CharacterStateMachine stateMachine = ref aspect.StateMachine.ValueRW;

        return false;
    }
}
