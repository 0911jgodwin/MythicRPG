using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityCooldown : MonoBehaviour
{
    public KeyCode abilityButtonName;
    public Image darkMask;
    public Text cooldownTextDisplay;

    [SerializeField] private Ability ability;
    [SerializeField] GameObject combatObject;
    private Image myButtonImage;
    private float cooldownDuration;
    private float nextReadyTime;
    private float cooldownTimeLeft;
    private bool cooldownComplete;
    private AbilityManager abilityManager;
    private float currentCooldownDuration;


    void Start()
    {
        abilityManager = GetComponentInParent<AbilityManager>();
        Initialize(ability, combatObject);
    }

    public void Initialize(Ability selectedAbility, GameObject combatObject)
    {
        ability = selectedAbility;
        myButtonImage = GetComponent<Image>();
        myButtonImage.sprite = ability.abilitySprite;
        darkMask.sprite = ability.abilitySprite;
        cooldownDuration = ability.abilityBaseCooldown;
        ability.Initialize(combatObject);
        AbilityReady();
    }

    void Update()
    {
        cooldownComplete = (Time.time > nextReadyTime);
        if (cooldownComplete)
        {
            AbilityReady();
            if (Input.GetKeyDown(abilityButtonName))
            {
                ButtonTriggered();
            }
        }
        else
        {
            Cooldown();
        }
    }

    public void TriggerGlobalCooldown()
    {
        if (cooldownComplete || cooldownTimeLeft < 1f)
        {
            currentCooldownDuration = 1f;
            nextReadyTime = 1f + Time.time;
            cooldownTimeLeft = 1f;
            darkMask.enabled = true;
            cooldownTextDisplay.enabled = false;
        }
    }

    private void AbilityReady()
    {
        cooldownTextDisplay.enabled = false;
        darkMask.enabled = false;
    }

    private void Cooldown()
    {
        cooldownTimeLeft -= Time.deltaTime;
        float roundedCD = Mathf.Round(cooldownTimeLeft);
        cooldownTextDisplay.text = roundedCD.ToString();
        darkMask.fillAmount = (cooldownTimeLeft / currentCooldownDuration);
    }

    private void ButtonTriggered()
    {
        if(ability.TriggerAbility())
        {
            currentCooldownDuration = cooldownDuration;
            nextReadyTime = cooldownDuration + Time.time;
            cooldownTimeLeft = cooldownDuration;
            darkMask.enabled = true;
            cooldownTextDisplay.enabled = true;
            cooldownComplete = false;
            abilityManager.ActivateGlobalCooldown();
        }
    }
}
