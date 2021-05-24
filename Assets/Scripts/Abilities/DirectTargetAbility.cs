using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Abilities/DirectTargetAbility")]
public class DirectTargetAbility : Ability
{
    public int attackDamage = 10;
    public float attackRange = 2.5f;
    private DirectTargeting directTarget;

    public override void Initialize(GameObject obj)
    {
        directTarget = obj.GetComponent<DirectTargeting>();
        directTarget.Initialize();
    }

    public override bool TriggerAbility()
    {
        directTarget.animationTrigger = animationTrigger;
        directTarget.attackDamage = attackDamage;
        directTarget.attackRange = attackRange;
        directTarget.abilityEffect = abilityEffect;
        directTarget.audioClip = abilitySound;
        if (directTarget.MakeAttack())
        {
            return true;
        }
        return false;
    }
}
