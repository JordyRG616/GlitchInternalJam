using UnityEngine;

public partial class TransformTweenExecutor
{
    private bool HasPositionAnimation => targetProperties.HasFlag(TargetTransformProperty.Position);
    private bool HasRotationAnimation => targetProperties.HasFlag(TargetTransformProperty.Rotation);
    private bool HasScaleAnimation => targetProperties.HasFlag(TargetTransformProperty.Scale);
    private bool HasSizeAnimation => targetProperties.HasFlag(TargetTransformProperty.RectSize);

    private bool IsConfigured => targetProperties != TargetTransformProperty.None;
}
