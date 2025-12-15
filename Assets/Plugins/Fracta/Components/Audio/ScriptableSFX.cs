using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;
using FMODUnity;

[CreateAssetMenu(menuName = "Fracta/Tools/Audio/Scriptable SFX")]
public class ScriptableSFX: ScriptableObject
{
    public EventReference sfxReference;
    public bool muted;

    public void Play()
    {
        if (muted) return;
        RuntimeManager.PlayOneShot(sfxReference);
    }

    public EventInstance Start()
    {
        var instance = RuntimeManager.CreateInstance(sfxReference);
        instance.start();
        return instance;
    }

    public void SetMute(bool mute)
    {
        muted = mute;
    }
}