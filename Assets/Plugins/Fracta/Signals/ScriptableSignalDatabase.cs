using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "Fracta/Signals/Signal Database")]
public class ScriptableSignalDatabase : ScriptableObject
{
    [SerializeField] private List<string> paths;

    [SerializeField] private List<ScriptableSignal> signals;

    
    [Button]
    private void LoadSignals()
    {
        foreach (var path in paths)
        {
            var loadedSignals = Resources.LoadAll<ScriptableSignal>(path);
            signals.AddRange(loadedSignals);
        }
    }

    public void ResetAll()
    {
        signals.ForEach(signal => signal.ResetToDefault());
    }
}
