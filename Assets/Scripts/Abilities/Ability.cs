using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Ability : ScriptableObject
{
    public string abilityName = "New Ability";
    public Sprite abilitySprite;
    public GameObject abilityEffect;
    public AudioClip abilitySound;
    public float abilityBaseCooldown;
    public string animationTrigger;

    public abstract void Initialize(GameObject obj);
    public abstract bool TriggerAbility();
}
