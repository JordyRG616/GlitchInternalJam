using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("Fracta/Tween/Shader Parameter Executor")]
public class ShaderParameterTweenExecutor : TweenExecutor
{
    [BoxGroup("Main")]
    [SerializeField] private List<ShaderParameterData> parametersToAnimate;
    
    private Material material;

    protected override string PreviewWarning => "This preview will modify the values of the materials being animated.";

    private void Start()
    {
        Configure();
    }

    public override void Configure()
    {
        foreach (var executor in GetComponents<ShaderParameterTweenExecutor>())
        {
            if (executor.material != null) material = executor.material;
        }

        if (material == null)
        {
            if (TryGetComponent<Renderer>(out var renderer))
            {
                material = new Material(renderer.material);
                renderer.material = material;
            } else if (TryGetComponent<Graphic>(out var graphic))
            {
                material = new  Material(graphic.material);
                graphic.material = material;
            }
        }
        
        parametersToAnimate.ForEach(p => p.Configure());
    }

    protected override void ApplyTweenStep(float time)
    {
        var value = preset.curve.Evaluate(time);
        
        parametersToAnimate.ForEach(p => p.ApplyTween?.Invoke(material, value));
    }

    protected override void ApplyTweenStepInEditor(float time)
    {
        var value = preset.curve.Evaluate(time);
        Material mat = null;
        
        if (TryGetComponent<Renderer>(out var renderer))
        {
            mat = renderer.material;
        } else if (TryGetComponent<Graphic>(out var graphic))
        {
            mat = graphic.material;
        }
        
        parametersToAnimate.ForEach(p => p.ApplyOnEditor(mat, value));
    }

    public override void ApplyIncrementalLoop()
    {
    }
}

[Serializable]
public partial class ShaderParameterData
{
    public enum ParameterType
    {
        Float,
        Vector2,
        Vector3,
        Vector4,
        Color,
    }
    
    [SerializeField] private string parameterName;
    
    [SerializeField] private ParameterType parameterType;
    
    [SerializeField, LabelText(""), InlineProperty]
    [ShowIf("IsFloat")]
    private StartEndPair<float> floatConfiguration;
    
    [SerializeField, LabelText(""), InlineProperty]
    [ShowIf("IsVector2")]
    private StartEndPair<Vector2> vector2Configuration;
    
    [SerializeField, LabelText(""), InlineProperty]
    [ShowIf("IsVector3")]
    private StartEndPair<Vector3> vector3Configuration;
    
    [SerializeField, LabelText(""), InlineProperty]
    [ShowIf("IsVector4")]
    private StartEndPair<Vector4> vector4Configuration;
    
    [SerializeField, LabelText(""), InlineProperty]
    [ShowIf("IsColor")]
    private Gradient colorGradient;
    
    public Action<Material, float> ApplyTween;
    private string ParameterName => "_" + parameterName;


    public void Configure()
    {
        switch (parameterType)
        {
            case ParameterType.Float:
                ApplyTween = AnimateFloatParameter;
                break;
            case ParameterType.Vector2:
                ApplyTween = AnimateVector2Parameter;
                break;
            case ParameterType.Vector3:
                ApplyTween = AnimateVector3Parameter;
                break;
            case ParameterType.Vector4:
                ApplyTween = AnimateVector4Parameter;
                break;
            case ParameterType.Color:
                ApplyTween = AnimateColorParameter;
                break;
        }
    }
    
    public void ApplyOnEditor(Material mat, float value)
    {
        switch (parameterType)
        {
            case ParameterType.Float:
                AnimateFloatParameter(mat, value);
                break;
            case ParameterType.Vector2:
                AnimateVector2Parameter(mat, value);
                break;
            case ParameterType.Vector3:
                AnimateVector3Parameter(mat, value);
                break;
            case ParameterType.Vector4:
                AnimateVector4Parameter(mat, value);
                break;
            case ParameterType.Color:
                AnimateColorParameter(mat, value);
                break;
        }
    }
    
    private void AnimateFloatParameter(Material material, float time)
    {
        var value = Mathf.Lerp(floatConfiguration.Start,  floatConfiguration.End, time);
        material.SetFloat(ParameterName, value);
    }

    private void AnimateVector2Parameter(Material material, float time)
    {
        var value = Vector2.Lerp(vector2Configuration.Start,  vector2Configuration.End, time);
        material.SetVector(ParameterName, value);
    }

    private void AnimateVector3Parameter(Material material, float time)
    {
        var value = Vector3.Lerp(vector3Configuration.Start,  vector3Configuration.End, time);
        material.SetVector(ParameterName, value);
    }

    private void AnimateVector4Parameter(Material material, float time)
    {
        var value = Vector4.Lerp(vector4Configuration.Start,  vector4Configuration.End, time);
        material.SetVector(ParameterName, value);
    }

    private void AnimateColorParameter(Material material, float time)
    {
        var value = colorGradient.Evaluate(time);
        material.SetColor(ParameterName, value);
    }
}