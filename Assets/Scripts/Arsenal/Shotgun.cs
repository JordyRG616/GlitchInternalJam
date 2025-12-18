using UnityEngine;

public class Shotgun : ParticleWeapon
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
            "Shotgun: Damage",
            "Increases damage by 10%",
            UpgradeDamage
        );

        var knockbackUpgrade = new WeaponUpgrade(
            "Shotgun: Knockback",
            "Increases knockback by 15%",
            UpgradeKnockback
        );

        var rangeUpgrade = new WeaponUpgrade(
            "Shotgun: Range",
            "Increases range by 15%",
            UpgradeRange);
        
        rewardManager.ReceiveUpgrade(damageUpgrade);
        rewardManager.ReceiveUpgrade(knockbackUpgrade);
        rewardManager.ReceiveUpgrade(rangeUpgrade);
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
        return new Shotgun()
        {
            damageRange = damageRange
        };
    }

    private void UpgradeDamage()
    {
        damageRange *= 1.1f;
    }

    private void UpgradeRange()
    {
        detectionRadius *= 1.15f;

        var main = particleWeapon.System.main;
        main.startLifetimeMultiplier += .2f;
    }

    private void UpgradeKnockback()
    {
        var coll = particleWeapon.System.collision;
        coll.colliderForce *= 1.15f;
    }
}
