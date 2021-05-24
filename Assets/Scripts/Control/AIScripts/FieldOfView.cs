using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float viewRadius;
    public float innerViewRadius;
    [Range(0, 360)]
    public float viewAngle;
    public float timeBeforeLeavingCombat = 5f;
    public float timer;

    public AIAgent agent;

    public LayerMask targetMask;
    public LayerMask obstacleMask;
    public Vector3 currentTargetPosition;

    public List<Transform> visibleTargets = new List<Transform>();

    public bool HasTarget
    {
        get { return visibleTargets.Count > 0; }
    }
    public Vector3 CurrentTarget
    {
        get { return currentTargetPosition; }
    }

    public void Update()
    {
        if (HasTarget && agent.GetCurrentState() != AIStateID.COMBAT)
        {
            agent.stateMachine.ChangeState(AIStateID.COMBAT);
            timer = timeBeforeLeavingCombat;
        } else if (!HasTarget && agent.GetCurrentState() == AIStateID.COMBAT)
        {
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                agent.stateMachine.ChangeState(AIStateID.ALERT);
            }
        }

    }


    private void Start()
    {
        StartCoroutine("FindTargetsWithDelay", 0.2f);
    }

    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    void FindVisibleTargets()
    {
        visibleTargets.Clear();
        //currentTargetPosition = new Vector3(0, 0, 0);
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if ((Vector3.Angle (transform.forward, dirToTarget) < viewAngle / 2) || Vector3.Distance(transform.position, target.position) <= innerViewRadius)
            {
                float dstToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                {
                    visibleTargets.Add(target);
                    currentTargetPosition = target.transform.position;
                }
            }
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if(!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    public Transform GetCurrentTarget()
    {
        return visibleTargets[0];
    }
}
