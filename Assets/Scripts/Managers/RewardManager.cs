
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RewardManager : FractaManager
{
    [SerializeField] private PlayerCharacter charOne;
    [SerializeField] private PlayerCharacter charTwo;
    [Space]
    [SerializeField] private GameObject leftHeader;
    [SerializeField] private List<PickThreeBox> leftBoxes;
    [Space]
    [SerializeField] private GameObject rightHeader;
    [SerializeField] private List<PickThreeBox> rightBoxes;
    [Space]
    [SerializeField] private ArsenalDatabase arsenalDatabase;

    private float chanceOfLeftWeapon = .1f;    
    private float chanceOfRightWeapon = .1f;    
    
    private List<WeaponUpgrade> currentLeftUpgrades = new List<WeaponUpgrade>();
    private List<WeaponUpgrade> currentRightUpgrades = new List<WeaponUpgrade>();
    

    private void Start()
    {
        charOne.OnLevelUp += ShowLeftRewards;
        charTwo.OnLevelUp += ShowRightRewards;

        foreach (var leftBox in leftBoxes)
        {
            leftBox.OnChoiceMade += Resume;
        }

        foreach (var rightBox in rightBoxes)
        {
            rightBox.OnChoiceMade += Resume;
        }
    }

    private void Resume()
    {
        leftBoxes.ForEach(x => x.gameObject.SetActive(false));
        rightBoxes.ForEach(x => x.gameObject.SetActive(false));
        Time.timeScale = 1;
    }

    private void ShowRightRewards()
    {
        var rewards = new List<WeaponUpgrade>(currentRightUpgrades);

        foreach (var box in rightBoxes)
        {
            if (Random.value < chanceOfRightWeapon)
            {
                var weapon = arsenalDatabase.GetRandomWeapon();
                box.ReceiveWeapon(weapon);
                chanceOfRightWeapon = 0;
            }
            else
            {
                box.ReceiveUpgrade(rewards.TakeRandom(true));
                chanceOfRightWeapon += .1f;
            }

            box.gameObject.SetActive(true);
        }
        
        Time.timeScale = 0;
    }

    private void ShowLeftRewards()
    {
        var rewards = new List<WeaponUpgrade>(currentLeftUpgrades);

        foreach (var box in leftBoxes)
        {
            if (Random.value < chanceOfLeftWeapon)
            {
                var weapon = arsenalDatabase.GetRandomWeapon();
                box.ReceiveWeapon(weapon);
                chanceOfLeftWeapon = 0;
            }
            else
            {
                box.ReceiveUpgrade(rewards.TakeRandom(true));
                chanceOfLeftWeapon += .1f;
            }
            
            box.gameObject.SetActive(true);
        }
        
        Time.timeScale = 0;
    }

    public void ReceiveUpgrade(WeaponUpgrade upgrade, GameObject owner)
    {
        if(owner == charOne.gameObject)
            currentLeftUpgrades.Add(upgrade);
        else
            currentRightUpgrades.Add(upgrade);
    }
}
