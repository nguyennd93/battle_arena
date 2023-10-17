using System.Collections;
using System.Collections.Generic;
using Unity.CharacterController;
using Unity.Mathematics;
using UnityEngine;

public struct DieState : ICharacterState
{
    public void OnStateEnter(StateType previousState, ref ThirdPersonCharacterUpdateContext context, ref KinematicCharacterUpdateContext baseContext, in ThirdPersonCharacterAspect aspect)
    {
    }

    public void OnStateExit(StateType nextState, ref ThirdPersonCharacterUpdateContext context, ref KinematicCharacterUpdateContext baseContext, in ThirdPersonCharacterAspect aspect)
    {
    }

    public void OnStatePhysicsUpdate(ref ThirdPersonCharacterUpdateContext context, ref KinematicCharacterUpdateContext baseContext, in ThirdPersonCharacterAspect aspect)
    {
    }

    public void GetMoveVectorFromPlayerInput(in ThirdPersonPlayerInputs inputs, quaternion cameraRotation, out float3 moveVector)
    {
        moveVector = float3.zero;
    }
}
