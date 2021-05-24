using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    public Health targetHealth;
    public bool hasTarget = false;
    public float abilityRange;


    private void DamageEnemy()
    {
        targetHealth.TakeDamage(10);
    }

    private bool CheckIsInRange()
    {
        return Vector3.Distance(transform.position, targetHealth.transform.position) < abilityRange;
    }

    public Health GetTarget()
    {
        return targetHealth;
    }

    public void UpdateTarget(GameObject enemy)
    {
        if (enemy !=null)
        {
            targetHealth = enemy.GetComponent<Health>();
        }
        else
        {
            targetHealth = null;
        }
        
        if (targetHealth != null)
        {
            hasTarget = true;
        }
        else
        {
            hasTarget = false;
        }
    }
}
