using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "New Data Table", menuName = "Fracta/Tools/Data Table",  order = 0)]
public class DataTable : ScriptableObject
{
    public Type DataType
    {
        get
        {
            if(data.Count == 0) return null;

            return data[0].data.GetType();
        }
    }

    [SerializeField] private bool UseRanges;
    [Space]
    [SerializeField] private List<DataItem> data = new List<DataItem>();

    public T GetRandom<T>() where T : Object
    {
        if (data.Count == 0 || DataType != typeof(T))
        {
            Debug.LogError(name + " does not store itens of type " + typeof(T).Name);
            return default;
        }

        if (!UseRanges)
        {
            return data.TakeRandom().data as T;
        }
        
        var random = Random.Range(0, 1f);
        float lowerBoundary = 0;

        foreach (var item in data)
        {
            if (lowerBoundary <= random && random < item.range.y)
                return item.data as T;
            else 
                lowerBoundary = item.range.x;
        }
        
        return default;
    }

    private void OnValidate()
    {
        data.ForEach(x => x.showRange = UseRanges);
    }
}

[Serializable]
public class DataItem
{
    [HideInInspector] public bool showRange;
    
    public Object data;
    [MinMaxSlider(0, 1), ShowIf("showRange")]
    public Vector2 range;
}