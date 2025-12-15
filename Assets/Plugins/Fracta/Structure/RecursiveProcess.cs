using System;
using System.Collections;
using UnityEngine;

public class RecursiveProcess<T>
{
    private Action<T> process;
    private WaitForSeconds interval;
    private ReactiveBehaviour invoker;
    private float duration = -1;
    private float runningTime = 0;
    private bool running = false;

    private bool ShouldRunAgain
    {
        get
        {
            if(duration < 0) return true;
            if(runningTime < duration) return true;
            return false;
        }
    }

    
    public RecursiveProcess(Action<T> process, float interval, MonoBehaviour invoker, float duration = -1)
    {
        if (invoker is not ReactiveBehaviour)
        {
            Debug.Log("Invoker is not ReactiveBehaviour");
            return;
        }
        
        this.process = process;
        this.interval = new WaitForSeconds(interval);
        this.invoker = invoker as ReactiveBehaviour;
        this.duration = duration;

        this.invoker.OnUpdateReaction += TickTime;
    }

    private void TickTime(ReactiveBehaviour obj)
    {
        if(!running) return;
        runningTime += Time.deltaTime;
    }

    public void Start(T value)
    {
        invoker.StartCoroutine(RepeatProcess(value));
    }

    private IEnumerator RepeatProcess(T value)
    {
        runningTime = 0f;
        while (ShouldRunAgain)
        {
            yield return interval;
            
            process.Invoke(value);
        }
    }
}
