using System.Collections;
using UnityEngine;

[System.Serializable]
public class LivingFire : ObjectWeapon
{
    [SerializeField] private Vector2 damageRange;
    [SerializeField] private float interval;
    [SerializeField] private float duration;
    [SerializeField] private float maxDistance;
    
    private UnityPool<ObjectWeaponModule> pool;
    private float counter;
    private float currentInterval;
    
    protected override void Configure(IWeaponModule _)
    {
        pool = new UnityPool<ObjectWeaponModule>(weaponTemplate);
        currentInterval = interval;
        var arsenal = owner.GetComponent<ArsenalController>();
        arsenal.StartCoroutine(HandleSpawn());

        var rewardManager = FractaGlobal.GetManager<RewardManager>();

        var durationUpgrade = new WeaponUpgrade(
            "Living fire: Duration",
            "Increases the duration of the area by 10%",
            UpgradeDuration);

        var damageUpgrade = new WeaponUpgrade(
            "Living fire: Damage",
            "Min. damage per second +0.25\nMax. damage per second +0.5",
            UpgradeDamage);

        var intervalUpgrade = new WeaponUpgrade(
            "Living fire: Spawn rate",
            "Increases the spawn rate by 10%",
            UpgradeInterval);
        
        rewardManager.ReceiveUpgrade(durationUpgrade);
        rewardManager.ReceiveUpgrade(damageUpgrade);
        rewardManager.ReceiveUpgrade(intervalUpgrade);
    }

    protected IEnumerator HandleSpawn()
    {
        while (owner.gameObject.activeInHierarchy)
        {
            counter += Time.deltaTime;

            if (counter >= currentInterval)
            {
                var area = pool.Get();
                var pos = Random.insideUnitCircle * maxDistance;
                
                area.transform.position = pos;
                
                area.OnHit += DoDamage;
                area.OnLifetimeOver += ReturnToPool;
                
                area.gameObject.SetActive(true);
                area.SetDuration(duration);
                counter = 0;
            }
            
            yield return null;
        }
    }

    private void ReturnToPool(ObjectWeaponModule area)
    {
        area.OnHit -= DoDamage;
        area.OnLifetimeOver -= ReturnToPool;
        
        pool.Return(area);
        area.gameObject.SetActive(false);
    }

    public override void Aim(Transform currentTarget)
    {
    }

    public override void DoDamage(HealthModule healthModule)
    {
        Debug.Log("Shadow damage");
        var damage = Random.Range(damageRange.x, damageRange.y);
        
        healthModule.TakeDamage(damage, owner.gameObject);
    }

    public override void SetEmpowered(bool empowered)
    {
        var intervalChange = empowered ? .66f : 1f;
        
        currentInterval = intervalChange * interval;
    }

    protected override Weapon Clone()
    {
        return new LivingFire()
        {
            damageRange = damageRange,
            interval = interval,
            duration = duration,
            maxDistance = maxDistance
        };
    }

    private void UpgradeDuration()
    {
        duration *= 1.1f;
    }

    private void UpgradeDamage()
    {
        damageRange.x += .25f;
        damageRange.y += .5f;
    }

    private void UpgradeInterval()
    {
        interval /= 1.1f;
    }
}
