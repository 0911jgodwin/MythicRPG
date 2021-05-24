using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AICombatState : AIState
{
    public float maxTime = 1.0f;
    public float maxDistance = 1.5f;
    float timer = 0.0f;
    float timeSinceLastAttack = Mathf.Infinity;
    float timeBetweenAttacks = 1f;
    int weaponDamage = 5;
    public PlayerHealth target;
    public AIAgent agent;
    Sequence rootNode;

    NodeStates behaviourTreeStatus = NodeStates.RUNNING;

    public void Enter(AIAgent agent)
    {
        this.agent = agent;
        ResetTree();
    }

    public void Exit(AIAgent agent)
    {
    }

    public AIStateID GetID()
    {
        return AIStateID.COMBAT;
    }

    public void Update(AIAgent agent)
    {
        timeSinceLastAttack += Time.deltaTime;
        if (behaviourTreeStatus != NodeStates.SUCCESS)
        {
            behaviourTreeStatus = rootNode.Evaluate();
        }
        else
        {
            ResetTree();
            behaviourTreeStatus = NodeStates.RUNNING;
        }
    }

    private NodeStates GetInRange()
    {
        Vector3 target = agent.fov.currentTargetPosition;
        //agent.transform.LookAt(target);
        Vector3 relativePos = target - agent.transform.position;
        Quaternion toRotation = Quaternion.LookRotation(relativePos);
        agent.transform.rotation = Quaternion.Lerp(agent.transform.rotation, toRotation, 5 * Time.deltaTime);
        timer -= Time.deltaTime;
        if(timer < 0.0f)
        {
            float sqrDistance = (target - agent.transform.position).sqrMagnitude;
            if (sqrDistance > maxDistance * maxDistance)
            {
                agent.navMeshAgent.destination = target;
                agent.navMeshAgent.isStopped = false;
            }
            else
            {
                agent.navMeshAgent.isStopped = true;
                return NodeStates.SUCCESS;
            }
            timer = maxTime;
        }
        return NodeStates.RUNNING;
    }

    private NodeStates AttackBehaviour()
    {
        if (timeSinceLastAttack > timeBetweenAttacks)
        {
            // This will trigger the Hit() event.
            TriggerAttack();
            timeSinceLastAttack = 0;
            return NodeStates.SUCCESS;
        }
        return NodeStates.RUNNING;
    }

    private void TriggerAttack()
    {
        agent.animator.ResetTrigger("attack");
        agent.animator.SetTrigger("attack");
    }

    private void ResetTree()
    {
        Leaf getInRangeNode = new Leaf(GetInRange);
        Leaf attackTarget = new Leaf(AttackBehaviour);
        rootNode = new Sequence();
        rootNode.AddChild(getInRangeNode);
        rootNode.AddChild(attackTarget);
    }
}
