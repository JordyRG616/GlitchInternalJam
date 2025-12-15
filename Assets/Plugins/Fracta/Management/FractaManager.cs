using System;
using System.Collections.Generic;
using UnityEngine;

public class FractaManager : MonoBehaviour
{
    protected virtual bool Unique => true;
    protected List<FractaController> Controllers = new List<FractaController>();
    
    protected virtual void Awake()
    {
        if (Unique && FractaGlobal.HasManagerOfType(this.GetType()))
        {
            Destroy(this);
            return;
        }
        
        FractaGlobal.AddManager(this);
    }

    protected virtual void OnDestroy()
    {
        FractaGlobal.RemoveManager(this);
    }
    
    internal void ReceiveController<T>(FractaController<T> controller) where T : FractaManager
    {
        if(this is not T) return;
        
        Controllers.Add(controller);
        Debug.Log(name + " received controller: " + controller.name);
    }

    public T GetController<T>() where T : FractaController
    {
        var controller = Controllers.Find(x => x is T);
        return controller as T;
    }
}
