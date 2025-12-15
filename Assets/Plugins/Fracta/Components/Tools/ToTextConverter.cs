using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class ToTextConverter : MonoBehaviour
{
    [SerializeField, Tooltip("Use <#> where you want to the value to be inserted")] 
    private string format;
    [Space]
    public Signal<string> OnConversionDone;
    [Space]
    public UnityEvent<string> OnValueConverted;


    private void Start()
    {
        OnConversionDone = new Signal<string>();
    }

    public void FloatToText(float value)
    {
        var text = new string(format);
        text = text.Replace("<#>", value.ToString());

        OnConversionDone.Fire(text);
        OnValueConverted?.Invoke(text);
    }

    public void IntToText(int value)
    {
        var text = new string(format);
        text = text.Replace("<#>", value.ToString());

        OnConversionDone.Fire(text);
        OnValueConverted?.Invoke(text);
    }

    public void SetFormat(string newFormat)
    {
        format = newFormat;
    }
}
