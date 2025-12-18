using UnityEngine;

[System.Serializable]
public class BasicWeapon : ParticleWeapon
{
    [SerializeField] private Vector2 damageRange;
    
    private float _damageMultiplier;

    private float Multiplier
    {
        get => _damageMultiplier;
        set => _damageMultiplier = Mathf.Max(1, value);
    }


    protected override void Configure(IWeaponModule module)
    {
        module.OnHit += DoDamage;

        var rewardManager = FractaGlobal.GetManager<RewardManager>();

        var damageUpgrade = new WeaponUpgrade(
            "Basic: Damage",
            "Increases damage by 10%",
            icon,
            UpgradeDamage
        );

        var fireRateUpgrade = new WeaponUpgrade(
            "Basic: Fire Rate",
            "Increases fire rate by 15%",
            icon,
            UpgradeFireRate
        );

        var rangeUpgrade = new WeaponUpgrade(
            "Basic: Range",
            "Increases range by 10%",
            icon,
            UpgradeRange);
        
        rewardManager.ReceiveUpgrade(damageUpgrade, owner.gameObject);
        rewardManager.ReceiveUpgrade(fireRateUpgrade, owner.gameObject);
        rewardManager.ReceiveUpgrade(rangeUpgrade, owner.gameObject);
    }

    public override void DoDamage(HealthModule healthModule)
    {
        var rdm = Random.Range(damageRange.x, damageRange.y);
        var damage = rdm * Multiplier;
        
        healthModule.TakeDamage(damage, owner.gameObject);
    }

    public override void SetEmpowered(bool empowered)
    {
        var multChange = empowered ? 2 : 1;
        
        Multiplier = multChange;
        
        particleWeapon.SetSize(empowered);
    }

    protected override Weapon Clone()
    {
        return new BasicWeapon()
        {
            damageRange = damageRange
        };
    }

    private void UpgradeDamage()
    {
        _damageMultiplier += .1f;
    }

    private void UpgradeFireRate()
    {
        var main = particleWeapon.System.main;
        main.duration *= .85f;
    }

    private void UpgradeRange()
    {
        var main = particleWeapon.System.main;
        main.startLifetimeMultiplier += .1f;
        detectionRadius *= 1.1f;
    }
}
