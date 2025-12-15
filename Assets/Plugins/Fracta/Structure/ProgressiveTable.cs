using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "Progressive Table", menuName = "Fracta/Tools/ProgressiveTable")]
public class ProgressiveTable : ScriptableObject
{
    [Header("Configurations")] 
    [SerializeField] private AnimationCurve curve;
    [SerializeField] private bool roundToInt;
    [SerializeField] private bool module;
    [SerializeField, ShowIf("module"), Min(1)] private int moduleValue = 1;
    [SerializeField] private bool includeOrigin;
    [SerializeField] private int points;
    
    [Space, Header("Values")]
    [SerializeField, ReadOnly] private List<float> values = new List<float>();

    private float increment;

    private void OnValidate()
    {
        if (roundToInt) module = false;
        if (module) roundToInt = false;
        
        if(curve == null || curve.keys.Length == 0) return;
        
        values.Clear();
        var time = curve.keys.Last().time;
        increment = time / points;

        for (int i = includeOrigin ? 0 : 1; i < points + 1; i++)
        {
            var value = curve.Evaluate(increment * i);
            
            if(roundToInt) value = Mathf.RoundToInt(value);
            if(module) value = Mathf.FloorToInt(value / moduleValue) * moduleValue;
            
            values.Add(value);
        }
    }

    public float GetValue(int index)
    {
        return values[index];
    }
}
