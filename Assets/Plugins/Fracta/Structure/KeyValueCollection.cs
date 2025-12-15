using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class KeyValueCollection<TKey, TValue>
{
    [SerializeField] private List<FractaValuePair<TKey, TValue>> pairs = new();
    private Dictionary<TKey, TValue> dictionary = new();


    public void Initialize()
    {
        foreach (FractaValuePair<TKey, TValue> pair in pairs)
        {
            dictionary.Add(pair.key, pair.value);
        }
    }

    public TValue Get(TKey key)
    {
        if (dictionary.Count == 0)
        {
            Initialize();

            if (dictionary.Count == 0)
            {
                Debug.LogError("Collection is empty");
                return default;
            }
        }
        
        return dictionary[key];
    }

    public void Add(TKey key, TValue value)
    {
        pairs.Add(new (key, value));
        dictionary.Add(key, value);
    }

    public void Remove(TKey key)
    {
        var pair = pairs.Find(x => x.key.Equals(key));
        if (pair == null) return;
        
        pairs.Remove(pair);
        dictionary.Remove(key);
    }
}

[Serializable]
public class FractaValuePair<Tkey, TValue>
{
    public Tkey key;
    public TValue value;


    public FractaValuePair(Tkey key, TValue value)
    {
        this.key = key;
        this.value = value;
    }
}