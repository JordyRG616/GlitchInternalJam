using System;
using UnityEngine;

public class ParticleWeaponModule : MonoBehaviour, IWeaponModule
{
    public Signal<HealthModule> OnHit { get; set; } = new Signal<HealthModule>();
    public ParticleSystem System => particleSystem;
    
    private ParticleSystem particleSystem;
    private float originalSize;
    

    public void Configure()
    {
        particleSystem = GetComponent<ParticleSystem>();
        originalSize = particleSystem.main.startSize.constant;
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.TryGetComponent(out HealthModule healthModule))
        {
            OnHit.Fire(healthModule);
        }
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

    public void SetSize(bool empowered)
    {
        if(particleSystem == null) return;
        var main = particleSystem.main;
        main.startSize = empowered ? originalSize * 2: originalSize;
    }
}
