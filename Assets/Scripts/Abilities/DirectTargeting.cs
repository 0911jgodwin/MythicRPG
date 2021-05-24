using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectTargeting : MonoBehaviour
{
    public int attackDamage = 10;
    public float attackRange = 2.5f;
    public Health targetHealth;
    public GameObject abilityEffect;
    Combat combatScript;
    public string animationTrigger;
    public AudioClip audioClip;


    public void Initialize()
    {
        combatScript = GetComponent<Combat>();
    }

    public bool MakeAttack()
    {
        targetHealth = combatScript.GetTarget();
        if (targetHealth != null && CheckIsInRange())
        {
            DamageEnemy();
            return true;
        }
        return false;
    }
    private bool CheckIsInRange()
    {
        return Vector3.Distance(transform.position, targetHealth.transform.position) < attackRange;
    }

    private void DamageEnemy()
    {
        GetComponent<Animator>().ResetTrigger(animationTrigger);
        GetComponent<Animator>().SetTrigger(animationTrigger);
        AudioSource audio = GetComponent<AudioSource>();
        audio.clip = audioClip;
        audio.Play();
        targetHealth.TakeDamage(attackDamage);
    }

    //Animation calls this
    void DirectParticleEffect()
    {
        
        Instantiate(abilityEffect, this.transform);
    }
}
