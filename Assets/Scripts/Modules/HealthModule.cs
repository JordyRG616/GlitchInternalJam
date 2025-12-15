using System;
using UnityEngine;

public class HealthModule : MonoBehaviour
{
    public Signal<HealthData> OnDamageTaken;
    public Signal<HealthModule> OnDeath;
    
    [SerializeField] private float maxHealth;
    
    private float currentHealth;
    private HealthData healthData = new HealthData();
    
    public float Health => currentHealth;
    public float RoundedHealth => Mathf.RoundToInt(currentHealth);
    public float MaxHealth => maxHealth;


    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        
        
        FireHealthEvent(amount);
        
        if (currentHealth <= 0)
            Die();
    }

    private void FireHealthEvent(float amount)
    {
        healthData.health = currentHealth;
        healthData.maxHealth = maxHealth;
        healthData.damageTaken = amount;
        OnDamageTaken.Fire(healthData);
    }

    private void Die()
    {
        OnDeath.Fire(this);
    }
}

public struct HealthData
{
    public float health;
    public float maxHealth;
    public float damageTaken;
}