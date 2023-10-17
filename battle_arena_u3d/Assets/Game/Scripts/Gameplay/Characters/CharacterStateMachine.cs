using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.CharacterController;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct CharacterStateMachine : IComponentData
{
    public StateType CurrentState;
    public StateType PreviousState;

    // Character State Data
    MoveState StateMove;
    AttackState StateAttack;
    DieState StateDie;

    public void TransitionToState(StateType newState, ref ThirdPersonCharacterUpdateContext context, ref KinematicCharacterUpdateContext baseContext, in ThirdPersonCharacterAspect aspect)
    {
        PreviousState = CurrentState;
        CurrentState = newState;

        OnStateExit(PreviousState, CurrentState, ref context, ref baseContext, in aspect);
        OnStateEnter(CurrentState, PreviousState, ref context, ref baseContext, in aspect);
    }

    private void OnStateEnter(StateType state, StateType prevState, ref ThirdPersonCharacterUpdateContext context, ref KinematicCharacterUpdateContext baseContext, in ThirdPersonCharacterAspect aspect)
    {
        switch (state)
        {
            case StateType.Move:
                StateMove.OnStateEnter(prevState, ref context, ref baseContext, in aspect);
                break;

            case StateType.Attack:
                StateAttack.OnStateEnter(prevState, ref context, ref baseContext, in aspect);
                break;

            case StateType.Die:
                StateDie.OnStateEnter(prevState, ref context, ref baseContext, in aspect);
                break;
        }
    }

    private void OnStateExit(StateType state, StateType newState, ref ThirdPersonCharacterUpdateContext context, ref KinematicCharacterUpdateContext baseContext, in ThirdPersonCharacterAspect aspect)
    {
        switch (state)
        {
            case StateType.Move:
                StateMove.OnStateExit(newState, ref context, ref baseContext, in aspect);
                break;

            case StateType.Attack:
                StateAttack.OnStateExit(newState, ref context, ref baseContext, in aspect);
                break;

            case StateType.Die:
                StateDie.OnStateExit(newState, ref context, ref baseContext, in aspect);
                break;
        }
    }

    public void OnStatePhysicsUpdate(StateType state, ref ThirdPersonCharacterUpdateContext context, ref KinematicCharacterUpdateContext baseContext, in ThirdPersonCharacterAspect aspect)
    {
        switch (state)
        {
            case StateType.Move:
                StateMove.OnStatePhysicsUpdate(ref context, ref baseContext, in aspect);
                break;

            case StateType.Attack:
                StateAttack.OnStatePhysicsUpdate(ref context, ref baseContext, in aspect);
                break;

            case StateType.Die:
                StateDie.OnStatePhysicsUpdate(ref context, ref baseContext, in aspect);
                break;
        }
    }

    public void GetMoveVectorFromPlayerInput(StateType state, in ThirdPersonPlayerInputs inputs, quaternion cameraRotation, out float3 moveVector)
    {
        moveVector = default;

        switch (state)
        {
            case StateType.Move:
                StateMove.GetMoveVectorFromPlayerInput(in inputs, cameraRotation, out moveVector);
                break;
            case StateType.Attack:
                StateAttack.GetMoveVectorFromPlayerInput(in inputs, cameraRotation, out moveVector);
                break;
            case StateType.Die:
                StateDie.GetMoveVectorFromPlayerInput(in inputs, cameraRotation, out moveVector);
                break;
        }
    }
}

