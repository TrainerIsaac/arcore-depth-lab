using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RapidFire : StateMachineBehaviour
{
    private int projectileCounter;
    public GameObject projectile;
    private GameObject localProjectile;
    private Vector3 leftToRight;
    static float t = 0.0f;
    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        projectileCounter = 0;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(projectileCounter < 30)
        {
            leftToRight = new Vector3(Mathf.Lerp(-5, 15, t), 0, 0);
            t += 0.5f * (Time.deltaTime / 2);

            Vector3 playerDirection = GameObject.FindGameObjectWithTag("MainCamera").transform.position - animator.transform.position;

            localProjectile = Instantiate(projectile, animator.transform.position, Quaternion.LookRotation(playerDirection +(leftToRight)));
            localProjectile.GetComponent<ProjectileScript>().Origin = animator.gameObject;
            projectileCounter += 1;
        }

        else
        {
            animator.SetInteger("SpecialMeter", 0);
            animator.SetBool("CoolingDown", false);
        }
    }

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        projectileCounter = 0;
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
