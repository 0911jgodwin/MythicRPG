using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOETargeting : MonoBehaviour
{
    public int attackDamage = 10;
    public float attackRange = 3;
    float maxAngle;
    public float maxAngleInDegrees;
    public GameObject abilityEffect;
    public string animationTrigger;
    public AudioClip audioClip;

    public void Initialize()
    {

    }

    public bool MakeAttack()
    {
        maxAngle = Mathf.Cos(maxAngleInDegrees * Mathf.Deg2Rad / 2.0f);
        //RaycastHit[] spherecast = Physics.SphereCastAll(transform.position, attackRange, transform.forward, 0);
        Collider[] colliders = Physics.OverlapSphere(transform.position, attackRange);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].CompareTag("Enemy"))
            {
                Vector3 enemyLocation = colliders[i].transform.position;
                Vector3 vectorToEnemy = (enemyLocation - transform.position);

                if (Vector3.Dot(vectorToEnemy.normalized, this.transform.forward) > maxAngle)
                {
                    DamageEnemy(colliders[i].transform.gameObject.GetComponent<Health>());
                }

            }
        }
        GetComponent<Animator>().ResetTrigger(animationTrigger);
        GetComponent<Animator>().SetTrigger(animationTrigger);
        AudioSource audio = GetComponent<AudioSource>();
        audio.clip = audioClip;
        audio.Play();
        return true;
        /*for (int i = 0; i < spherecast.Length; i++)
        {
            if (spherecast[i].collider.CompareTag("Enemy"))
            {
                Vector3 enemyLocation = spherecast[i].collider.transform.position;
                Vector3 vectorToEnemy = (enemyLocation - transform.position);

                if (Vector3.Dot(vectorToEnemy.normalized, this.transform.forward) > maxAngle)
                {
                    DamageEnemy(spherecast[i].transform.gameObject.GetComponent<Health>());
                }

            }
        }*/
    }

    //Animation should call this at the appropriate time
    void AOEParticleEffect()
    {
        
        Instantiate(abilityEffect, this.transform);
    }

    private void DamageEnemy(Health targetHealth)
    {
        targetHealth.TakeDamage(attackDamage);
    }
}
