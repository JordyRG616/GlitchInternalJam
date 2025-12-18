using UnityEngine;

public interface IWeaponModule
{
    public Signal<HealthModule> OnHit { get; set; }
}

[System.Serializable]
public abstract class Weapon
{
    public float detectionRadius;
    protected Transform owner;


    public virtual void Initialize(ArsenalController owner)
    {
        this.owner = owner.transform;
    }

    protected abstract void Configure(IWeaponModule module);
    public abstract void Aim(Transform currentTarget);
    public abstract void DoDamage(HealthModule healthModule);
    public abstract void SetEmpowered(bool empowered);

    protected abstract Weapon Clone();

    public virtual Weapon GetInstance()
    {
        return Clone();
    }

    // {
    //     var damageAmount = currentDamage * damageMultiplier;
    //     healthModule.TakeDamage(damageAmount, gameObject);
    // }
}

[System.Serializable]
public abstract class ParticleWeapon : Weapon
{
    [SerializeField] private ParticleWeaponModule weaponTemplate;
    protected ParticleWeaponModule particleWeapon;

    public override void Initialize(ArsenalController owner)
    {
        base.Initialize(owner);

        particleWeapon = Object.Instantiate(weaponTemplate, owner.transform);
        particleWeapon.Configure();
        particleWeapon.transform.localPosition = Vector3.zero;
        particleWeapon.transform.localRotation = Quaternion.identity;
        Configure(particleWeapon);
    }

    public override void Aim(Transform currentTarget)
    {
        particleWeapon.SetActive(currentTarget != null) ;

        if (currentTarget != null)
        {
            var direction = currentTarget.position - owner.transform.position;
            particleWeapon.transform.up = direction.normalized;
        }
    }

    public override Weapon GetInstance()
    {
        var clone = Clone() as ParticleWeapon;
        clone.weaponTemplate = weaponTemplate;
        clone.detectionRadius = detectionRadius;
        return clone;
    }
}

[System.Serializable]
public abstract class ObjectWeapon : Weapon
{
    [SerializeField] protected ObjectWeaponModule weaponTemplate;

    public override void Initialize(ArsenalController owner)
    {
        base.Initialize(owner);
        
        Configure(weaponTemplate);
    }
    
    public override Weapon GetInstance()
    {
        var clone = Clone() as ObjectWeapon;
        clone.weaponTemplate = weaponTemplate;
        clone.detectionRadius = detectionRadius;
        return clone;
    }
}