using UnityEngine;

public class ShieldArea : ObjectWeapon
{
    [SerializeField] private Vector2 damageRange;
    
    
    protected override void Configure(IWeaponModule _)
    {
        var area = Object.Instantiate(weaponTemplate, owner.transform);
        area.transform.localPosition = Vector3.zero;
        area.transform.localRotation = Quaternion.identity;

        area.OnHit += DoDamage;
        area.gameObject.SetActive(true);
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
        var maxDamage = empowered ? 2 : .5f;
        
        damageRange.y *= maxDamage;
    }
}
