using UnityEngine;

public partial class TextMeshTweenExecutor
{
    private bool HasSizeAnimation => targetProperty.HasFlag(TargetTextProperty.FontSize);
    private bool HasColorAnimation => targetProperty.HasFlag(TargetTextProperty.FontColor);
}
