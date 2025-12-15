using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

[AddComponentMenu("Fracta/Tween/Transform Executor")]
public partial class TransformTweenExecutor : TweenExecutor
{
    [Flags]
    private enum TargetTransformProperty
    {
        None = 0,
        Position = 1,
        Rotation = 2,
        Scale = 4,
        RectSize = 8
    }
    
    [Tooltip("Which properties will be animated. More than one can be selected")]
    [ValidateInput("IsConfigured", "No property selected to animate")]
    [SerializeField, BoxGroup("Main")] 
    private TargetTransformProperty targetProperties;
    
    [SerializeField, BoxGroup("Main"), LabelText("Start from current value")] 
    private bool UseCurrentValueAsInitial;
    
    [SerializeField, BoxGroup("Position", order: 11), LabelText(""), InlineProperty]
    [ShowIf("HasPositionAnimation")]
    private StartEndPair<Vector3> positionConfigurations;
    
    [SerializeField, BoxGroup("Rotation", order: 12), LabelText(""), InlineProperty]
    [ShowIf("HasRotationAnimation")]
    private StartEndPair<Vector3> rotationConfigurations;
    
    [SerializeField, BoxGroup("Scale", order: 13), LabelText(""), InlineProperty]
    [ShowIf("HasScaleAnimation")]
    private StartEndPair<Vector3> scaleConfigurations;
    
    [SerializeField, BoxGroup("Rect Size", order: 14), LabelText(""), InlineProperty]
    [ShowIf("HasSizeAnimation")]
    private StartEndPair<Vector2> rectSizeConfigurations;
    
    [SerializeField, BoxGroup("Executor Behaviour"), ShowIf("IsIncremental")] 
    private Vector3 increment;

    private Action<float> ApplyTween;


    private void Start()
    {
        Configure();
    }

    public override void Configure()
    {
        ApplyTween = null;
        
        if(targetProperties.HasFlag(TargetTransformProperty.Position))
            ApplyTween += AnimatePosition;
        if(targetProperties.HasFlag(TargetTransformProperty.Rotation))
            ApplyTween += AnimateRotation;
        if(targetProperties.HasFlag(TargetTransformProperty.Scale))
            ApplyTween += AnimateScale;
        if (targetProperties.HasFlag(TargetTransformProperty.RectSize))
            ApplyTween += AnimateRectSize;
    }

    private void AnimateRectSize(float time)
    {
        if (transform is RectTransform rectTransform)
        {
            var start = UseCurrentValueAsInitial ? rectTransform.sizeDelta : rectSizeConfigurations.Start; 
            var size = Vector2.LerpUnclamped(start, rectSizeConfigurations.End, time);
            rectTransform.sizeDelta = size;
        }
    }

    private void AnimatePosition(float time)
    {
        Vector3 GetPosition()
        {
            if (transform is RectTransform rectTransform)
                return rectTransform.anchoredPosition;
            return transform.localPosition;
        }
        
        var start =  UseCurrentValueAsInitial ? GetPosition() : positionConfigurations.Start;
        var position = Vector3.LerpUnclamped(start, positionConfigurations.End, time);
        
        if (transform is RectTransform rectTransform)
        {
            rectTransform.anchoredPosition = position;
        }
        else
        {
            transform.localPosition = position;
        }
    }
    
    private void AnimateRotation(float time)
    {
        var start = UseCurrentValueAsInitial ? transform.eulerAngles : rotationConfigurations.Start;
        var rotation = Vector3.LerpUnclamped(start, rotationConfigurations.End, time);
        transform.localRotation = Quaternion.Euler(rotation);        
    }

    private void AnimateScale(float time)
    {
        var start = UseCurrentValueAsInitial ? transform.localScale : scaleConfigurations.Start;
        var scale = Vector3.LerpUnclamped(start, scaleConfigurations.End, time);
        transform.localScale = scale;
    }

    protected override void ApplyTweenStep(float time)
    {
        var value = preset.curve.Evaluate(time);
        ApplyTween?.Invoke(value);
    }

    public override void ApplyIncrementalLoop()
    {
        
    }

    protected override void ApplyTweenStepInEditor(float time)
    {
        var value = preset.curve.Evaluate(time);

        if(targetProperties.HasFlag(TargetTransformProperty.Position))
            AnimatePosition(value);
        if(targetProperties.HasFlag(TargetTransformProperty.Rotation))
            AnimateRotation(value);
        if(targetProperties.HasFlag(TargetTransformProperty.Scale))
            AnimateScale(value);
        if(targetProperties.HasFlag(TargetTransformProperty.RectSize))
            AnimateRectSize(value);
    }
}

[Serializable]
public struct StartEndPair<T>
{
    public T Start;
    public T End;
}