using System;
using TMPro;
using UnityEngine;

public class TextPopup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI label;
    [SerializeField] private TweenCompositeExecutor tween;
    
    private Action<TextPopup> ReturnCallback {get; set;}

    public bool Popping { get; private set; } = false;


    private void Start()
    {
        tween.OnTweenEnd += Return;
    }

    public TextPopup WithCallback(Action<TextPopup> returnCallback)
    {
        ReturnCallback = returnCallback;
        return this;
    }

    public TextPopup SetText(string text)
    {
        label.text = text;
        return this;
    }

    public void Pop()
    {
        Popping = true;
        tween.Play();
    }

    public void Cancel()
    {
        Popping = false;
        ReturnCallback?.Invoke(this);
        ReturnCallback = null;
        tween.Set(0);
    }

    private void Return()
    {
        Popping = false;
        ReturnCallback?.Invoke(this);
        ReturnCallback = null;
    }
}
