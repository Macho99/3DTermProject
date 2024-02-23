using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.AI;

public class BattleIdleState : StateMachineBehaviour
{
    public float rotationValue;
    float timer;
    float attackDelay;
    Transform target;
    Transform myTf;
    NavMeshAgent agent;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        attackDelay = animator.GetComponent<Monster>().attackDelay;
        rotationValue = animator.GetComponent<Monster>().rotationSpeed;
        timer = 0f;
        target = animator.GetComponent<Monster>().target;
        myTf = animator.GetComponent<Transform>();
        agent = animator.GetComponent<NavMeshAgent>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer += Time.deltaTime;

        if (target == null)
        {
            animator.SetBool("isAttacking", false);
            return;
        }

        Turn(target, myTf);

        if (timer > attackDelay)
        {
            if (Vector3.Distance(animator.transform.position, target.position) > agent.stoppingDistance)
            {
                animator.SetBool("isAttacking", false);
            }
            animator.SetBool("AttackDelay", false);
        }

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    private void Turn(Transform target, Transform myTf)
    {
        Vector3 directionToTarget = target.position - myTf.position;
        directionToTarget.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget.normalized);
        myTf.rotation = Quaternion.Slerp(myTf.rotation, targetRotation, rotationValue * Time.deltaTime);
    }
}