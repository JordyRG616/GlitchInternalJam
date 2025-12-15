using System;

[Serializable]
public class Signal<T> : ISignal
{
    public Type ParameterType => typeof(T);
    private Action<T> callback;
    private Action<T, SignalResult> callbackWithResult;

    public bool Suspended { get; set; }
    public SignalResult Result { get; set; } = new();


    public void Fire(T value)
    {
        if (Suspended) return;

        callback?.Invoke(value);
        callbackWithResult?.Invoke(value, Result);
    }

    public void FireDelayed(T value, float delay)
    {
        //! IMPLEMENTA��O DEPENDE DE ASYNC
    }

    public void Set(Action<T> action)
    {
        callback = action;
    }
    
    public void Set(Action<T, SignalResult> action)
    {
        callbackWithResult = action;
    }

    public void Register(Action<T> action)
    {
        callback += action;
    }
    
    public void Register(Action<T, SignalResult> action)
    {
        callbackWithResult += action;
    }

    public void Clear()
    {
        callback = null;
    }

    #region Operators
    public static Signal<T> operator+(Signal<T> a, Signal<T> b)
    {
        a.callback += b.callback;
        return a;
    }

    public static Signal<T> operator -(Signal<T> a, Signal<T> b)
    {
        a.callback -= b.callback;
        return a;
    }

    public static Signal<T> operator +(Signal<T> a, Action<T> b)
    {
        a.callback += b;
        return a;
    }

    public static Signal<T> operator -(Signal<T> a, Action<T> b)
    {
        a.callback -= b;
        return a;
    }
    #endregion
}

[Serializable]
public class Signal : ISignal
{
    public Type ParameterType => null;
    private Action callback;
    private Action<SignalResult> callbackWithResult;

    public bool Suspended { get; set; }
    public SignalResult Result { get; set; } = new();
    


    public void Fire()
    {
        if (Suspended) return;

        callback?.Invoke();
        callbackWithResult?.Invoke(Result);
    }

    public void FireDelayed(float delay)
    {
        //! IMPLEMENTA��O DEPENDE DE ASYNC
    }

    public void Set(Action action)
    {
        callback = action;
    }
    
    public void Set(Action<SignalResult> action)
    {
        callbackWithResult = action;
    }

    public void Register(Action action)
    {
        callback += action;
    }
    
    public void Register(Action<SignalResult> action)
    {
        callbackWithResult += action;
    }
    
    public void Clear()
    {
        callback = null;
        callbackWithResult = null;
    }

    #region Operators
    public static Signal operator +(Signal a, Signal b)
    {
        a.callback += b.callback;
        return a;
    }

    public static Signal operator -(Signal a, Signal b)
    {
        a.callback -= b.callback;
        return a;
    }

    public static Signal operator +(Signal a, Action b)
    {
        a.callback += b;
        return a;
    }

    public static Signal operator -(Signal a, Action b)
    {
        a.callback -= b;
        return a;
    }
    #endregion
}

public interface ISignal
{
    public Type ParameterType { get; }
    public bool Suspended { get; set; }
}

public class SignalResult
{
    public bool success = true;
}