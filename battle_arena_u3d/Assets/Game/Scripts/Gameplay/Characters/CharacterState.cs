using System.Collections;
using System.Collections.Generic;
using Unity.CharacterController;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public enum StateType
{
    Init = 0,
    Move = 1,
    Attack = 2,
    Die = 3
}

public struct CharacterState : IComponentData
{
    public float AttackRate;
    public float IntervalAttack;

    public bool Attack;
    public bool Dead;
}

public interface ICharacterState
{
    void OnStateEnter(StateType previousState, ref ThirdPersonCharacterUpdateContext context, ref KinematicCharacterUpdateContext baseContext, in ThirdPersonCharacterAspect aspect);
    void OnStateExit(StateType nextState, ref ThirdPersonCharacterUpdateContext context, ref KinematicCharacterUpdateContext baseContext, in ThirdPersonCharacterAspect aspect);
    void OnStatePhysicsUpdate(ref ThirdPersonCharacterUpdateContext context, ref KinematicCharacterUpdateContext baseContext, in ThirdPersonCharacterAspect aspect);
    void GetMoveVectorFromPlayerInput(in ThirdPersonPlayerInputs inputs, quaternion cameraRotation, out float3 moveVector);
}
