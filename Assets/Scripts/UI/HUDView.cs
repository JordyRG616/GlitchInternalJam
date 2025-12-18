using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDView : MonoBehaviour
{
    [SerializeField] private PlayerCharacter charOne;
    [SerializeField] private PlayerCharacter charTwo;
    [Space]
    [SerializeField] private Image healthbar;
    [Space]
    [SerializeField] private Image leftExperienceBar;
    [SerializeField] private Image rightExperienceBar;


    private IEnumerator Start()
    {
        yield return null;
        
        charOne.healthModule.OnDamageTaken += UpdateHealthBar;
        charTwo.healthModule.OnDamageTaken += UpdateHealthBar;

        charOne.OnExpCollected += UpdateLeftExpBar;
        charTwo.OnExpCollected += UpdateRightExpBar;
    }

    private void UpdateRightExpBar(ExpData expData)
    {
        var percentage = expData.experience / expData.maxExperience;
        rightExperienceBar.fillAmount = percentage;
    }

    private void UpdateLeftExpBar(ExpData expData)
    {
        var percentage = expData.experience / expData.maxExperience;
        leftExperienceBar.fillAmount = percentage;
    }

    private void UpdateHealthBar(HealthData obj)
    {
        var percentage = obj.health / obj.maxHealth;
        healthbar.fillAmount = percentage;
    }
}
