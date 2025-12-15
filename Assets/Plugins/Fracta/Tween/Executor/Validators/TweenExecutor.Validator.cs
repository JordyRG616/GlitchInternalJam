using System;
using System.Collections;
using Fracta.Core;
using Sirenix.OdinInspector;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using Unity.EditorCoroutines.Editor;
#endif


public abstract partial class TweenExecutor
{
    protected virtual string PreviewWarning => "";
    protected bool HasWarning => PreviewWarning != "";
    protected bool HasDelay => delay > 0;
    protected bool IsIncremental => HasLoops && tweenLoopType == TweenLoopType.Incremental;
    protected virtual bool HideConfigurations => false;

    private void ConfigureFromPreset()
    {
        playOnEnable = preset != null && preset.playOnEnable;
        duration = preset != null ? preset.duration : .1f;
        chanceToTrigger = preset != null ? preset.chanceToTrigger : 1f;
        
        delay = preset != null ? preset.delay : 0f;
        applyDelayBetweenLoops = preset != null && preset.applyDelayBetweenLoops;
        
        loops = preset != null ? preset.loops : 0;
        tweenLoopType = preset != null ? preset.tweenLoopType : TweenLoopType.Restart;

        elastic = preset != null && preset.elastic;
        elasticity = preset != null ? preset.elasticity : 1;
        defaultState = preset != null ? preset.defaultState : 0;

    }

#if UNITY_EDITOR

    private EditorCoroutine currentPreview;
    
    [InfoBox("$PreviewWarning", InfoMessageType.Warning, "HasWarning")]
    [Tooltip(
        "This is a preview of the movement and may not be executed in the same speed and/or duration as in game.")]
    [DisableIf("HideConfigurations")]
    [FoldoutGroup("Debug", order:99), HideInPlayMode, Button("Play")]
    protected virtual void Preview()
    {
        if(currentPreview != null)
            EditorCoroutineUtility.StopCoroutine(currentPreview);
        
        currentPreview = EditorCoroutineUtility.StartCoroutine(PlayInEditor(), this);
    }

    [DisableIf("HideConfigurations")]
    [FoldoutGroup("Debug"), HideInPlayMode, Button("Stop")]
    private void StopPreview()
    {
        if(currentPreview != null)
            EditorCoroutineUtility.StopCoroutine(currentPreview);
    }
    
    [DisableIf("HideConfigurations")]
    [FoldoutGroup("Debug"), HideInPlayMode, Button("Reset")]
    protected virtual void ResetPreview()
    {
        if(currentPreview != null)
            EditorCoroutineUtility.StopCoroutine(currentPreview);
        
        ApplyTweenStepInEditor(0);
    }
    
    private IEnumerator PlayInEditor()
    {
        yield return new EditorWaitForSeconds(delay);
        var currentTime = 0f;
        
        for (int i = loops; i >= 0; i--)
        {
            if(applyDelayBetweenLoops && i < loops)
                yield return new EditorWaitForSeconds(delay);
            
            while (currentTime >= 0 && currentTime <= duration)
            {
                var t = currentTime / duration;
                ApplyTweenStepInEditor(t);
                
                currentTime += Time.deltaTime * direction * InternalTimeScale;
                yield return null;
                
                if(currentTime == 0 || currentTime.Approximately(duration)) break;
            }

            ApplyTweenStepInEditor(1);
        }
        
        
        if (elastic)
        {
            var returnTime = 0f;
            var returnDuration = duration / elasticity;
            var initialState = CurrentTime;

            while (returnTime < returnDuration)
            {
                var time = Mathf.Lerp(initialState, defaultState, returnTime / returnDuration);
                ApplyTweenStepInEditor(time);

                returnTime += Time.deltaTime * InternalTimeScale;
                yield return null;
            }
    
            ApplyTweenStepInEditor(defaultState);
        }
        
        currentPreview = null;
    }
    
    private static AnimationCurve CustomCurveDrawer(AnimationCurve curve, GUIContent label)
    {
        return EditorGUILayout.CurveField(
            label, 
            curve,
            Color.coral, 
            new Rect(0, -.2f, 1, 1.5f),
            GUILayout.MinHeight(50)
            );
    }
    
#endif
}
