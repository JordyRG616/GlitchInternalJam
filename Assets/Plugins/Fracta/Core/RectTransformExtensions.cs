using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Fracta.Core
{
    public static class RectTransformExtensions
    {
        public static bool IsFullyWithin(this RectTransform child, RectTransform container) {
            if (child == null || container == null)
                return false;

            Rect childRect = child.GetWorldRect();
            Rect containerRect = container.GetWorldRect();
            bool isWithin = containerRect.Contains(childRect.min) && containerRect.Contains(childRect.max);
            
            return isWithin;

        }
        
        public static bool Overlaps(this RectTransform a, RectTransform b, bool allowInverse = true) {
            if (a == null || b == null)
                return false;
            
            var rectA = a.GetWorldRect();
            var rectB = b.GetWorldRect();
            bool overlaps = rectA.Overlaps(rectB, allowInverse);

            return overlaps;
        }

        public static bool FindUnderRect<T>(this RectTransform rectTransform, out T obj) where T : Component
        {
            var corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);
            var raycaster = rectTransform.root.GetComponent<GraphicRaycaster>();
            var results = new List<RaycastResult>();

            foreach (var corner in corners)
            {
                var eventData = new PointerEventData(EventSystem.current);
                eventData.position = corner;
                raycaster.Raycast(eventData, results);

                foreach (var result in results)
                {
                    if (result.gameObject.TryGetComponent<T>(out obj))
                    {
                        return true;
                    }
                }
            }
            
            obj = null;
            return false;
        }

        public static Rect GetWorldRect(this RectTransform rectTransform) {
            // from https://discussions.unity.com/t/convert-recttransform-rect-to-rect-world/153391/3
            Vector3[] corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);
            // Get the bottom left corner.
            Vector3 position = corners[0];

            Vector2 size = new Vector2(
                rectTransform.lossyScale.x * rectTransform.rect.size.x,
                rectTransform.lossyScale.y * rectTransform.rect.size.y);

            return new Rect(position, size);
        }
        
    }
}