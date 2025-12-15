using UnityEngine;
using UnityEngine.EventSystems;

namespace Fracta.Core
{
    public static class FractaGeneralExtensions
    {
        public static bool GetHoveredByType<T>(this PointerEventData data, out T result) where T : MonoBehaviour
        {
            foreach (var hover in data.hovered)
            {
                if(hover.TryGetComponent<T>(out result)) return true;
            }
            
            result = null;
            return false;
        }
    }
}