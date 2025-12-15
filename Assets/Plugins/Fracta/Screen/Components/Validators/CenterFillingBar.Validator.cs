using System;
using UnityEngine;
using UnityEngine.UI;

public partial class CenterFillingBar
{
#if UNITY_EDITOR

    private void OnValidate()
    {
        if (targetImages == null)
        {
            targetImages = GetComponentsInChildren<Image>();
        }
        
        SetTargetFillings();
    }

#endif
}
