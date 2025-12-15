using System;
using System.Linq;
using UnityEngine;

public class WeaponModule : MonoBehaviour
{
    [SerializeField] private ParticleWeaponModule particleWeapon;
    [Space]
    [SerializeField] private float detectionRadius;
    [Space]
    [SerializeField] private float damage;

    private Transform currentTarget;
    
    
    private void Start()
    {
        particleWeapon.OnParticleHit += DoDamage;
    }

    private void Update()
    {
        Aim();
    }

    private void DoDamage(HealthModule healthModule)
    {
        healthModule.TakeDamage(damage);
    }

    private void Aim()
    {
        currentTarget = GetTarget();

        particleWeapon.SetActive(currentTarget != null) ;

        if (currentTarget != null)
        {
            var direction = currentTarget.position - transform.position;
            particleWeapon.transform.up = direction.normalized;
        }
    }
    
    private Transform GetTarget()
    {
        var enemies = Physics2D.OverlapCircleAll(transform.position, detectionRadius, LayerMask.GetMask("Enemy"));
        Debug.Log(enemies.Length);
        
        if(enemies.Length == 0) return null;
        
        var orderedEnemies = enemies.OrderBy(x => Vector3.Distance(transform.position, x.transform.position));
        return orderedEnemies.First().transform;
    }
}
