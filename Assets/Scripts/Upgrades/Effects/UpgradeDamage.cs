using UnityEngine;

public class UpgradeDamage : UpgradeEffect<UpgradeDamage>
{
    [SerializeField] private float flatDamageBonus;
    [SerializeField] private float multDamageBonus;
    
    public override void Apply(ArsenalController weapon)
    {
        throw new System.NotImplementedException();
    }

    protected override UpgradeDamage Clone()
    {
        throw new System.NotImplementedException();
    }
}
