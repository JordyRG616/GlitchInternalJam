using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(menuName = "Fracta/Bridge Board")]
public class BridgeBoard : ScriptableObject, ISerializationCallbackReceiver
{
    [ReadOnly, SerializeField] private List<string> keys = new();
    private Dictionary<string, IBridge> bridges = new();


    public void Register<T>(Func<T> callback, string key)
    {
        var bridge = new Bridge<T>(callback);
        if (bridges.ContainsKey(key))
        {
            bridges[key] = bridge;            
        }
        else
        {
            bridges.Add(key, bridge);
        }
    }

    public void Unregister<T>(string key)
    {
        bridges.Remove(key);
    }

    public T Retrieve<T>(string key)
    {
        var bridge = bridges[key] as Bridge<T>;
        
        if (bridge == null) return default;
        return bridge.GetValue();
    }

    public void OnBeforeSerialize()
    {
        keys = new List<string>(bridges.Keys);
    }

    public void OnAfterDeserialize()
    {
        foreach (var key in keys)
        {
            bridges.TryAdd(key, null);
        }
    }
}
