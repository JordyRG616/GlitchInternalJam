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
        Debug.Log("Shadow damage");
        var damage = Random.Range(damageRange.x, damageRange.y);
        
        healthModule.TakeDamage(damage, owner.gameObject);
    }

    public override void SetEmpowered(bool empowered)
    {
        var intervalChange = empowered ? .66f : 1f;
        
        currentInterval = intervalChange * interval;
    }
}
