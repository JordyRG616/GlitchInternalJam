using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(Signal<>))]
public class SignalPropertyDrawer : PropertyDrawer
{
    private static Dictionary<string, string> contextualNames = new()
    {
        { "Single", "Numeric"},
        { "String", "Textual"},
        { "Int32", "Intenger"},
        { "Boolean", "Branching"},
    };


    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight(property, label) + 6;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        var rect = new Rect(position.x + 20, position.y + 3, position.width, position.height - 6);
        var type = fieldInfo.FieldType.GetGenericArguments()[0].Name;
        if (contextualNames.ContainsKey(type)) type = contextualNames[type];
        GUI.enabled = false;
        EditorGUI.TextField(
            rect,
            property.displayName,
            type);
        GUI.enabled = true;

        EditorGUI.EndProperty();
    }
}

[CustomPropertyDrawer(typeof(Signal))]
public class DefaultSignalPropertyDrawer : PropertyDrawer
{

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight(property, label) + 6;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        var rect = new Rect(position.x + 20, position.y + 3, position.width, position.height - 6);
        GUI.enabled = false;
        EditorGUI.TextField(
            rect,
            property.displayName,
            "Empty");
        GUI.enabled = true;

        EditorGUI.EndProperty();
    }
}