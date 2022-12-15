using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attk1Script : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Debug.Log(MJB_PlayerMove.instance.myAnim.GetCurrentAnimatorStateInfo(0).length);
        
        if(MJB_PlayerMove.instance.isAttacking == true) {
            MJB_PlayerMove.instance.comboHit = true;
            Debug.Log("boom");
        }

        MJB_PlayerMove.instance.attkAnimPlaying = true;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //MJB_PlayerMove.instance.isAttacking = false;
        if(MJB_PlayerMove.instance.comboHit == true) {
            MJB_PlayerMove.instance.myAnim.Play("PlayerAttk2");
            MJB_PlayerMove.instance.comboHit = false;
        } else if(MJB_PlayerMove.instance.comboHit == false) {
            MJB_PlayerMove.instance.comboDone = true;
            MJB_PlayerMove.instance.attkAnimPlaying = false;
        }
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
