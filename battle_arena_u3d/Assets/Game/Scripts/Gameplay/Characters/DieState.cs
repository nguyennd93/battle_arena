using System.Collections;
using System.Collections.Generic;
using Unity.CharacterController;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public struct DieState : ICharacterState
{
    public void OnStateEnter(StateType previousState, ref ThirdPersonCharacterUpdateContext context, ref KinematicCharacterUpdateContext baseContext, in ThirdPersonCharacterAspect aspect)
    {
        aspect.AnimationAspect.RunAnimation(3, 1f, 1f, 0, 0.3f);
    }

    public void OnStateExit(StateType nextState, ref ThirdPersonCharacterUpdateContext context, ref KinematicCharacterUpdateContext baseContext, in ThirdPersonCharacterAspect aspect)
    {
    }

    public void OnStatePhysicsUpdate(ref ThirdPersonCharacterUpdateContext context, ref KinematicCharacterUpdateContext baseContext, in ThirdPersonCharacterAspect aspect)
    {
        if (aspect.AnimationAspect.IsStop())
        {
            aspect.Transform.ValueRW.Position = new float3(0f, -500f, 0f);
            aspect.CharacterAspect.DeferredImpulsesBuffer.Clear();
            context.EndFrameFCB.AddComponent<DeadTag>(context.ChunkIndex, aspect.CurrentEntity);
        }

        DetectTransitions(ref context, ref baseContext, in aspect);
    }

    public void GetMoveVectorFromPlayerInput(in ThirdPersonPlayerInputs inputs, quaternion cameraRotation, out float3 moveVector)
    {
        moveVector = float3.zero;
    }

    public bool DetectTransitions(ref ThirdPersonCharacterUpdateContext context, ref KinematicCharacterUpdateContext baseContext, in ThirdPersonCharacterAspect aspect)
    {
        ref KinematicCharacterBody characterBody = ref aspect.CharacterAspect.CharacterBody.ValueRW;
        ref ThirdPersonCharacterControl characterControl = ref aspect.CharacterControl.ValueRW;
        ref CharacterStateMachine stateMachine = ref aspect.StateMachine.ValueRW;

        return false;
    }
}
