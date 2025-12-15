using System;
using UnityEngine;

public class ParticleWeaponModule : MonoBehaviour
{
    private ParticleSystem particleSystem;
    public Signal<HealthModule> OnParticleHit;


    private void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
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
}
