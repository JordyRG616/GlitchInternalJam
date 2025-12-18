using UnityEngine;

[System.Serializable]
public class MagicSkull : ParticleWeapon
{
    [SerializeField] private Vector2 damageRange;
    

    protected override void Configure(IWeaponModule module)
    {
        module.OnHit += DoDamage;
        particleWeapon.gameObject.SetActive(true);

        var rewardManager = FractaGlobal.GetManager<RewardManager>();

        var countUpgrade = new WeaponUpgrade(
            "Magic Skull: Skull",
            "Spawns an addition skull",
            UpgradeCount);
        
        var speedUpgrade = new WeaponUpgrade(
            "Magic Skull: Speed",
            "Increases the speed of the skulls by 15%",
            UpgradeSpeed);

        var damageUpgrade = new WeaponUpgrade(
            "Magic Skull: Damage",
            "Increases Min. and Max. damage by 1",
            UpgradeDamage);
        
        rewardManager.ReceiveUpgrade(damageUpgrade);
        rewardManager.ReceiveUpgrade(speedUpgrade);
        rewardManager.ReceiveUpgrade(countUpgrade);
    }

    public override void Aim(Transform currentTarget)
    {
        
    }

    public override void DoDamage(HealthModule healthModule)
    {
        var damage = Random.Range(damageRange.x, damageRange.y);
        
        healthModule.TakeDamage(damage, owner.gameObject);
    }

    public override void SetEmpowered(bool empowered)
    {
        var size = empowered ? 1.5f : 1f;

        var shape = particleWeapon.System.shape;
        shape.radius = size;
    }

    protected override Weapon Clone()
    {
        return new MagicSkull()
        {
            damageRange = damageRange
        };
    }

    private void UpgradeCount()
    {
        particleWeapon.System.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        var emission = particleWeapon.System.emission;
        var burst = emission.GetBurst(0);
        burst.count = new ParticleSystem.MinMaxCurve(burst.count.constant + 1);
        
        emission.SetBurst(0, burst);
        particleWeapon.System.Play();
    }

    private void UpgradeSpeed()
    {
        var main = particleWeapon.System.main;
        main.startSpeedMultiplier += .15f;
    }

    private void UpgradeDamage()
    {
        damageRange += Vector2.one;
    }
}
