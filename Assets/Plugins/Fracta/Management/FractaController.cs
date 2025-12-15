using System;
using System.Collections;
using UnityEngine;


public class FractaController : MonoBehaviour
{
    
}


public class FractaController<T> : FractaController where T : FractaManager
{
    protected T Manager { get; private set; }
    
    
    protected virtual void Awake()
    {
        StartCoroutine(WaitToRegister());
    }

    private IEnumerator WaitToRegister()
    {
        yield return new WaitUntil(() => FractaGlobal.HasManagerOfType(typeof(T)));

        Manager = FractaGlobal.GetManager<T>();
        Manager.ReceiveController(this);
    }
}