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
}
