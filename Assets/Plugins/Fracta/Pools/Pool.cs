using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public interface IPool<T>
{
    public int TotalCount { get; }
    public int AvailableCount { get; }
    
    public T Get();
    public T Get(Action<T> callback);
    public void Return(T item);
}

public class Pool<T> : IPool<T> where T : new()
{
    private List<T> allItems =  new List<T>();
    private Queue<T> availableItems = new Queue<T>();
    
    public int TotalCount => allItems.Count;
    public int AvailableCount => availableItems.Count;


    public Pool(int initialCount = 0)
    {
        for (int i = 0; i < initialCount; i++)
        {
            allItems.Add(new T());
        }
        
        CreateQueueFromList();
    }
    
    public T Get()
    {
        T item = default(T);
        if (availableItems.Count > 0)
            item = availableItems.Dequeue();
        else
        {
            item = new T();
            allItems .Add(item);
        }
        
        return item;
    }

    public T Get(Action<T> callback)
    {
        T item = Get();
        callback.Invoke(item);
        return item;
    }

    public void Return(T item)
    {
        availableItems.Enqueue(item);
    }

    private void CreateQueueFromList()
    {
        availableItems.Clear();
        var container = new List<T>(allItems);

        for (int i = 0; i < allItems.Count; i++)
        {
            var obj = container.TakeRandom(true);
            availableItems.Enqueue(obj);
        }
    }
}

public class UnityPool<T> : IPool<T> where T : Object
{
    private Func<Vector3> PositionSetter = null;
    private Func<Quaternion> RotationSetter = null;
    private List<T> models = new List<T>();
    
    private List<T> allItems = new List<T>();
    private Queue<T> availableItems = new Queue<T>();
    private List<T> itemsInUse = new List<T>();
    
    public int TotalCount => allItems.Count;
    public int AvailableCount => availableItems.Count;
    public List<T> ActiveItems => itemsInUse;


    public UnityPool(T model, int initialCount = 0)
    {
        models.Add(model);

        for (int i = 0; i < initialCount; i++)
        {
            var pos = Vector3.zero;
            var rot = Quaternion.identity;
            var chosenModel = models.TakeRandom();
            var item = Object.Instantiate(chosenModel, pos, rot);
            allItems.Add(item);
        }
        
        CreateQueueFromList();
    }
    
    public UnityPool(List<T> models, int initialCount = 0)
    {
        this.models.AddRange(models);

        for (int i = 0; i < initialCount; i++)
        {
            var pos = Vector3.zero;
            var rot = Quaternion.identity;
            var chosenModel = this.models.TakeRandom();
            var item = Object.Instantiate(chosenModel, pos, rot);
            allItems.Add(item);
        }
        
        CreateQueueFromList();
    }
    
    
    public T Get()
    {
        T item = null;
        if (availableItems.Count > 0)
            item = availableItems.Dequeue();
        else
        {
            var pos = PositionSetter?.Invoke() ?? Vector3.zero;
            var rot = RotationSetter?.Invoke() ?? Quaternion.identity;
            var model = models.TakeRandom();
            item = Object.Instantiate(model, pos, rot);
            allItems .Add(item);
        }
        
        itemsInUse.Add(item);
        return item;
    }

    public T Get(Action<T> callback)
    {
        T item = Get();
        callback.Invoke(item);
        return item;
    }

    public void Return(T item)
    {
        availableItems.Enqueue(item);
        itemsInUse.Remove(item);
    }

    public void ReturnAll()
    {
        var container = new List<T>(itemsInUse);
        foreach (var item in container)
        {
            Return(item);
        }
    }

    private void CreateQueueFromList()
    {
        availableItems.Clear();
        var container = new List<T>(allItems);

        for (int i = 0; i < allItems.Count; i++)
        {
            var obj = container.TakeRandom(true);
            availableItems.Enqueue(obj);
        }
    }
}