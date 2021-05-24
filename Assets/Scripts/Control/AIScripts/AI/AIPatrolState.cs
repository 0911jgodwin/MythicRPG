using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIPatrolState : AIState
{
    public AIAgent agent;
    Sequence rootNode;
    float timer;
    float waitTime = 2f;
    public float maxDistance = 1.5f;
    Vector3[] waypoints;
    int currentWaypointIndex;

    NodeStates behaviourTreeStatus = NodeStates.RUNNING;

    public void Enter(AIAgent agent)
    {
        this.agent = agent;
        waypoints = new Vector3[agent.patrolPath.childCount];
        for (int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = agent.patrolPath.GetChild(i).position;
        }
        GetNearestWaypoint();
        ResetTree();
    }

    public void Exit(AIAgent agent)
    {
    }

    public AIStateID GetID()
    {
        return AIStateID.PATROL;
    }

    public void Update(AIAgent agent)
    {
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


    public NodeStates WaitAtWaypoint()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = waitTime;
            return NodeStates.SUCCESS;
        }
        return NodeStates.RUNNING;
    }

    public void GetNearestWaypoint()
    {
        float sqrDistance = Mathf.Infinity;
        float currentClosestDistance = (waypoints[0] - agent.transform.position).sqrMagnitude;
        currentWaypointIndex = 0;
        foreach (Vector3 point in waypoints)
        {
            sqrDistance = (point - agent.transform.position).sqrMagnitude;
            if (sqrDistance < currentClosestDistance)
            {
                currentClosestDistance = sqrDistance;
                currentWaypointIndex = System.Array.IndexOf(waypoints, point);
            }
        }
    }

    public NodeStates GoToNextWaypoint()
    {
        float sqrDistance = (waypoints[currentWaypointIndex] - agent.transform.position).sqrMagnitude;
        if (sqrDistance > maxDistance * maxDistance)
        {
            agent.navMeshAgent.destination = waypoints[currentWaypointIndex];
            agent.navMeshAgent.isStopped = false;
            return NodeStates.RUNNING;
        }
        else
        {
            agent.navMeshAgent.isStopped = true;
            return NodeStates.SUCCESS;
        }
    }

    public NodeStates SetNextWaypoint()
    {
        currentWaypointIndex++;
        if (currentWaypointIndex > waypoints.Length - 1)
        {
            currentWaypointIndex = 0;
        }
        return NodeStates.SUCCESS;
    }

    private void ResetTree()
    {
        Leaf setNextWaypoint = new Leaf(SetNextWaypoint);
        Leaf goToNextWaypoint = new Leaf(GoToNextWaypoint);
        Leaf waitAtWaypoint = new Leaf(WaitAtWaypoint);

        rootNode = new Sequence();

        rootNode.AddChild(goToNextWaypoint);
        rootNode.AddChild(waitAtWaypoint);
        rootNode.AddChild(setNextWaypoint);
    }
}
