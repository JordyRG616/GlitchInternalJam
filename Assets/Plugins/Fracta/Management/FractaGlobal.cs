using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class FractaGlobal
{
    private static List<FractaManager> globalManagers = new List<FractaManager>();

    public static void AddManager(FractaManager manager)
    {
        globalManagers.Add(manager);
    }

    public static void RemoveManager(FractaManager manager)
    {
        globalManagers.Remove(manager);
    }

    public static T GetManager<T>() where T : FractaManager
    {
        var manager = globalManagers.Find(x => x is T);

        if (manager == null)
        {
            Debug.LogError("No manager of type " + typeof(T).ToString() + " was found on Global Managers");
            return null;
        }
        return manager as T;
    }

    public static bool HasManagerOfType(Type type)
    {
        var manager = globalManagers.Find(x => x.GetType() == type);
        return manager != null;
    }

    #region EXTENSIONS

    public static T TakeRandom<T>(this List<T> list, bool exclude = false)
    {
        var rdm = Random.Range(0, list.Count);
        var item = list[rdm];
        if(exclude) list.Remove(item);
        return item;
    }

    #endregion
}
