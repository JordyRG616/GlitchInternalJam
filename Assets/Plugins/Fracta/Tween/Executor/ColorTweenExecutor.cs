using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("Fracta/Tween/Color Executor")]
public class ColorTweenExecutor : TweenExecutor
{
    [BoxGroup("Main")]
    [SerializeField] private Gradient gradient;

    private Action<Color> ApplyTween;


    private void Start()
    {
        ApplyTween = null;
        Configure();
    }

    public override void Configure()
    {
        if (TryGetComponent<SpriteRenderer>(out var spriteRenderer))
        {
            ApplyTween = color => spriteRenderer.color = color; 
        } else if (TryGetComponent<Image>(out var image))
        {
            ApplyTween = color => image.color = color;
        }
        else
        {
            Debug.LogError($"Can't find SpriteRenderer or Image on {gameObject.name}");
        }
    }

    protected override void ApplyTweenStep(float time)
    {
        var value = preset.curve.Evaluate(time);
        value = Mathf.Clamp01(value);
        var color = gradient.Evaluate(value);
        
        ApplyTween?.Invoke(color);
    }

    protected override void ApplyTweenStepInEditor(float time)
    {
        var value = preset.curve.Evaluate(time);
        value = Mathf.Clamp01(value);
        var color = gradient.Evaluate(value);
        
        if (TryGetComponent(out SpriteRenderer spriteRenderer))
            spriteRenderer.color = color;
        else if (TryGetComponent(out Image image))
            image.color = color;
    }

    public override void ApplyIncrementalLoop()
    {
        
    }
}
