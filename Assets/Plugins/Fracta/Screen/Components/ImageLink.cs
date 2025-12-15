using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ImageLink : MonoBehaviour
{
    [SerializeField, Required] private Image targetImage;
    
    private Image selfImage;
    
    private bool HasTargetImage => targetImage != null;


    private void Awake()
    {
        selfImage = GetComponent<Image>();        
    }

    private void Update()
    {
        if(!HasTargetImage) return;
            selfImage.sprite = targetImage.sprite;
    }
}
