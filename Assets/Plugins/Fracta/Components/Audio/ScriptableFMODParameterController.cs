using FMODUnity;
using UnityEngine;

[CreateAssetMenu(fileName = "Fmod Parameter Controller", menuName = "Fracta/Tools/Audio/Fmod Parameter Controller")]
public class ScriptableFMODParameterController : ScriptableObject
{
    [SerializeField, ParamRef] private string parameterName;
    [SerializeField] private AnimationCurve curve;

    public void SetValue(float value)
    {
        var time = Mathf.Clamp01(value);
        time = curve.Evaluate(time);
        // Debug.Log(parameterName + ": " + time);
        RuntimeManager.StudioSystem.setParameterByName(parameterName, time);
    }

    public void SetValue(int value)
    {
        var time = Mathf.Clamp01(value);
        time = curve.Evaluate(time);
        RuntimeManager.StudioSystem.setParameterByName(parameterName, time);
    }
}
