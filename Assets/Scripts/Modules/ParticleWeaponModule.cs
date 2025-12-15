using System;
using UnityEngine;

public class ParticleWeaponModule : MonoBehaviour
{
    private ParticleSystem particleSystem;
    public Signal<HealthModule> OnParticleHit;

    private float originalSize;
    

    private void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        originalSize = particleSystem.main.startSize.constant;
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.TryGetComponent(out HealthModule healthModule))
        {
            OnParticleHit.Fire(healthModule);
        }
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

    public void SetSize(bool empowered)
    {
        var main = particleSystem.main;
        main.startSize = empowered ? originalSize * 2: originalSize;
    }
}
