using UnityEngine;

[System.Serializable]
public class MagicSkull : ParticleWeapon
{
    [SerializeField] private Vector2 damageRange;
    

    protected override void Configure(IWeaponModule module)
    {
        module.OnHit += DoDamage;
        particleWeapon.gameObject.SetActive(true);
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
}
