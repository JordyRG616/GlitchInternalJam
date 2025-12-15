using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[InitializeOnLoad]
class HierarchyIcons
{
    static Texture2D texture;
    static List<int> markedObjects = new List<int>();
	
    static HierarchyIcons()
    {
        // Init
        texture = AssetDatabase.LoadAssetAtPath ("Assets/Plugins/Fracta/Editor/Resources/ManagerIcon.png", typeof(Texture2D)) as Texture2D;
        EditorApplication.update += UpdateCB;
        EditorApplication.hierarchyWindowItemOnGUI += HierarchyItemCB;
    }
	
    static void UpdateCB ()
    {
        // Check here
        GameObject[] go = Object.FindObjectsByType<GameObject>(FindObjectsInactive.Include, FindObjectsSortMode.None);
		
        markedObjects = new List<int> ();
        foreach (GameObject g in go) 
        {
            if (g.GetComponent<FractaManager>() != null)
            {
                markedObjects.Add (g.GetInstanceID ());
            } else if(g.GetComponentsInChildren<FractaManager>(true).Length > 0)
            {
                markedObjects.Add (g.GetInstanceID ());
            }
        }
		
    }

    static void HierarchyItemCB (int instanceID, Rect selectionRect)
    {
        Rect r = new Rect (selectionRect);
        r.xMin = r.xMax - 20;
		
        if (markedObjects.Contains (instanceID)) 
        {
            GUI.Label (r, texture); 
        }
    }
	
}