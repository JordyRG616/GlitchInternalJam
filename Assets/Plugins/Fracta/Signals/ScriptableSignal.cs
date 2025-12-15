using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Fracta/Signals/Scriptable Signal")]
public class ScriptableSignal : ScriptableObject
{
    private Signal defaultSignal = new Signal();
    private Signal anySignal =  new Signal();
    private Signal oneShotSignal =  new Signal();
    private int customSignalCount;

    private List<CustomSignalHolder> customSignals = new();
    private List<TimedExecutionStep> timedExecutionSteps = new();

    public CustomSignalHolder<T> CreateCustomSignal<T>()
    {
        var holder = customSignals.Find(x => x.type == typeof(T));

        if(holder != null)
        {
            return holder as CustomSignalHolder<T>;
        }

        var signal = new CustomSignalHolder<T>();
        customSignals.Add(signal);

        return signal;
    }
    

    #region Fire
    
    public void Fire()
    {
        if (defaultSignal == null)
        {
            return;
        }

        defaultSignal.Fire();
        anySignal.Fire();
        
        oneShotSignal.Fire();
        oneShotSignal.Clear();
    }

    public void Fire<T>(T value)
    {
        var holder = customSignals.Find(x => x.type == typeof(T));
        var tHolder = holder as CustomSignalHolder<T>;

        if (tHolder != null) 
            tHolder.signal.Fire(value);
        
        anySignal.Fire();
        oneShotSignal.Fire();
        oneShotSignal.Clear();
    }
    
    #endregion

    #region Register
    
    public void Register(Action callback)
    {
        if (defaultSignal == null)
        {
            defaultSignal = new Signal();
        }

        defaultSignal += callback;
    }
    
    public void RegisterToAnySignalFired(Action callback)
    {
        if (anySignal == null)
        {
            anySignal = new Signal();
        }

        anySignal += callback;
    }

    public void Register<T>(Action<T> callback)
    {
        var holder = customSignals.Find(x => x.type == typeof(T));
        var tHolder = holder as CustomSignalHolder<T>;

        if (tHolder == null)
        {
            tHolder = CreateCustomSignal<T>();
        }

        tHolder.signal.Register(callback);
    }
    
    public void RegisterOneShot(Action callback)
    {
        Debug.Log(callback.Method.Name + " was registered as one shot");
        oneShotSignal += callback;
    }

    public TimedExecutionStep RegisterTimedExecution(Action callback, float duration, bool oneShot = false, int insertAt = -1)
    {
        var step = new TimedExecutionStep();
        step.SetCallback(callback);
        step.duration = duration;
        step.oneTime = oneShot;

        if (insertAt < 0)
        {
            timedExecutionSteps.Add(step);
        }
        else
        {
            timedExecutionSteps.Insert(insertAt, step);
        }
        
        return step;
    }

    public TimedExecutionStep RegisterTimedExecution<T>(Action<T> callback, T arg, float duration, bool oneShot = false, int insertAt = -1)
    {
        var step = new TimedExecutionStep();
        step.SetCallback(callback, arg);
        step.duration = duration;
        step.oneTime = oneShot;

        if (insertAt < 0)
        {
            timedExecutionSteps.Add(step);
        }
        else
        {
            timedExecutionSteps.Insert(insertAt, step);
        }
        
        return step;
    }
    
    #endregion

    #region Delist
    
    public void Delist(Action callback)
    {
        if (defaultSignal == null) return;

        defaultSignal -= callback;
    }
    
    public void DelistFromAnySignalFired(Action callback)
    {
        if (anySignal == null)
        {
            return;
        }

        anySignal -= callback;
    }

    public void CancelOneShot(Action callback)
    {
        if (oneShotSignal == null) return;
        oneShotSignal -= callback;
    }

    public void Delist<T>(Action<T> callback)
    {
        var holder = customSignals.Find(x => x.type == typeof(T));
        var tHolder = holder as CustomSignalHolder<T>;

        if (tHolder == null) return;
        
        tHolder.signal -= callback;
    }

    public void DelistTimedExecutionStep(TimedExecutionStep step)
    {
        timedExecutionSteps.Remove(step);
    }

    #endregion

    #region Timed Execution

    private IEnumerator SetExecution(Action callback)
    {
        var sequence = new List<TimedExecutionStep>(timedExecutionSteps);
        
        foreach (var step in sequence)
        {
            step.Execute();
            
            if(step.oneTime)
                timedExecutionSteps.Remove(step);
            
            yield return new WaitForSeconds(step.duration);
        }
        
        callback?.Invoke();
    }

    public IEnumerator GetExecution(Action endOfExecutionCallback = null)
    {
        IEnumerator execution = SetExecution(endOfExecutionCallback);
        return execution;
    }

    #endregion

    public void ResetToDefault()
    {
        defaultSignal.Clear();
        anySignal.Clear();
        customSignals.Clear();
        timedExecutionSteps.Clear();
    }
}

public class CustomSignalHolder
{
    public Type type;
}

public class CustomSignalHolder<T> : CustomSignalHolder
{
    public Signal<T> signal;

    public CustomSignalHolder()
    {
        signal = new Signal<T>();
        type = typeof(T);
    }
}

[Serializable]
public class TimedExecutionStep
{
    private Action callback;
    public string name;
    public float duration;
    public bool oneTime;


    public void SetCallback(Action callback)
    {
        name = callback.Method.Name;
        this.callback = callback;
    }

    public void SetCallback<T>(Action<T> callback, T arg)
    {
        this.callback = () => callback.Invoke(arg);
    }
    
    public void Execute()
    {
        callback.Invoke();
    }
}
