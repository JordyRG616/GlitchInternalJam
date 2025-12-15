using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

[AddComponentMenu("Signal Links/Default Link")]
public class SignalLink : MonoBehaviour
{
    public SignalReference signalReference;
    [Space]
    public UnityEvent linkedCallback;


    private void Awake()
    {
        if (signalReference.CreateLink())
        {
            signalReference.Signal += InvokeCallbacks;
        }
        else
        {
        }
    }

    private void InvokeCallbacks()
    {
        linkedCallback?.Invoke();
    }
}
