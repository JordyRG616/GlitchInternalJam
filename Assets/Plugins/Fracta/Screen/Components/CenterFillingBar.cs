using System;
using UnityEngine;
using UnityEngine.UI;

public partial class CenterFillingBar : MonoBehaviour
{
    [SerializeField, Range(0, 1)] private float _value;

    public float Value
    {
        get => _value;
        set
        {
            _value = value;
            SetTargetFillings();
        }
    }
    
    private Image[] targetImages;


    private void Start()
    {
        targetImages = GetComponentsInChildren<Image>();
    }

    private void SetTargetFillings()
    {
        foreach (var targetImage in targetImages)
        {
            targetImage.fillAmount = _value;
        }
    }
}
