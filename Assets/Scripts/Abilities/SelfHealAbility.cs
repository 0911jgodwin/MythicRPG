using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/SelfHealAbility")]
public class SelfHealAbility : Ability
{
    public int healAmount = 10;
    private SelfHeal selfHeal;

    public override void Initialize(GameObject obj)
    {
        selfHeal = obj.GetComponent<SelfHeal>();
        selfHeal.Initialize();
    }

    public override bool TriggerAbility()
    {
        selfHeal.animationTrigger = animationTrigger;
        selfHeal.healAmount = healAmount;
        selfHeal.abilityEffect = abilityEffect;
        selfHeal.audioClip = abilitySound;
        if (selfHeal.MakeHeal())
        {
            return true;
        }
        return false;
    }
}
