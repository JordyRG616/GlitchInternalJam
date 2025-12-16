using System;
using UnityEngine;

public class DropManager : FractaManager
{
    [SerializeField] private ExperienceDrop experienceDropTemplate;
    
    private UnityPool<ExperienceDrop> experienceDropPool;


    private void Start()
    {
        experienceDropPool = new UnityPool<ExperienceDrop>(experienceDropTemplate);
    }

    public ExperienceDrop RequestExperienceDrop()
    {
        var drop = experienceDropPool.Get();
        drop.OnCollect += ReturnToPool;
        return drop;
    }

    private void ReturnToPool(Drop drop)
    {
        if (drop is ExperienceDrop exp)
        {
            experienceDropPool.Return(exp);
        }
    }
}
