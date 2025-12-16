using UnityEngine;

public abstract class Drop : MonoBehaviour
{
    public Signal<Drop> OnDrop = new Signal<Drop>();
    public Signal<Drop> OnCollect =  new Signal<Drop>();

    protected abstract void Collect();
}
