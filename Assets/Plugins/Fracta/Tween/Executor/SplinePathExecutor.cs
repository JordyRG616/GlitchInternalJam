using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Splines;

[AddComponentMenu("Fracta/Tween/Spline Path Executor")]
public class SplinePathExecutor : TweenExecutor
{
    [FoldoutGroup("Debug")]
    [ShowInInspector, ReadOnly] 
    private SplineContainer splineContainer;
    
    [FoldoutGroup("Debug")]
    [SerializeField, ReadOnly]
    private Vector3 offset;
    
    private Spline currentSpline;


    protected override void Awake()
    {
        ResetParameters();
        SetVariables();
    }

    private void Start()
    {
        splineContainer = GetComponent<SplineContainer>();
    }

    protected override void ApplyTweenStep(float time)
    {
        var value = preset.curve.Evaluate(time);
        value = Mathf.Clamp01(value);
        
        AnimatePosition(value);
    }
    
    private void SetVariables()
    {
        currentSpline = new Spline(splineContainer.Spline);

        if (transform is RectTransform rectTransform)
        {
            offset = rectTransform.anchoredPosition;
        }
        else
        {
            offset = transform.localPosition;
        }
    }


    private void AnimatePosition(float time)
    {
        var position =  currentSpline.EvaluatePosition(time);
        
        if (transform is RectTransform rectTransform)
        {
            var pos = new Vector2(position.x, position.y);
            rectTransform.anchoredPosition = pos + (Vector2)offset;
        }
        else
        {
            transform.localPosition = (Vector3)position + offset;
        }
    }

    public override void ApplyIncrementalLoop()
    {
    }

#if Unity_EDITOR
    protected override void Preview()
    {
        SetVariables();
        base.Preview();
    }
    
    protected override void ApplyTweenStepInEditor(float time)
    {
        ApplyTweenStep(time);
    }
    private void OnValidate()
    {
        if (splineContainer == null)
        {
            if (TryGetComponent(out SplineContainer splineContainer))
            {
                this.splineContainer = splineContainer.GetComponent<SplineContainer>();
            }
            else
            {
                this.splineContainer = gameObject.AddComponent<SplineContainer>();
            }
        }
    }

    protected override void ResetPreview()
    {
        if (transform is RectTransform rectTransform)
        {
            rectTransform.anchoredPosition = offset;
        }
        else
        {
            transform.localPosition = offset;
        }
    }
#endif

}
