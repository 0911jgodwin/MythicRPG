using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIAgent : MonoBehaviour
{
    public AIStateMachine stateMachine;
    public AIStateID initialState;
    public Transform patrolPath;
    public NavMeshAgent navMeshAgent;
    public FieldOfView fov;
    public Animator animator;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        fov = GetComponent<FieldOfView>();
        animator = GetComponent<Animator>();
        stateMachine = new AIStateMachine(this);
        stateMachine.RegisterState(new AICombatState());
        stateMachine.RegisterState(new AIAlertState());
        stateMachine.RegisterState(new AIPatrolState());
        stateMachine.RegisterState(new AIDeathState());
        stateMachine.ChangeState(initialState);
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Update();
    }

    private void OnDrawGizmos()
    {
        Vector3 startPosition = patrolPath.GetChild(0).position;
        Vector3 previousPosition = startPosition;
        foreach(Transform waypoint in patrolPath)
        {
            Gizmos.DrawSphere(waypoint.position, 0.4f);
            Gizmos.DrawLine(previousPosition, waypoint.position);
            previousPosition = waypoint.position;
        }
        Gizmos.DrawLine(previousPosition, startPosition);
    }

    public AIStateID GetCurrentState()
    {
        return stateMachine.currentState;
    }
}
