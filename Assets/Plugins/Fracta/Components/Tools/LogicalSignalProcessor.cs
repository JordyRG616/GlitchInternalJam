using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public enum Logic
{
    GreaterThan, 
    GreaterOrEqualTo, 
    LesserThan, 
    LesserOrEqualTo, 
    EgualsTo
}

[AddComponentMenu("Signal Links/Logical Link")]
public class LogicalSignalProcessor : MonoBehaviour
{


    [SerializeField] private ScriptableSignal sourceSignal;
    [SerializeField] private List<LogicProcessorInfo> processes;
    


    private void Awake()
    {
        foreach (var process in processes)
        {
            process.Initialize();
        }

        sourceSignal.Register<int>(InvokeCallbacks);
    }

    public void InvokeCallbacks(int value)
    {
        foreach (var process in processes)
        {
            if(process.Compare(value))
            {
                process.OnTrue?.Invoke(value);
            } else
            {
                process.OnFalse.Invoke(value);
            }
        }
    }
}

[Serializable]
public class LogicProcessorInfo
{
    public Logic logic;
    public int compareAgainst;
    [Space]
    public UnityEvent<int> OnTrue;
    public UnityEvent<int> OnFalse;
    
    private Func<int, bool> compareFuction;
    
    
    public void Initialize()
    {
        switch(logic)
        {
            case Logic.GreaterThan:
                compareFuction = (v) => v > compareAgainst;
                break;
            case Logic.GreaterOrEqualTo:
                compareFuction = (v) => v >= compareAgainst;
                break;
            case Logic.LesserThan:
                compareFuction = (v) => v < compareAgainst;
                break;
            case Logic.LesserOrEqualTo:
                compareFuction = (v) => v <= compareAgainst;
                break;
            case Logic.EgualsTo:
                compareFuction = (v) => v == compareAgainst;
                break;
        }
    }

    public bool Compare(int value)
    {
        return compareFuction.Invoke(value);
    }
}