using System.Collections;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System;

[CustomPropertyDrawer(typeof(SignalReference<>))]
public class SignalReferencePropertyDrawer : PropertyDrawer
{
    private SerializedProperty source;
    private SerializedProperty signal;

    private int signalPopupIndex = -1;

    private List<string> signals = new List<string>();
    private string[] signalnames => signals.ToArray();

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return 32f;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.serializedObject.isEditingMultipleObjects)
        {
            EditorGUI.LabelField(position, "Cannot edit multiple Signal References.");
            return;
        }

        source = property.FindPropertyRelative("source");
        signal = property.FindPropertyRelative("signalName");
        var target = fieldInfo.FieldType;
        var referenceType = target.GetGenericArguments()[0];

        if (source.objectReferenceValue is GameObject obj)
        {
            signals.Clear();
            foreach (var s in obj.GetComponents<MonoBehaviour>())
            {
                foreach (var p in s.GetType().GetFields())
                {
                    if (typeof(ISignal).IsAssignableFrom(p.FieldType))
                    {
                        var sig = p.GetValue(s) as ISignal;
                        if(sig.ParameterType == referenceType)
                        {
                            signals.Add(p.Name);
                        }
                    }
                }
            }
        }

        EditorGUI.BeginProperty(position, label, property);
        

        var rect_one = new Rect(position.x, position.y, position.width, position.height / 2);
        var rect_two = new Rect(position.x, position.y + position.height / 2, position.width, position.height / 2);


        EditorGUI.PropertyField(rect_one, source);
        if (source.objectReferenceValue != null)
        {
            if(signal.stringValue != "") signalPopupIndex = signalnames.ToList().IndexOf(signal.stringValue);
            signalPopupIndex = EditorGUI.Popup(rect_two, "Signal", signalPopupIndex, signalnames);

            if (signalPopupIndex != -1)
            {
                signal.stringValue = signalnames[signalPopupIndex];
            }
        }

        EditorGUI.EndProperty();
    }
}

[CustomPropertyDrawer(typeof(SignalReference))]
public class DefaultSignalReferencePropertyDrawer : PropertyDrawer
{
    private SerializedProperty source;
    private SerializedProperty signal;

    private int signalPopupIndex = -1;

    private List<string> signals = new List<string>();
    private string[] signalnames => signals.ToArray();

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return 32f;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.serializedObject.isEditingMultipleObjects)
        {
            EditorGUI.LabelField(position, "Cannot edit multiple Signal References.");
            return;
        }

        source = property.FindPropertyRelative("source");
        signal = property.FindPropertyRelative("signalName");

        if (source.objectReferenceValue is GameObject obj)
        {
            signals.Clear();
            foreach (var s in obj.GetComponents<MonoBehaviour>())
            {
                foreach (var p in s.GetType().GetFields())
                {
                    if (typeof(ISignal).IsAssignableFrom(p.FieldType))
                    {
                        signals.Add(p.Name);
                    }
                }
            }
        }

        EditorGUI.BeginProperty(position, label, property);


        var rect_one = new Rect(position.x, position.y, position.width, position.height / 2);
        var rect_two = new Rect(position.x, position.y + position.height / 2, position.width, position.height / 2);


        EditorGUI.PropertyField(rect_one, source);
        if (source.objectReferenceValue != null)
        {
            if (signal.stringValue != "") signalPopupIndex = signalnames.ToList().IndexOf(signal.stringValue);
            signalPopupIndex = EditorGUI.Popup(rect_two, "Signal", signalPopupIndex, signalnames);

            if (signalPopupIndex != -1)
            {
                signal.stringValue = signalnames[signalPopupIndex];
            }
        }

        EditorGUI.EndProperty();
    }
}
