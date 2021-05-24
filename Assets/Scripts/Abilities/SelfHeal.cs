using UnityEngine;

public class SelfHeal : MonoBehaviour
{
    PlayerHealth playerHealth;
    public int healAmount = 10;
    public GameObject abilityEffect;
    public string animationTrigger;
    public AudioClip audioClip;
    public void Initialize()
    {
        playerHealth = GetComponent<PlayerHealth>();
    }

    public bool MakeHeal()
    {
        GetComponent<Animator>().ResetTrigger(animationTrigger);
        GetComponent<Animator>().SetTrigger(animationTrigger);

        playerHealth.TakeDamage(-healAmount);
        AudioSource audio = GetComponent<AudioSource>();
        audio.clip = audioClip;
        audio.Play();
        Instantiate(abilityEffect, this.transform);
        return true;
    }
}
