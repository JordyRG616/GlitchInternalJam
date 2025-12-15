using NUnit.Framework;
using UnityEngine;

public class WaitForSignal : CustomYieldInstruction
{
    private bool hasTriggered = false;
    private Signal signal;
    private readonly ScriptableSignal scriptableSignal;

    public WaitForSignal(Signal signal)
    {
        this.signal = signal;
        this.signal += SetTriggered;
    }

    public WaitForSignal(ScriptableSignal signal)
    {
        scriptableSignal = signal;
        scriptableSignal.RegisterToAnySignalFired(SetTriggered);
    }

    private void SetTriggered()
    {
        hasTriggered = true;
    }

    public override bool keepWaiting
    {
        get
        {
            if (hasTriggered)
            {
                if(signal != null) signal -= SetTriggered;
                scriptableSignal?.DelistFromAnySignalFired(SetTriggered);
            }
            
            return !hasTriggered;
        }
    }
}

public class WaitForSignal<T> : CustomYieldInstruction
{
    private bool hasTriggered = false;
    private Signal<T> signal;

    public WaitForSignal(Signal<T> signal)
    {
        this.signal = signal;
        this.signal.Register(SetTriggered);
    }

    private void SetTriggered(T _)
    {
        hasTriggered = true;
    }

    public override bool keepWaiting
    {
        get
        {
            if (hasTriggered)
            {
                signal -= SetTriggered;
            }
            
            return !hasTriggered;
        }
    }
}