using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Tween Preset", menuName = "Fracta/Tween/Preset")]
public class TweenPreset : ScriptableObject
{
    [SerializeField] public AnimationCurve curve;
    
    [BoxGroup("Main", order:0)]
    [Tooltip("Should the animation play when the object is enabled")]
    public bool playOnEnable;
    
    [BoxGroup("Main")]
    [Tooltip("How should the animation be, in seconds."), MinValue(0.1f)]
    public float duration;
    
    [BoxGroup("Main")]
    [Tooltip("Delay before the animation begins, in seconds.")]
    [MinValue(0)] public float delay;
    
    [BoxGroup("Main")]
    [Tooltip("Chance for the animation to play before each loop")]
    [Range(0, 1)]
    public float chanceToTrigger = 1;
    
    [ToggleGroup("HasLoops",  order:1, groupTitle:"Loop Behaviour")]
    [SerializeField]
    private bool HasLoops = false;
    
    [ToggleGroup("HasLoops",  order:1, groupTitle:"Loop Behaviour")]
    [Tooltip("If checked, the animation will pause for the duration of the delay before each loop.")]
    public bool applyDelayBetweenLoops;

    [ToggleGroup("HasLoops",  order:1, groupTitle:"Loop Behaviour")]
    [Tooltip("How many times the animation will repeat. Use -1 to set to infinite loops.")]
    [MinValue(-1)] public int loops = 0;
    
    [ToggleGroup("HasLoops",  order:1, groupTitle:"Loop Behaviour")]
    [Tooltip("How the animation will behave at the end of each loop:\n\n" +
             "Restart: Restart to initial state and play again.\n\n" +
             "PingPong: Will smoothly alternate between initial and final state.\n\n" +
             "Incremental: Will apply an increment to the end state and play again from the current state.")]
    public TweenLoopType tweenLoopType;
    
    [ToggleGroup("elastic", order:2, groupTitle:"Elastic behaviour")]
    [Tooltip("If the animation should return to the default state after playing.")]
    public bool elastic = false;
    
    [ToggleGroup("elastic", order:2, groupTitle:"Elastic behaviour")]
    [Tooltip("The speed of the elastic effect, proportional to the duration.")]
    [MinValue(0.1f), ShowIf("elastic")]
    public float elasticity = 1;
    
    [ToggleGroup("elastic", order:2, groupTitle:"Elastic behaviour")]
    [Tooltip("The default state the animation will return to")]
    [Range(0, 1), ShowIf("elastic")]
    public float defaultState = 0;
    
}
