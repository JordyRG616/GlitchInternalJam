using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(LayoutGroup))]
public partial class SegmentedBar : Selectable
{
    private readonly string overlayName = "Overlay";
    
    [SerializeField, Min(2)]
    [Tooltip("Number of segments in the bar, including the start and end caps")]
    private int segmentCount = 3;
    
    [SerializeField, Min(0)] 
    [Tooltip("Current value of the bar. Cannot be lower than 0, or greater than the segment count")] 
    private int _value;
    
    [SerializeField]
    [Tooltip("Gradient to control the color of the filling of the segments")]
    private Gradient _gradient =  new Gradient()
    {
        alphaKeys = new GradientAlphaKey[] { new (1, 0), new (1, 1) },
        colorKeys = new GradientColorKey[] {new (Color.green, 0), new (Color.green, 1)}
    };
    
    [Space] public UnityEvent<int> onValueChanged;
    [Space] public UnityEvent<float> onValueChangedNormalized;
    
    public int Value
    {
        get => _value;
        set
        {
            if(value == _value) return;
            
            value = Mathf.Clamp(value, 0, segmentCount);
            _value = value;
            
            CheckSegmentsOverlay();
            onValueChanged.Invoke(_value);
            onValueChangedNormalized.Invoke(_value / (float)segmentCount);
        }
    }

    private void CheckSegmentsOverlay()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var overlay = transform.GetChild(i).Find(overlayName);
            var active = i < Value;

            var time = i / (float)(segmentCount - 1);
            overlay.GetComponent<Image>().color = _gradient.Evaluate(time);
            overlay.gameObject.SetActive(active);
        }
    }

    public void SetValue(float value)
    {
        Value = Mathf.RoundToInt(value);
    }
}
