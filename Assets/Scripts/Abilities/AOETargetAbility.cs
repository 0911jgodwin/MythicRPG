using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/AOETargetAbility")]
public class AOETargetAbility : Ability
{
    public int attackDamage = 10;
    public float attackRange = 2.5f;
    private AOETargeting aoeTarget;
    public float maxAngleInDegrees = 180f;

    public override void Initialize(GameObject obj)
    {
        aoeTarget = obj.GetComponent<AOETargeting>();
        aoeTarget.Initialize();
    }

    public override bool TriggerAbility()
    {
        aoeTarget.animationTrigger = animationTrigger;
        aoeTarget.maxAngleInDegrees = maxAngleInDegrees;
        aoeTarget.attackDamage = attackDamage;
        aoeTarget.attackRange = attackRange;
        aoeTarget.abilityEffect = abilityEffect;
        aoeTarget.audioClip = abilitySound;
        if (aoeTarget.MakeAttack())
        {
            return true;
        }
        return false;
    }
}
