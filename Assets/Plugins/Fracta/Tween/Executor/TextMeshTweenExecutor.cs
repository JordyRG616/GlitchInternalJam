using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
[AddComponentMenu("Fracta/Tween/Text Mesh Executor")]
public partial class TextMeshTweenExecutor : TweenExecutor
{
    [Flags]
    private enum TargetTextProperty
    {
        None = 0,
        FontSize = 1,
        FontColor = 2,
        TextReveal = 4
    }
    
    [BoxGroup("Main")]
    [SerializeField] private TargetTextProperty targetProperty;

    [BoxGroup("Color", order: 10)]
    [SerializeField, ShowIf("HasColorAnimation")] 
    private Gradient gradient;
    
    [BoxGroup("Font Size",  order: 11)]
    [SerializeField, Min(0), ShowIf("HasSizeAnimation")] 
    private float initialFontSize;
    
    [BoxGroup("Font Size")]
    [SerializeField, ShowIf("HasSizeAnimation")] 
    private float endFontSize;
    
    private Action<float> ApplyTween;
    private TextMeshProUGUI textMesh;
    private string textToReveal;
    
    private void Start()
    {
        Configure();
    }

    public override void Configure()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        textToReveal = textMesh.text;
        ApplyTween = null;
        
        if(targetProperty.HasFlag(TargetTextProperty.FontSize))
            ApplyTween += AnimateSize;
        if(targetProperty.HasFlag(TargetTextProperty.FontColor))    
            ApplyTween += AnimateColor;
        if (targetProperty.HasFlag(TargetTextProperty.TextReveal))
            ApplyTween += RevealText;
    }

    private void RevealText(float time)
    {
        var pos = Mathf.CeilToInt(Mathf.Lerp(0, textToReveal.Length, time));
        textMesh.text = textToReveal.Substring(0, pos);
    }

    private void AnimateColor(float time)
    {
        var color = gradient.Evaluate(time);
        textMesh.color = color;
    }

    private void AnimateSize(float time)
    {
        var size = Mathf.Lerp(initialFontSize, endFontSize, time);
        textMesh.fontSize = size;
    }

    protected override void ApplyTweenStep(float time)
    {
        var value = preset.curve.Evaluate(time);
        ApplyTween?.Invoke(value);
    }

    protected override void ApplyTweenStepInEditor(float time)
    {
        var value = preset.curve.Evaluate(time);
        
        if(targetProperty.HasFlag(TargetTextProperty.FontSize))
            AnimateSize(value);
        if(targetProperty.HasFlag(TargetTextProperty.FontColor))
            AnimateColor(value);
        if(targetProperty.HasFlag(TargetTextProperty.TextReveal))
            RevealText(value);
    }

    public override void ApplyIncrementalLoop()
    {
        
    }
}
