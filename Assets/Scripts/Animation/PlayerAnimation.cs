using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnimationType
{
    Idle, Jump, WipeOut, ChangingSpeed, GrabbingBoard, GrabbingVine, Fall, JumpEnd, GrabbingBoardEnd
}

public class PlayerAnimation : MonoBehaviour
{
    Animator anim;
    Dictionary<AnimationType, int> stateToAnimHash = new Dictionary<AnimationType, int>()
    {
        { AnimationType.Idle, Animator.StringToHash("Player_Idle") },
        { AnimationType.WipeOut, Animator.StringToHash("Player_Wipeout") },
        { AnimationType.Jump, Animator.StringToHash("Player_Jump") },
        { AnimationType.JumpEnd, Animator.StringToHash("Player_JumpEnd") },
        { AnimationType.ChangingSpeed, Animator.StringToHash("Player_IncreaseSpeed") },
        { AnimationType.GrabbingBoard, Animator.StringToHash("Player_GrabBoard") },
        { AnimationType.GrabbingVine, Animator.StringToHash("Player_GrabVine") },
        { AnimationType.Fall, Animator.StringToHash("Player_Fall") },
        { AnimationType.GrabbingBoardEnd, Animator.StringToHash("Player_GrabBoardEnd") },
        
    };
    PlayerMovement movement;
    StateType lastState;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        movement = GetComponent<PlayerMovement>();
        movement.OnStateChange += CheckMovementState;
    }

    private void CheckMovementState(StateType newState)
    {
        if (newState == StateType.Dead)
        {
            PlayAnimation(AnimationType.WipeOut);
        }
        if (newState == StateType.Grounded)
        {
            if (lastState == StateType.InAir)
            {
                PlayAnimation(AnimationType.JumpEnd);
            }
            else
            {
                PlayAnimation(AnimationType.Idle);
            }
        }
        if (newState == StateType.Jump)
        {
            PlayAnimation(AnimationType.Jump);
        }
        if (newState == StateType.InAir)
        {
            if (lastState == StateType.InTrick)
            {
                PlayAnimation(AnimationType.GrabbingBoardEnd);
            }
            else
            {
                PlayAnimation(AnimationType.Fall);
            }
        }
        if (newState == StateType.InTrick)
        {
            PlayAnimation(AnimationType.GrabbingBoard);
        }
        lastState = newState;
    }

    void Update()
    {
        if (lastState == StateType.Grounded)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                PlayAnimation(AnimationType.ChangingSpeed);
            }
            if (Input.GetKeyUp(KeyCode.D))
            {
                PlayAnimation(AnimationType.Idle);
            }
        }
    }

    void PlayAnimation(AnimationType animation)
    {
        anim.Play(stateToAnimHash[animation]);
    }
}
