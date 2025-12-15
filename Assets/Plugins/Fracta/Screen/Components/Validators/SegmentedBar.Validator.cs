using UnityEngine;
using UnityEngine.UI;

public partial class SegmentedBar
{
    private Transform segment => transform.Find("Segment Model");
    
#if UNITY_EDITOR
    
    private void OnValidate()
    {
        if (_value > segmentCount) _value = segmentCount;
        
        CheckSegments();
        CheckSegmentsOverlay();
    }
    
    private void CheckSegments()
    {
        if(segmentCount < 2) return;
        
        var count = transform.childCount - 1;
        if (count > segmentCount)
        {
            for (int i = 0; i < count - segmentCount; i++)
            {
                var delete = transform.childCount > 3;
                if (delete)
                {
                    UnityEditor.EditorApplication.delayCall += () =>
                    {
                        DestroyImmediate(transform.GetChild(1).gameObject);
                    };
                }
            }
            
        }
        
        if (count < segmentCount)
        {
            for (int i = 0; i < segmentCount - count; i++)
            {
                var seg = Instantiate(segment, transform);
                seg.name = "Segment";
                seg.SetSiblingIndex(1);
                seg.gameObject.SetActive(true);
            }
        }
    }
    
#endif
}
