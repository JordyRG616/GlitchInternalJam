
using System;
using System.Collections.Generic;
using UnityEngine;

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
            box.ReceiveUpgrade(rewards.TakeRandom(true));
            box.gameObject.SetActive(true);
        }
        
        Time.timeScale = 0;
    }

    private void ShowLeftRewards()
    {
        var rewards = new List<WeaponUpgrade>(currentLeftUpgrades);

        foreach (var box in leftBoxes)
        {
            box.ReceiveUpgrade(rewards.TakeRandom(true));
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
