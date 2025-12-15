using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SignalReference<T> : ISignalReference
{
    public Type signalType => typeof(T);

    [SerializeField] private GameObject source;
    [SerializeField] private string signalName = "";

    public Signal<T> Signal;

    public bool CreateLink()
    {
        if (source == null) return false;

        foreach (var behaviour in source.GetComponents<MonoBehaviour>())
        {
            foreach (var field in behaviour.GetType().GetFields())
            {
                if (field.Name == signalName && typeof(ISignal).IsAssignableFrom(field.FieldType))
                {
                    Signal = field.GetValue(behaviour) as Signal<T>;
                    return true;
                }
            }
        }

        return false;
    }
}

[Serializable]
public class SignalReference : ISignalReference
{
    public Type signalType => null;

    [SerializeField] private GameObject source;
    [SerializeField] private string signalName = "";

    public Signal Signal;

    public bool CreateLink()
    {
        if (source == null) return false;

        foreach (var behaviour in source.GetComponents<MonoBehaviour>())
        {
            foreach (var field in behaviour.GetType().GetFields())
            {
                if (field.Name == signalName && typeof(ISignal).IsAssignableFrom(field.FieldType))
                {
                    Signal = field.GetValue(behaviour) as Signal;
                    return true;
                }
            }
        }

        return false;
    }
}

public interface ISignalReference
{
    public Type signalType { get; }
}