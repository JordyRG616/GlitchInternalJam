using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ReactiveScreenElement : MonoBehaviour
{
    public RectTransform rectTransform => transform as RectTransform;
    
    private EventTrigger _eventTrigger;

    public EventTrigger EventTrigger
    {
        get
        {
            _eventTrigger = GetComponent<EventTrigger>();
            if (_eventTrigger == null)
                _eventTrigger = gameObject.AddComponent<EventTrigger>();
            
            return _eventTrigger;
        }
    }

    public EventTrigger.Entry RegisterReaction(EventTriggerType trigger, Action<PointerEventData> callback)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = trigger;
        entry.callback.AddListener(data => callback.Invoke((PointerEventData)data));
        EventTrigger.triggers.Add(entry);
        return entry;
    }

    public void Unregister(EventTrigger.Entry entry)
    {
        EventTrigger.triggers.Remove(entry);
    }
}
