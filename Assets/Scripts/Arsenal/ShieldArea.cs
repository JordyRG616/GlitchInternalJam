using UnityEngine;

public class ShieldArea : ObjectWeapon
{
    [SerializeField] private Vector2 damageRange;
    
    private ObjectWeaponModule area;
    private float chanceModifier;
    
    protected override void Configure(IWeaponModule _)
    {
        area = Object.Instantiate(weaponTemplate, owner.transform);
        area.transform.localPosition = Vector3.zero;
        area.transform.localRotation = Quaternion.identity;

        area.OnHit += DoDamage;
        area.gameObject.SetActive(true);
        
        var rewardManager = FractaGlobal.GetManager<RewardManager>();

        var damageUpgrade = new WeaponUpgrade(
            "Shield: Damage",
            "Increases Min Damage by 10%\nIncreases Max Damage by 15%",
            UpgradeDamage);
        
        var sizeUpgrade = new WeaponUpgrade(
            "Shield: Size",
            "Increases the size of the shield by 15%",
            UpgradeSize);

        var upgradeChance = new WeaponUpgrade(
            "Shied: Max damage chance",
            "Increases chance of max damage by 5%",
            UpgradeMaxDamageChance);
        
        rewardManager.ReceiveUpgrade(damageUpgrade);
        rewardManager.ReceiveUpgrade(sizeUpgrade);
        rewardManager.ReceiveUpgrade(upgradeChance);
    }

    public override void Aim(Transform currentTarget)
    {
        
    }

    public override void DoDamage(HealthModule healthModule)
    {
        var rdm = Random.value + chanceModifier;
        rdm = Mathf.Clamp01(rdm);
        var damage = Mathf.Lerp(damageRange.x, damageRange.y, rdm);
        
        healthModule.TakeDamage(damage, owner.gameObject);
    }

    public override void SetEmpowered(bool empowered)
    {
        var maxDamage = empowered ? 2 : .5f;
        
        damageRange.y *= maxDamage;
    }

    protected override Weapon Clone()
    {
        return new ShieldArea()
        {
            damageRange = damageRange
        };
    }

    private void UpgradeDamage()
    {
        damageRange.x *= 1.1f;
        damageRange.y *= 1.15f;
    }

    private void UpgradeSize()
    {
        area.gameObject.transform.localScale *= 1.15f;
    }

    private void UpgradeMaxDamageChance()
    {
        chanceModifier += 0.05f;
    }
}
