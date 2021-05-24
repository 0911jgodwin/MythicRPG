using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIAlertState : AIState
{
    public AIAgent agent;
    Sequence rootNode;
    public bool isRotating;
    public Quaternion rotation;
    float timer;
    float waitTime = 2f;
    float wanderRadius = 10f;
    Vector3 wanderPosition;
    public float maxDistance = 2.0f;
    public int wanderLoops = 4;
    public int loopCount;

    NodeStates behaviourTreeStatus = NodeStates.RUNNING;

    public void Enter(AIAgent agent)
    {
        loopCount = wanderLoops;
        this.agent = agent;
        ResetTree();
    }

    public void Exit(AIAgent agent)
    {
    }

    public AIStateID GetID()
    {
        return AIStateID.ALERT;
    }

    public void Update(AIAgent agent)
    {
        if (behaviourTreeStatus != NodeStates.SUCCESS)
        {
            behaviourTreeStatus = rootNode.Evaluate();
        }
        else
        {
            loopCount = loopCount - 1;
            if (loopCount <= 0)
            {
                agent.stateMachine.ChangeState(AIStateID.PATROL);
            }
            ResetTree();
            behaviourTreeStatus = NodeStates.RUNNING;
        }
    }

    public NodeStates LookLeft()
    {
        if (isRotating == false)
        {
            rotation = agent.transform.rotation * Quaternion.Inverse(Quaternion.Euler(0, 60, 0));
            isRotating = true;
        }
        else if (rotation == agent.transform.rotation) {
            isRotating = false;
            return NodeStates.SUCCESS;
        }
        agent.transform.rotation = Quaternion.RotateTowards(agent.transform.rotation, rotation, 30 * Time.deltaTime);
        return NodeStates.RUNNING;
    }

    public NodeStates LookRight()
    {
        if (isRotating == false)
        {
            rotation = agent.transform.rotation * Quaternion.Euler(0, 120, 0);
            isRotating = true;
        }
        else if (rotation == agent.transform.rotation)
        {
            isRotating = false;
            return NodeStates.SUCCESS;
        }
        agent.transform.rotation = Quaternion.RotateTowards(agent.transform.rotation, rotation, 30 * Time.deltaTime);
        return NodeStates.RUNNING;
    }

    public NodeStates WaitTimer()
    {
        timer -= Time.deltaTime;
        if (timer <=0)
        {
            timer = waitTime;
            return NodeStates.SUCCESS;
        }
        return NodeStates.RUNNING;
    }

    public NodeStates PlotNewPosition()
    {
        wanderPosition = GetWanderPosition(agent.transform.position, wanderRadius, -1);
        return NodeStates.SUCCESS;
    }

    public NodeStates Wander()
    {
        float sqrDistance = (wanderPosition - agent.transform.position).sqrMagnitude;
        if (sqrDistance > maxDistance * maxDistance)
        {
            agent.navMeshAgent.destination = wanderPosition;
            agent.navMeshAgent.isStopped = false;
            return NodeStates.RUNNING;
        }
        else
        {
            agent.navMeshAgent.isStopped = true;
            return NodeStates.SUCCESS;
        }
    }

    public static Vector3 GetWanderPosition(Vector3 origin, float dist, int layermask)
    {
        Vector3 randomDirection = Random.insideUnitSphere * dist;

        randomDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randomDirection, out navHit, dist, layermask);

        return navHit.position;
    }

    private void ResetTree()
    {
        timer = waitTime;
        Leaf lookLeftNode = new Leaf(LookLeft);
        Leaf waitNodeOne = new Leaf(WaitTimer);
        Leaf lookRightNode = new Leaf(LookRight);
        Leaf waitNodeTwo = new Leaf(WaitTimer);
        Leaf lookCenterNode = new Leaf(LookLeft);
        Sequence LookAroundNode = new Sequence();
        
        LookAroundNode.AddChild(lookLeftNode);
        LookAroundNode.AddChild(waitNodeOne);
        LookAroundNode.AddChild(lookRightNode);
        LookAroundNode.AddChild(waitNodeTwo);
        LookAroundNode.AddChild(lookCenterNode);

        Leaf plotWanderNode = new Leaf(PlotNewPosition);
        Leaf wanderNode = new Leaf(Wander);
        Sequence wanderingNode = new Sequence();

        wanderingNode.AddChild(plotWanderNode);
        wanderingNode.AddChild(wanderNode);

        rootNode = new Sequence();
        rootNode.AddChild(LookAroundNode);
        rootNode.AddChild(wanderingNode);
    }
}
