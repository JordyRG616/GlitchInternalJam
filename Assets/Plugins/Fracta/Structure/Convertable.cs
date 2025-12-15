using System;
using System.Collections.Generic;
using UnityEngine;

public class Convertable<T> : IEquatable<Convertable<T>>
{
    public T Value { get; set; }

    private Dictionary<Type, Delegate> converters = new();


    public Convertable(T value)
    {
        Value = value;
    }

    public void AddConverter<Tkey>(Func<T, Tkey> converter)
    {
        converters.Add(typeof(Tkey), converter);
    }
    
    private void AddConverter(Type type, Delegate converter)
    {
        converters.Add(type, converter);
    }

    public TOutput To<TOutput>()
    {
        if (converters.TryGetValue(typeof(TOutput), out var converter))
        {
            var func = converter as Func<T, TOutput>;
            return func.Invoke(Value);
        }

        Debug.LogError("Converter to type " + typeof(TOutput) + " was not found");
        return default;
    }
    
    
    #region IEquatable
    
    public bool Equals(Convertable<T> other)
    {
        try
        {
            return other.Value.Equals(Value);
        }
        catch (Exception e)
        {
            throw new Exception(typeof(T).Name + " does not implement IEquatable");
        }
    }
    
    #endregion
    
    #region Operators
    
    public static Convertable<T> operator +(Convertable<T> a, Convertable<T> b)
    {
        dynamic aValue = a.Value;
        dynamic bValue = b.Value;

        try
        {
            a.Value = aValue + bValue;
            foreach (var converter in b.converters)
            {
                a.AddConverter(converter.Key, converter.Value);
            }
            return a;
        }
        catch (Exception e)
        {
            throw new Exception(typeof(T).Name + " does not implement this operator. Error: " + e.Message);
        }
    }
    
    public static Convertable<T> operator +(Convertable<T> a, T b)
    {
        dynamic bValue = b;

        try
        {
            a.Value += bValue;
            return a;
        }
        catch (Exception e)
        {
            throw new Exception(typeof(T).Name + " does not implement this operator. Error: " + e.Message);
        }
    }
    
    public static Convertable<T> operator -(Convertable<T> a, Convertable<T> b)
    {
        dynamic aValue = a.Value;
        dynamic bValue = b.Value;

        try
        {
            a.Value =  aValue - bValue;
            foreach (var converter in b.converters)
            {
                a.AddConverter(converter.Key, converter.Value);
            }
            return a;
        }
        catch (Exception e)
        {
            throw new Exception(typeof(T).Name + " does not implement this operator. Error: " + e.Message);
        }
    }
    
    public static Convertable<T> operator -(Convertable<T> a, T b)
    {
        dynamic bValue = b;

        try
        {
            a.Value -= bValue;
            return a;
        }
        catch (Exception e)
        {
            throw new Exception(typeof(T).Name + " does not implement this operator. Error: " + e.Message);
        }
    }
    
    public static Convertable<T> operator *(Convertable<T> a, Convertable<T> b)
    {
        dynamic aValue = a.Value;
        dynamic bValue = b.Value;

        try
        {
            a.Value =  aValue * bValue;
            foreach (var converter in b.converters)
            {
                a.AddConverter(converter.Key, converter.Value);
            }
            return a;
        }
        catch (Exception e)
        {
            throw new Exception(typeof(T).Name + " does not implement this operator. Error: " + e.Message);
        }
    }
    
    public static Convertable<T> operator *(Convertable<T> a, T b)
    {
        dynamic bValue = b;

        try
        {
            a.Value *= bValue;
            return a;
        }
        catch (Exception e)
        {
            throw new Exception(typeof(T).Name + " does not implement this operator. Error: " + e.Message);
        }
    }
    
    public static Convertable<T> operator /(Convertable<T> a, Convertable<T> b)
    {
        dynamic aValue = a.Value;
        dynamic bValue = b.Value;

        try
        {
            a.Value =  aValue / bValue;
            foreach (var converter in b.converters)
            {
                a.AddConverter(converter.Key, converter.Value);
            }
            return a;
        }
        catch (Exception e)
        {
            throw new Exception(typeof(T).Name + " does not implement this operator. Error: " + e.Message);
        }
    }
    
    public static Convertable<T> operator /(Convertable<T> a, T b)
    {
        dynamic bValue = b;

        try
        {
            a.Value /= bValue;
            return a;
        }
        catch (Exception e)
        {
            throw new Exception(typeof(T).Name + " does not implement this operator. Error: " + e.Message);
        }
    }
    
    public static implicit operator T(Convertable<T> a) => a.Value;
    public static implicit operator Convertable<T>(T b) => new Convertable<T>(b);
    
    #endregion

}
