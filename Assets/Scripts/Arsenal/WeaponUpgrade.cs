
using System;

public class WeaponUpgrade
{
    public Signal OnUpgradedApplied = new Signal();
    public string upgradeName;
    public string upgradeDescription;

    public WeaponUpgrade(string name, string description, Action upgradeCallback)
    {
        upgradeName = name;
        upgradeDescription = description;
        
        OnUpgradedApplied += upgradeCallback;
    }
    
    public void Apply()
    {
        OnUpgradedApplied.Fire();
    }
    
    
}
