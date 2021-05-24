using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth = 100;
    public float currentHealth;
    bool isDead = false;
    public EnemyManager enemyManager;

    private void Start()
    {
        currentHealth = maxHealth;
        enemyManager = this.GetComponentInParent<EnemyManager>();
    }
    public bool IsDead()
    {
        return isDead;
    }

    public void TakeDamage(int damage)
    {
        currentHealth = Mathf.Max(currentHealth - damage, 0);
        if (currentHealth == 0)
        {
            Die();
        }
    }

    public void Die()
    {
        if (isDead) return;

        isDead = true;
        GetComponent<Animator>().SetTrigger("die");
        enemyManager.ClearEnemy(this.gameObject);
    }
}
