using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    public List<GameObject> enemies;
    public Targeting playerTargeting;
    public Text enemyCountDisplay;
    public int remainingEnemies;
    void Start()
    {
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Enemy"))
            {
                enemies.Add(child.gameObject);
            }
        }    
    }

    public void ClearEnemy(GameObject enemyToRemove)
    {
        MonoBehaviour[] scripts = enemyToRemove.GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour script in scripts)
        {
            script.enabled = false;
        }
        enemyToRemove.tag = "Untagged";

        enemies.Remove(enemyToRemove);
        playerTargeting.RemoveTargetFromList(enemyToRemove);
    }

    public void Update()
    {
        remainingEnemies = enemies.Count;
        enemyCountDisplay.text = remainingEnemies.ToString();
    }
}
