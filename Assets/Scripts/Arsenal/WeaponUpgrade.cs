using System;
using UnityEngine;

public class WeaponUpgrade
{
    public Signal OnUpgradedApplied = new Signal();
    public string upgradeName;
    public string upgradeDescription;
    public Sprite weaponIcon;

    public WeaponUpgrade(string name, string description, Sprite icon, Action upgradeCallback)
    {
        upgradeName = name;
        upgradeDescription = description;
        weaponIcon = icon;
        
        OnUpgradedApplied += upgradeCallback;
    }
    
    public void Apply()
    {
        OnUpgradedApplied.Fire();
    }
    
    
}
