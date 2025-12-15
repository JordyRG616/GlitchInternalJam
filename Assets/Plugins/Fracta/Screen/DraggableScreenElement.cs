using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DraggableScreenElement : ReactiveScreenElement
{
    [Foldout("Signals"), SerializeField] 
    public Signal<DraggableScreenElement> OnDragBegun;
    [Foldout("Signals"), SerializeField] 
    public Signal<DraggableScreenElement> OnDrag;
    [Foldout("Signals"), SerializeField] 
    public Signal<DraggableScreenElement> OnDragEnded;

    private Predicate<DraggableScreenElement> CanDrop;
    private Func<Vector2, Vector2> processPosition;
    
    [SerializeField] private bool returnToOriginalParent;
    
    private Vector2 defaultAnchorMin;
    private Vector2 defaultAnchorMax;
    private Vector2 originalPosition;
    private Transform originalParent;
    
    public bool IsDragging { get; private set; }
    
    
    private void Start()
    {
        RegisterReaction(EventTriggerType.BeginDrag, SetDragAnchor);
        RegisterReaction(EventTriggerType.Drag, FollowDrag);
        RegisterReaction(EventTriggerType.EndDrag, ResetDragAnchor);
        
        // CanDrop = _ => true;
    }

    public void SetDropCondition(Predicate<DraggableScreenElement> condition)
    {
        CanDrop = condition;
    }

    public void SetProcessPosition(Func<Vector2, Vector2> function)
    {
        processPosition = function;
    }

    private void SetDragAnchor(PointerEventData eventData)
    {
        defaultAnchorMin = rectTransform.anchorMin;
        defaultAnchorMax = rectTransform.anchorMax;
        
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.zero;
        originalPosition = rectTransform.anchoredPosition;
        originalParent = rectTransform.parent;
        rectTransform.parent = rectTransform.root;
        rectTransform.SetAsLastSibling();
        
        IsDragging = true;
        OnDragBegun.Fire(this);
    }

    private void FollowDrag(PointerEventData eventData)
    {
        if (!IsDragging) return;
        
        var pos = Mouse.current.position.ReadValue();
        pos.x /= Screen.width;
        pos.y /= Screen.height;
        
        var scaler = rectTransform.root.GetComponent<CanvasScaler>();
        var res = scaler.referenceResolution;
        pos.x *= res.x;
        pos.y *= res.y;
        
        if(processPosition != null)
            pos = processPosition.Invoke(pos);
        
        rectTransform.anchoredPosition = pos;
        
        OnDrag.Fire(this);
    }
    
    private void ResetDragAnchor(PointerEventData obj)
    {
        IsDragging = false;
        
        var pos = rectTransform.position;
        rectTransform.anchorMin = defaultAnchorMin;
        rectTransform.anchorMax = defaultAnchorMax;
        rectTransform.position = pos;
        
        if(returnToOriginalParent)
            rectTransform.parent = originalParent;

        if (CanDrop?.Invoke(this) == false)
        {
            rectTransform.parent = originalParent;
            rectTransform.anchoredPosition = originalPosition;            
        }
        
        OnDragEnded.Fire(this);
    }
}
