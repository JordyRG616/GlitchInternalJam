using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

[AddComponentMenu("Fracta/Tween/Composite Executor")]
public class TweenCompositeExecutor : TweenExecutor
{
    private enum CompositeType
    {
        Sequential,
        Concurrent
    }

    protected override bool HideConfigurations => true;
    protected override string PreviewWarning => "Composite executor preview is not yet supported.";

    [Space]
    [SerializeField] private List<TweenExecutionStep> executionSteps;
    
    [BoxGroup("Main")]
    [SerializeField] private CompositeType type;


    protected override void ApplyTweenStep(float time)
    {
    }

    public override void ApplyIncrementalLoop()
    {
    }

    public override void Pause()
    {
        foreach (var step in executionSteps)
        {
            step.Executor.InternalTimeScale = 0;
        }
    }

    public override void Resume()
    {
        foreach (var step in executionSteps)
        {
            step.Executor.InternalTimeScale = 1;
        }
    }

    protected override void SetEndLoopProcess()
    {
        switch (tweenLoopType)
        {
            case TweenLoopType.Restart:
                EndLoopProcess = 
                    () => executionSteps.ForEach(x => x.Executor.Set(0));
                break;
            case TweenLoopType.PingPong:
                EndLoopProcess = () =>
                {
                    direction *= -1;
                };
                break;
            case TweenLoopType.Incremental:;
                EndLoopProcess = () =>
                {
                    executionSteps.ForEach(x =>
                    {
                        x.Executor.ApplyIncrementalLoop();
                        x.Executor.Set(0);
                    });
                };
                break;
        }
    }

    protected override IEnumerator DoTween(bool reverse = false)
    {
        Playing = true;
        direction = 1;
        Func<int, bool> condition = loops >= 0 ? i => i >= 0 : i => true;
        OnTweenStart.Fire();
        executionSteps.ForEach(x => x.Executor.ResetParameters());
        
        yield return new WaitForSeconds(delay);
        
        for (int i = loops; condition.Invoke(i); i--)
        {
            if(applyDelayBetweenLoops && i < loops)
                yield return new WaitForSeconds(delay);
            
            var canTrigger = Random.value < chanceToTrigger;
            foreach (TweenExecutionStep step in executionSteps)
            {
                step.Executor.direction = direction;
                
                if(type == CompositeType.Sequential)
                    yield return StartCoroutine(step.Execute(canTrigger));
                if (type == CompositeType.Concurrent)
                {
                    StartCoroutine(step.Execute(canTrigger));
                }
            }
            
            yield return new WaitWhile(IsExecutingAnyStep);

            if (loops > 0)
            {
                EndLoopProcess?.Invoke();
                OnLoopEnd.Fire();
            }
        }
        
        Playing = false;
        OnTweenEnd.Fire();
    }

    private bool IsExecutingAnyStep()
    {
        foreach (var step in executionSteps)
        {
            if (step.Executor.Playing)
            {
                return true;
            }
        }

        return false;
    }

    public override void Configure()
    {
        foreach (var step in executionSteps)
        {
            step.Executor.Configure();
        }
    }
}

[Serializable]
public class ExecutorFinder
{
    [SerializeField] public GameObject target;
    [SerializeField] public TweenExecutor executor;
}

[Serializable]
public class TweenExecutionStep
{
    [SerializeField] private ExecutorFinder finder;
    [Space]
    // public TweenExecutionStepOperation operation;
    public float intervalToNextStep;
    
    public TweenExecutor Executor => finder.executor;

    
    public IEnumerator Execute(bool canTrigger)
    {
        // switch (operation)
        // {
        //     case TweenExecutionStepOperation.Play:
        //         return Play();
        //     case TweenExecutionStepOperation.Stop:
        //         return Stop();
        //     case TweenExecutionStepOperation.Pause:
        //         return Pause();
        //     case TweenExecutionStepOperation.Resume:
        //         return Resume();
        // }
        if(canTrigger)
            return Play();
        
        return Wait();
    }

    private IEnumerator Play()
    {
        Executor.Play();
        yield return new WaitForSignal(Executor.OnTweenEnd);
        Executor.ResetParameters();
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(Executor.Duration);
    }

    private IEnumerator Pause()
    {
        Executor.Pause();
        yield return null;
    }

    private IEnumerator Resume()
    {
        Executor.Resume();
        yield return null;
    }

    private IEnumerator Stop()
    {
        Executor.Stop();
        yield return null;
    }
}

public enum TweenExecutionStepOperation
{
    Play,
    Pause,
    Resume,
    Stop,
}