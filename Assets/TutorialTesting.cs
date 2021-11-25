using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTesting : StateMachineBehaviour
{
    public GameObject projectile;
    private GameObject localProjectile;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector3 playerDirection = GameObject.FindGameObjectWithTag("MainCamera").transform.position - animator.transform.position;

        localProjectile = Instantiate(projectile, animator.transform);
        localProjectile.GetComponent<ProjectileScript>().Origin = animator.gameObject;

        animator.SetInteger("SpecialMeter", animator.GetInteger("SpecialMeter") + 1);
        animator.SetBool("CoolingDown", true);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

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
