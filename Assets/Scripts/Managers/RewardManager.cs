
using System.Collections.Generic;

public class RewardManager : FractaManager
{
    private List<WeaponUpgrade> currentUpgrades = new List<WeaponUpgrade>();


    public void ReceiveUpgrade(WeaponUpgrade upgrade)
    {
        currentUpgrades.Add(upgrade);
    }
}
