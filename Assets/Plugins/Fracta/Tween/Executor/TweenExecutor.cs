using System;
using System.Collections;
using System.Collections.Generic;
using Fracta.Core;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public enum TweenLoopType
{
    Restart,
    PingPong,
    Incremental
}

public abstract partial class TweenExecutor : MonoBehaviour
{
    [AssetSelector, SerializeField, HideIf("HideConfigurations")]
    [OnValueChanged("ConfigureFromPreset")]
    protected TweenPreset preset;
    
    [BoxGroup("Main", order:0)]
    [Tooltip("Should the animation play when the object is enabled")]
    [SerializeField] 
    protected bool playOnEnable;
    
    [BoxGroup("Main")]
    [Tooltip("How should the animation be, in seconds.")]
    [SerializeField, HideIf("HideConfigurations"), MinValue(0.1f)] 
    protected float duration = 1f;
    
    [BoxGroup("Main")]
    [Tooltip("Delay before the animation begins, in seconds.")]
    [SerializeField, MinValue(0)] protected float delay;
    
    [BoxGroup("Main")]
    [Tooltip("Chance for the animation to play before each loop. Preview of this feature is not yer supported.")]
    [SerializeField, Range(0, 1)]
    protected float chanceToTrigger = 1;
    
    [ToggleGroup("HasLoops", groupTitle:"Loop Behaviour", order:1)]
    [SerializeField]
    protected bool HasLoops = false;
    
    [ToggleGroup("HasLoops", groupTitle:"Loop Behaviour", order:1)]
    [Tooltip("If checked, the animation will pause for the duration of the delay before each loop.")]
    [SerializeField] 
    protected bool applyDelayBetweenLoops;

    [ToggleGroup("HasLoops", groupTitle:"Loop Behaviour", order:1)]
    [Tooltip("How many times the animation will repeat. Use -1 to set to infinite loops. Preview of this feature is not yer supported.")]
    [SerializeField, MinValue(-1)] 
    protected int loops = 0;
    
    [ToggleGroup("HasLoops", groupTitle:"Loop Behaviour", order:1)]
    [Tooltip("How the animation will behave at the end of each loop:\n\n" +
             "Restart: Restart to initial state and play again.\n\n" +
             "PingPong: Will smoothly alternate between initial and final state.\n\n" +
             "Incremental: Will apply an increment to the end state and play again from the current state.")]
    [SerializeField] 
    protected TweenLoopType tweenLoopType;
    
    [ToggleGroup("elastic", order:2, groupTitle:"Elastic behaviour")]
    [SerializeField]
    protected bool elastic = false;
    
    [ToggleGroup("elastic", order:2, groupTitle:"Elastic behaviour")]
    [InfoBox("Elastic animations return to the default state after playing.")]
    [Tooltip("The speed of the elastic effect, proportional to the duration.")]
    [SerializeField, MinValue(0.1f)]
    protected float elasticity = 1;
    
    [ToggleGroup("elastic", order:2, groupTitle:"Elastic behaviour")]
    [Tooltip("The default state the animation will return to")]
    [SerializeField, Range(0, 1)]
    protected float defaultState = 0;
    
    [ToggleGroup("usedByComposite", groupTitle:"Composite behaviour", order:3)] 
    [SerializeField, 
     Tooltip("Whether this executor will be used in a Composite Executor")]
    private bool usedByComposite;
    
    [ToggleGroup("usedByComposite", groupTitle:"Composite behaviour", order:3)] 
    [Tooltip("Used in Composite executors to identify this executor.")]
    public string identifier = "";
        

    [FoldoutGroup("Signals", order:90)] public Signal OnTweenStart;
    [FoldoutGroup("Signals"), ShowIf("HasLoops")] public Signal OnLoopEnd;
    [FoldoutGroup("Signals")] public Signal OnTweenEnd;
    
    protected Coroutine currentTween = null;
    protected Action EndLoopProcess;

    [HideInInspector] public int direction = 1;
    protected float _internalTimeScale = 1;
    protected float _currentTime;
    protected bool _playing = false;
    
    protected float CurrentTime
    {
        get => _currentTime;
        set => _currentTime = Mathf.Clamp(value, 0, duration);
    }

    public float InternalTimeScale
    {
        get => _internalTimeScale;
        set => _internalTimeScale = Mathf.Clamp01(value);
    }

    public bool Playing
    {
        get => _playing && _internalTimeScale > 0; 
        protected set => _playing = value;
    } 
    
    public float Duration => duration;
    public bool UsedByComposite => usedByComposite;
    
    protected abstract void ApplyTweenStep(float time);
    protected virtual void ApplyTweenStepInEditor(float time) { }
    public abstract void ApplyIncrementalLoop();
    public virtual void Configure() { }


    protected virtual void Awake()
    {
        ResetParameters();
        ApplyTweenStep(CurrentTime);
    }

    private void OnEnable()
    {
        if(playOnEnable && gameObject.activeInHierarchy)
            Play();
    }
    
    [FoldoutGroup("Debug"), HideInEditorMode, Button("Play")]
    public virtual void Play()
    {
        if(gameObject.activeInHierarchy == false)
            return;
        
        if (Playing)
        {
            StopAllCoroutines();
        }
        
        InternalTimeScale = 1;
        ResetParameters();
        SetEndLoopProcess();
        currentTween = StartCoroutine(DoTween());
    }
    
    public virtual void PlayReversed()
    {
        if(Playing) return;
        
        ResetParameters();
        SetEndLoopProcess();
        currentTween = StartCoroutine(DoTween(true));
    }
    
    public virtual void PlayFromCurrentState()
    {
        if(Playing) return;
        
        InternalTimeScale = 1;
        SetEndLoopProcess();
        currentTween = StartCoroutine(DoTween());
    }

    [FoldoutGroup("Debug"), HideInEditorMode, Button("Pause")]
    public virtual void Pause()
    {
        InternalTimeScale = 0;
    }

    [FoldoutGroup("Debug"), HideInEditorMode, Button("Resume")]
    public virtual void Resume()
    {
        InternalTimeScale = 1;
    }

    [FoldoutGroup("Debug"), HideInEditorMode, Button("Reset")]
    public virtual void Stop()
    {
        StopCoroutine(currentTween);
        CurrentTime = 0;
        Playing = false;
        ApplyTweenStep(0);
    }

    protected virtual void SetEndLoopProcess()
    {
        switch (tweenLoopType)
        {
            case TweenLoopType.Restart:
                EndLoopProcess = () => CurrentTime = 0;
                ApplyTweenStep(0);
                break;
            case TweenLoopType.PingPong:
                EndLoopProcess = () => direction *= -1;
                break;
            case TweenLoopType.Incremental:;
                EndLoopProcess = () =>
                {
                    ApplyIncrementalLoop();
                    CurrentTime = 0;
                };
                break;
        }
    }

    public void Set(float time)
    {
        CurrentTime = time / duration;
        
        ApplyTweenStep(CurrentTime);
    }
    
    public void SetNormalized(float time)
    {
        CurrentTime = time;
        
        ApplyTweenStep(CurrentTime);
    }
    
    protected virtual IEnumerator DoTween(bool reverse = false)
    {
        Func<int, bool> condition = loops >= 0 ? i => i >= 0 : i => true;
        Playing = true;
        OnTweenStart.Fire();
        
        yield return new WaitForSeconds(delay);
        
        for (int i = loops; condition.Invoke(i); i--)
        {
            if(applyDelayBetweenLoops && i < loops)
                yield return new WaitForSeconds(delay);
            
            var canTrigger = Random.value < chanceToTrigger;
            while (CurrentTime >= 0 && CurrentTime <= duration)
            {
                var t = reverse ? 1 - CurrentTime / duration : CurrentTime / duration;
                
                if(canTrigger)
                    ApplyTweenStep(t);
                
                CurrentTime += Time.deltaTime * direction * InternalTimeScale;
                yield return new WaitForEndOfFrame();
                
                if(CurrentTime == 0 || CurrentTime.Approximately(duration)) break;
            }

            ApplyTweenStep(reverse ? 0 : 1);
            EndLoopProcess?.Invoke();
            OnLoopEnd.Fire();
        }

        if (elastic)
        {
            var returnTime = 0f;
            var returnDuration = duration / elasticity;
            var initialState = reverse ? 0 : 1;

            while (returnTime < returnDuration)
            {
                var t = Mathf.Lerp(initialState, defaultState, returnTime / returnDuration);
                ApplyTweenStep(t);
                
                returnTime += Time.deltaTime * InternalTimeScale;
                yield return new WaitForEndOfFrame();
            }
    
            ApplyTweenStep(defaultState);
        }
        
        Playing = false;
        OnTweenEnd.Fire();
    }

    public void SetDuration(float value)
    {
        duration = value;
    }

    public void ResetParameters()
    {
        CurrentTime = 0;
        direction = 1;
    }
}
