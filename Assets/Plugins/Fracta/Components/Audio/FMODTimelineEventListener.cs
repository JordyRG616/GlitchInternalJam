//--------------------------------------------------------------------
//
// This is a Unity behaviour script that demonstrates how to use
// timeline markers in your game code. 
//
// Timeline markers can be implicit - such as beats and bars. Or they 
// can be explicity placed by sound designers, in which case they have 
// a sound designer specified name attached to them.
//
// Timeline markers can be useful for syncing game events to sound
// events.
//
// The script starts a piece of music and then displays on the screen
// the current bar and the last marker encountered.
//
// This document assumes familiarity with Unity scripting. See
// https://unity3d.com/learn/tutorials/topics/scripting for resources
// on learning Unity scripting. 
//
// For information on using FMOD example code in your own programs, visit
// https://www.fmod.com/legal
//
//--------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using FMOD.Studio;
using UnityEngine;
using UnityEngine.Events;

public class FMODTimelineEventListener : MonoBehaviour
{
    private static Action<string> OnEventTriggered;
    
    class TimelineInfo
    {
        public int CurrentMusicBar = 0;
        public FMOD.StringWrapper LastMarker = new FMOD.StringWrapper();
    }

    [SerializeField] private string markerName;
    [SerializeField] private UnityEvent EventCallback;

    EVENT_CALLBACK beatCallback;
    TimelineInfo timelineInfo;
    GCHandle timelineHandle;

    public void Configure(EventInstance musicInstance)
    {
        OnEventTriggered += CheckTrigger;
        
        timelineInfo = new TimelineInfo();

        // Explicitly create the delegate object and assign it to a member so it doesn't get freed
        // by the garbage collected while it's being used
        beatCallback = new FMOD.Studio.EVENT_CALLBACK(TimelineEventCallback);
        
        // Pin the class that will store the data modified during the callback
        timelineHandle = GCHandle.Alloc(timelineInfo);
        // Pass the object through the userdata of the instance
        musicInstance.setUserData(GCHandle.ToIntPtr(timelineHandle));

        musicInstance.setCallback(beatCallback, FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_MARKER);
        musicInstance.start();
    }

    private void CheckTrigger(string marker)
    {
        if (marker == markerName)
        {
            EventCallback.Invoke();
        }
    }

    [AOT.MonoPInvokeCallback(typeof(EVENT_CALLBACK))]
    static FMOD.RESULT TimelineEventCallback(EVENT_CALLBACK_TYPE type, IntPtr instancePtr, IntPtr parameterPtr)
    {
        EventInstance instance = new EventInstance(instancePtr);

        // Retrieve the user data
        IntPtr timelineInfoPtr;
        FMOD.RESULT result = instance.getUserData(out timelineInfoPtr);
        if (result != FMOD.RESULT.OK)
        {
            Debug.LogError("Timeline Callback error: " + result);
        }
        else if (timelineInfoPtr != IntPtr.Zero)
        {
            // Get the object to store beat and marker details
            GCHandle timelineHandle = GCHandle.FromIntPtr(timelineInfoPtr);
            TimelineInfo timelineInfo = (TimelineInfo)timelineHandle.Target;

            switch (type)
            {
                case EVENT_CALLBACK_TYPE.TIMELINE_MARKER:
                {
                    var parameter = (TIMELINE_MARKER_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(FMOD.Studio.TIMELINE_MARKER_PROPERTIES));
                    timelineInfo.LastMarker = parameter.name;
                    OnEventTriggered?.Invoke(timelineInfo.LastMarker);
                    break;
                }
                case EVENT_CALLBACK_TYPE.DESTROYED:
                {
                    // Now the event has been destroyed, unpin the timeline memory so it can be garbage collected
                    timelineHandle.Free();
                    break;
                }
            }
        }
        return FMOD.RESULT.OK;
    }
}