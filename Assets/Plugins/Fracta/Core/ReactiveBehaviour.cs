using System;
using UnityEngine;

public class ReactiveBehaviour : MonoBehaviour
{
    [HideInInspector] public Signal<ReactiveBehaviour> OnAwakeReaction = new();
    [HideInInspector] public Signal<ReactiveBehaviour> OnEnableReaction = new();
    [HideInInspector] public Signal<ReactiveBehaviour> OnStartReaction = new();
    [HideInInspector] public Signal<ReactiveBehaviour> OnFixedUpdateReaction = new();
    [HideInInspector] public Signal<ReactiveBehaviour> OnUpdateReaction = new();
    [HideInInspector] public Signal<ReactiveBehaviour> OnDisableReaction = new();
    [HideInInspector] public Signal<ReactiveBehaviour> OnBecameVisibleReaction = new();
    [HideInInspector] public Signal<ReactiveBehaviour> OnBecameInvisibleReaction = new();
    
    [HideInInspector] public Signal<Collision> OnCollisionEnterReaction = new();
    [HideInInspector] public Signal<Collision> OnCollisionExitReaction = new();
    [HideInInspector] public Signal<Collision> OnCollisionStayReaction = new();
    
    [HideInInspector] public Signal<Collider> OnTriggerEnterReaction = new();
    [HideInInspector] public Signal<Collider> OnTriggerExitReaction = new();
    [HideInInspector] public Signal<Collider> OnTriggerStayReaction = new();
    
    [HideInInspector] public Signal<Collision2D> OnCollisionEnter2DReaction = new();
    [HideInInspector] public Signal<Collision2D> OnCollisionExit2DReaction = new();
    [HideInInspector] public Signal<Collision2D> OnCollisionStay2DReaction = new();

    [HideInInspector] public Signal<Trigger2D> OnTriggerEnter2DReaction = new();
    [HideInInspector] public Signal<Trigger2D> OnTriggerExit2DReaction = new();
    [HideInInspector] public Signal<Trigger2D> OnTriggerStay2DReaction = new();


    protected virtual void Awake() => OnAwakeReaction.Fire(this);

    protected virtual void OnEnable() => OnEnableReaction.Fire(this);

    protected virtual void Start() => OnStartReaction.Fire(this);

    protected virtual void FixedUpdate() => OnFixedUpdateReaction.Fire(this);

    protected virtual void Update() => OnUpdateReaction.Fire(this);

    protected virtual void OnDisable() => OnDisableReaction.Fire(this);

    protected virtual void OnBecameVisible() => OnBecameVisibleReaction.Fire(this);

    protected virtual void OnBecameInvisible() => OnBecameInvisibleReaction.Fire(this);
    
    
    protected virtual void OnCollisionEnter(Collision collision) => OnCollisionEnterReaction.Fire(collision);
    protected virtual void OnCollisionExit(Collision collision) => OnCollisionExitReaction.Fire(collision);
    protected virtual void OnCollisionStay(Collision collision) => OnCollisionStayReaction.Fire(collision);

    protected virtual void OnTriggerEnter(Collider other) => OnTriggerEnterReaction.Fire(other);
    protected virtual void OnTriggerExit(Collider other) => OnTriggerExitReaction.Fire(other);
    protected virtual void OnTriggerStay(Collider other) => OnTriggerStayReaction.Fire(other);

    protected virtual void OnCollisionEnter2D(Collision2D collision) => OnCollisionEnter2DReaction.Fire(collision);
    protected virtual void OnCollisionExit2D(Collision2D collision) => OnCollisionExit2DReaction.Fire(collision);
    protected virtual void OnCollisionStay2D(Collision2D collision) => OnCollisionStay2DReaction.Fire(collision);

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        var param = new Trigger2D(gameObject, other);       
        OnTriggerEnter2DReaction.Fire(param);
    }

    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        var param = new Trigger2D(gameObject, other);       
        OnTriggerExit2DReaction.Fire(param);
    }

    protected virtual void OnTriggerStay2D(Collider2D other)
    {
        var param = new Trigger2D(gameObject, other);       
        OnTriggerStay2DReaction.Fire(param);
    }
}

public class Trigger2D
{
    public Collider2D other;
    public GameObject self;


    public Trigger2D(GameObject self, Collider2D other)
    {
        this.other = other;
        this.self = self;
    }
}