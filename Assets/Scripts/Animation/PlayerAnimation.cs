using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnimationType
{
    Idle, Jump, WipeOut, ChangingSpeed, GrabbingBoard, GrabbingVine, Fall
}

public class PlayerAnimation : MonoBehaviour
{
    Animator anim;
    Dictionary<AnimationType, int> stateToAnimHash = new Dictionary<AnimationType, int>()
    {
        { AnimationType.Idle, Animator.StringToHash("Player_Idle") },
        { AnimationType.WipeOut, Animator.StringToHash("Player_Wipeout") },
        { AnimationType.Jump, Animator.StringToHash("Player_Jump") },
        { AnimationType.ChangingSpeed, Animator.StringToHash("Player_IncreaseSpeed") },
        { AnimationType.GrabbingBoard, Animator.StringToHash("Player_GrabBoard") },
        { AnimationType.GrabbingVine, Animator.StringToHash("Player_GrabVine") },
        { AnimationType.Fall, Animator.StringToHash("Player_Fall") },
        
    };
    PlayerMovement movement;

    private void Start()
    {
        anim = GetComponent<Animator>();
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
            if (Input.GetKey(KeyCode.D))
            {
                PlayAnimation(AnimationType.ChangingSpeed);
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
            PlayAnimation(AnimationType.Fall);
        }
    }

    void PlayAnimation(AnimationType animation)
    {
        anim.Play(stateToAnimHash[animation]);
    }
}
