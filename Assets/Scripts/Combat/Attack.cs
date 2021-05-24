using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{

    [SerializeField] int weaponDamage = 5;
    [SerializeField] int weaponRange = 5;
    public PlayerHealth target;
    void Hit()
    {
        if (target == null) return;
        this.transform.LookAt(target.transform);
        float sqrDistance = (target.transform.position - this.transform.position).sqrMagnitude;
        if (sqrDistance < weaponRange * weaponRange)
        {
            target.TakeDamage(weaponDamage);
        }
    }
}
