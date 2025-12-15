using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine.Events;
using UnityEngine;

public class ScriptableLink : MonoBehaviour
{
    private enum LinkType { Void, Int, Float, String, Boolean, InvertedBoolean}

    private bool IsVoid => type == LinkType.Void;
    private bool IsInt => type == LinkType.Int;
    private bool IsFloat => type == LinkType.Float;
    private bool IsString => type == LinkType.String;
    private bool IsBoolean => type == LinkType.Boolean || type == LinkType.InvertedBoolean;

    [SerializeField] private ScriptableSignal signal;
    [SerializeField] private LinkType type;

    [ShowIf("IsVoid")]
    public bool subscribeToAllSignals = false;
    
    [SerializeField, LabelText("Signal Callback"), ShowIf("IsVoid")] 
    private UnityEvent OnSignalFired;

    [SerializeField, LabelText("Signal Callback"), ShowIf("IsFloat")]
    private UnityEvent<float> OnFloatSignalFired;

    [SerializeField, LabelText("Signal Callback"), ShowIf("IsBoolean")] 
    private UnityEvent<bool> OnBooleanSignalFired;
    
    [SerializeField, LabelText("Signal Callback"), ShowIf("IsInt")] 
    private UnityEvent<int> OnIntSignalFired;
    
    [SerializeField, LabelText("Signal Callback"), ShowIf("IsString")] 
    private UnityEvent<string> OnStringSignalFired;

    public bool initialized;

    
    private void Start()
    {
        initialized = true;
        if (subscribeToAllSignals)
            signal.RegisterToAnySignalFired(Fire);
        else
            signal.Register(Fire);
        
        signal.Register((Action<bool>)Fire);
        signal.Register((Action<float>)Fire);
        signal.Register((Action<int>)Fire);
        signal.Register((Action<string>)Fire);
    }

    private void Fire()
    {
        OnSignalFired?.Invoke();
    }

    private void Fire(bool value)
    {
        if (type == LinkType.InvertedBoolean) value = !value;

        OnBooleanSignalFired?.Invoke(value);
    }

    private void Fire(float value)
    {
        OnFloatSignalFired?.Invoke(value);
    }
    
    
    private void Fire(int value)
    {
        OnIntSignalFired?.Invoke(value);
    }
    
    
    private void Fire(string value)
    {
        OnStringSignalFired?.Invoke(value);
    }
}