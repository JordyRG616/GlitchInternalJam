using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TweenEventLinker : ReactiveScreenElement
{
     [SerializeField] private List<EventLink> eventExecutors;

     private void Start()
     {
          foreach (var link in eventExecutors)
          {
               RegisterReaction(link.eventTriggerType, _ => link.executorFinder.executor.Play());
          }
     }
}

[System.Serializable]
public struct EventLink
{
     public EventTriggerType eventTriggerType;
     public ExecutorFinder executorFinder;
}
