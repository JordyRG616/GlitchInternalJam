using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public static class ScreenMenuItens 
{
    [MenuItem("GameObject/UI/Fracta/Segmented Bar")]
    public static void CreateSegmentedBar(MenuCommand menuCommand)
    {
	    CreateFromPrefab("Segmented Bar", menuCommand);
    }
    
    [MenuItem("GameObject/UI/Fracta/Center Filling Bar")]
    public static void CreateCenterFillingBar(MenuCommand menuCommand)
    {
	    CreateFromPrefab("Center Filling Bar", menuCommand);
    }

    
    private static void CreateFromPrefab(string prefabName, MenuCommand menuCommand)
    {
	    var model = Resources.Load(prefabName) as GameObject;
	    var parent = menuCommand.context as GameObject;
        
	    var bar = PrefabUtility.InstantiatePrefab(model) as GameObject;
	    if (bar == null || parent == null) return;
        
	    if (parent == menuCommand.context)
	    {
		    bar.transform.SetParent(parent.transform);
		    ((RectTransform)bar.transform).anchoredPosition = Vector2.zero;
	    }
	    else
	    {
		    SetPositionVisibleinSceneView(parent.GetComponent<RectTransform>(), (RectTransform)bar.transform);
	    }
        
	    bar.transform.localScale = Vector3.one;
	    var pos = bar.transform.localPosition;
	    pos.z = 0;
	    bar.transform.localPosition = pos;
	    
	    PrefabUtility.UnpackPrefabInstance(bar, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
	    Selection.activeGameObject = bar;
    }

    private static void SetPositionVisibleinSceneView(RectTransform canvasRTransform, RectTransform itemTransform)
	{
		// Find the best scene view
		SceneView sceneView = SceneView.lastActiveSceneView;
		if (sceneView == null && SceneView.sceneViews.Count > 0)
			sceneView = SceneView.sceneViews[0] as SceneView;

		// Couldn't find a SceneView. Don't set position.
		if (sceneView == null || sceneView.camera == null)
			return;

		// Create world space Plane from canvas position.
		Vector2 localPlanePosition;
		Camera camera = sceneView.camera;
		Vector3 position = Vector3.zero;
		if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRTransform, new Vector2(camera.pixelWidth / 2, camera.pixelHeight / 2), camera, out localPlanePosition))
		{
			// Adjust for canvas pivot
			localPlanePosition.x = localPlanePosition.x + canvasRTransform.sizeDelta.x * canvasRTransform.pivot.x;
			localPlanePosition.y = localPlanePosition.y + canvasRTransform.sizeDelta.y * canvasRTransform.pivot.y;

			localPlanePosition.x = Mathf.Clamp(localPlanePosition.x, 0, canvasRTransform.sizeDelta.x);
			localPlanePosition.y = Mathf.Clamp(localPlanePosition.y, 0, canvasRTransform.sizeDelta.y);

			// Adjust for anchoring
			position.x = localPlanePosition.x - canvasRTransform.sizeDelta.x * itemTransform.anchorMin.x;
			position.y = localPlanePosition.y - canvasRTransform.sizeDelta.y * itemTransform.anchorMin.y;

			Vector3 minLocalPosition;
			minLocalPosition.x = canvasRTransform.sizeDelta.x * (0 - canvasRTransform.pivot.x) + itemTransform.sizeDelta.x * itemTransform.pivot.x;
			minLocalPosition.y = canvasRTransform.sizeDelta.y * (0 - canvasRTransform.pivot.y) + itemTransform.sizeDelta.y * itemTransform.pivot.y;

			Vector3 maxLocalPosition;
			maxLocalPosition.x = canvasRTransform.sizeDelta.x * (1 - canvasRTransform.pivot.x) - itemTransform.sizeDelta.x * itemTransform.pivot.x;
			maxLocalPosition.y = canvasRTransform.sizeDelta.y * (1 - canvasRTransform.pivot.y) - itemTransform.sizeDelta.y * itemTransform.pivot.y;

			position.x = Mathf.Clamp(position.x, minLocalPosition.x, maxLocalPosition.x);
			position.y = Mathf.Clamp(position.y, minLocalPosition.y, maxLocalPosition.y);
		}

		itemTransform.anchoredPosition = position;
		itemTransform.localRotation = Quaternion.identity;
		itemTransform.localScale = Vector3.one;
	}
    
}
