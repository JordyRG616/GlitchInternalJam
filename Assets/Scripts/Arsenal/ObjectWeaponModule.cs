using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectWeaponModule : MonoBehaviour, IWeaponModule
{
    public Signal<HealthModule> OnHit { get; set; } = new Signal<HealthModule>();
    public Signal<ObjectWeaponModule> OnLifetimeOver { get; set; } = new Signal<ObjectWeaponModule>();

    private float timer;
    private bool ready;

    private List<GameObject> affectedObjects = new List<GameObject>();

    private void Update()
    {
        if (timer > 0.1f)
        {
            ready = false;
            affectedObjects.Clear();
        }
        
        timer += Time.deltaTime;
        if (timer >= 1f)
        {
            ready = true;
            timer = 0;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.TryGetComponent(out HealthModule healthModule))
        {
            if (!ready || affectedObjects.Contains(other.gameObject))
                return;
            
            affectedObjects.Add(other.gameObject);
            OnHit.Fire(healthModule);
        }
    }

    public void SetDuration(float duration)
    {
        StartCoroutine(HandleLifetime(duration));
    }

    private IEnumerator HandleLifetime(float duration)
    {
        yield return new WaitForSeconds(duration);
        
        OnLifetimeOver.Fire(this);
    }
}
