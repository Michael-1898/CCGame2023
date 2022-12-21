﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attk2Script : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        MJB_PlayerMove.instance.canRotate = true;

        MJB_PlayerMove.instance.isAttacking = false;
        MJB_PlayerMove.instance.attkStart = true; 
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(MJB_PlayerMove.instance.attkStart == true) {
            MJB_PlayerMove.instance.attkStart = false;
        }

        if(MJB_PlayerMove.instance.isAttacking == true) {
            MJB_PlayerMove.instance.comboHit = true;
        }

        MJB_PlayerMove.instance.attkAnimPlaying = true;
        MJB_PlayerMove.instance.canRotate = false;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        MJB_PlayerMove.instance.isAttacking = false;
        if(MJB_PlayerMove.instance.comboHit == true) {
            MJB_PlayerMove.instance.myAnim.Play("PlayerAttk3");
            MJB_PlayerMove.instance.comboHit = false;
            MJB_PlayerMove.instance.attkNum = 3;
            MJB_PlayerMove.instance.isAttacking = true;
        } else if(MJB_PlayerMove.instance.comboHit == false) {
            MJB_PlayerMove.instance.comboDone = true;
            MJB_PlayerMove.instance.attkAnimPlaying = false;
        }
        MJB_PlayerMove.instance.canRotate = true;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
