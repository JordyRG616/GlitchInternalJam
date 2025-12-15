using System;
using UnityEngine;

public interface IBridge
{
    
}

public class Bridge<T> : IBridge
{
    private Func<T> callback;


    public Bridge(Func<T> callback)
    {
        this.callback = callback;
    }

    public void Set(Func<T> callback)
    {
        this.callback = callback;
    }

    public T GetValue()
    {
        return callback.Invoke();
    }
}