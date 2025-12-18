using System.Collections;
using UnityEngine;

[System.Serializable]
public class Shadow : ObjectWeapon
{
    [SerializeField] private Vector2 damageRange;
    [SerializeField] private float interval;
    [SerializeField] private float duration;
    
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
            "Shadow: Duration",
            "Increases the duration of the area by 10%",
            icon,
            UpgradeDuration);

        var damageUpgrade = new WeaponUpgrade(
            "Shadow: Damage",
            "Min. damage per second -10%\nMax. damage per second +10%",
            icon,
            UpgradeDamage);

        var intervalUpgrade = new WeaponUpgrade(
            "Shadow: Spawn rate",
            "Increases the spawn rate by 15%",
            icon,
            UpgradeInterval);
        
        rewardManager.ReceiveUpgrade(durationUpgrade, owner.gameObject);
        rewardManager.ReceiveUpgrade(damageUpgrade, owner.gameObject);
        rewardManager.ReceiveUpgrade(intervalUpgrade, owner.gameObject);
    }

    protected IEnumerator HandleSpawn()
    {
        while (owner.gameObject.activeInHierarchy)
        {
            counter += Time.deltaTime;

            if (counter >= currentInterval)
            {
                var area = pool.Get();
                area.transform.position = owner.transform.position;
                
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
        return new Shadow()
        {
            damageRange = damageRange,
            interval = interval,
            duration = duration
        };
    }

    private void UpgradeDamage()
    {
        damageRange.x *= .9f;
        damageRange.y *= 1.1f;
    }

    private void UpgradeDuration()
    {
        duration *= 1.1f;
    }

    private void UpgradeInterval()
    {
        interval /= 1.15f;
    }
}
