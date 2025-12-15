using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(ParticleSystem))]
public class ReactiveParticleBehaviour : ReactiveBehaviour
{
    [HideInInspector] public Signal<GameObject> OnCollisionReaction = new();
    [HideInInspector] public Signal<ReactiveParticleBehaviour> OnStopReaction = new();
    [HideInInspector] public Signal<ReactiveParticleBehaviour> OnTriggerReaction = new();

    [SerializeField] private bool UseScriptableSignals;
    [SerializeField] private bool UseUnityEvents;

    [SerializeField, ShowIf("UseScriptableSignals")]
    private ScriptableSignal OnCollisionSignal;
    [SerializeField, ShowIf("UseScriptableSignals")]
    private ScriptableSignal OnStopSignal;
    [SerializeField, ShowIf("UseScriptableSignals")]
    private ScriptableSignal OnTriggerSignal;
    
    [SerializeField, ShowIf("UseUnityEvents")]
    private UnityEvent<GameObject> OnCollisionEvent;
    [SerializeField, ShowIf("UseUnityEvents")]
    private UnityEvent<ReactiveParticleBehaviour> OnStopEvent;
    [SerializeField, ShowIf("UseUnityEvents")]
    private UnityEvent<ReactiveParticleBehaviour> OnTriggerEvent;

    
    private void OnParticleCollision(GameObject other)
    {
        OnCollisionReaction.Fire(other);
        OnCollisionSignal?.Fire(other);
        OnCollisionEvent?.Invoke(other);
    }

    private void OnParticleSystemStopped()
    {
        OnStopReaction.Fire(this);
        OnStopSignal?.Fire(this);
        OnStopEvent?.Invoke(this);
    }

    private void OnParticleTrigger()
    {
        OnTriggerReaction.Fire(this);
        OnTriggerSignal?.Fire(this);
        OnTriggerEvent?.Invoke(this);
    }

    private void OnValidate()
    {
        if (UseScriptableSignals == false)
        {
            OnCollisionSignal = null;
            OnStopSignal = null;
            OnTriggerSignal = null;
        }
        
        if (UseUnityEvents == false)
        {
            OnCollisionEvent = null;
            OnStopEvent = null;
            OnTriggerEvent = null;
        }
    }
}
