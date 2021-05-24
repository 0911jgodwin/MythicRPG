using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    bool isDead = false;
    public Slider playerSlider;
    public PlayerController player;

    private void Start()
    {
        currentHealth = maxHealth;
    }
    public bool IsDead()
    {
        return isDead;
    }

    public void TakeDamage(int damage)
    {
        currentHealth = Mathf.Clamp(Mathf.Max(currentHealth - damage, 0), 0, 100);
        playerSlider.value = currentHealth;
        if (currentHealth == 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return;

        isDead = true;
        player.enabled = false;
        GetComponent<Animator>().SetTrigger("die");
    }
}
