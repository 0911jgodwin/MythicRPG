using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    public AbilityCooldown[] abilities;
    void Start()
    {
        abilities = GetComponentsInChildren<AbilityCooldown>();
    }

    public void ActivateGlobalCooldown()
    {
        foreach(AbilityCooldown ability in abilities)
        {
            ability.TriggerGlobalCooldown();
        }
    }
}
