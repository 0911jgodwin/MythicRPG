using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public EnemyManager enemyManager;

    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerHealth.currentHealth <= 0)
        {
            anim.SetTrigger("GameOverFail");
            playerHealth.gameObject.SetActive(false);
        }
        if (enemyManager.remainingEnemies <= 0)
        {
            anim.SetTrigger("GameOverSuccess");
            playerHealth.gameObject.SetActive(false);
        }
    }
}
