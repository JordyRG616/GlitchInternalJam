using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CallbackDictionary : MonoBehaviour
{
    [SerializeField] private List<CallbackDictionaryEntry> entries = new List<CallbackDictionaryEntry>();


    public void InvokeCallback(string key)
    {
        var entry = entries.Find(x => x.key == key);
        
        if(entry == null) return;
        
        entry.callback.Invoke();
    }
}

[System.Serializable]
public class CallbackDictionaryEntry
{
    public string key;
    public UnityEvent callback;
}
