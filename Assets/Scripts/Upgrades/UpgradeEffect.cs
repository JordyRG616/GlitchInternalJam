using UnityEngine;

public abstract class UpgradeEffect
{
    public abstract void Apply(ArsenalController weapon);

    public virtual UpgradeEffect GetClone()
    {
        return this;
    }
}

public abstract class UpgradeEffect<T> : UpgradeEffect where T : UpgradeEffect<T>
{
    protected abstract T Clone();
    
    public override UpgradeEffect GetClone()
    {
        return Clone();
    }
}