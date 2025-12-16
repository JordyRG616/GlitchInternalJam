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
    public GameObject LastAttacker => healthData.attacker;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount, GameObject attacker)
    {
        currentHealth -= amount;
        
        FireHealthEvent(amount, attacker);
        
        if (currentHealth <= 0)
            Die();
    }

    private void FireHealthEvent(float amount, GameObject attacker)
    {
        healthData.health = currentHealth;
        healthData.maxHealth = maxHealth;
        healthData.damageTaken = amount;
        healthData.attacker = attacker;
        
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
    public GameObject attacker;
}