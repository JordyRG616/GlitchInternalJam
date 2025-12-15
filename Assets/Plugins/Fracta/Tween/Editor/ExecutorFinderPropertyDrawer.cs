using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ExecutorFinder))]
public class ExecutorFinderPropertyDrawer : PropertyDrawer
{
    private SerializedProperty target;
    private SerializedProperty executor;
    private SerializedProperty operation;
    private SerializedProperty intervalToNextStep;

    private int index;
    
    private List<TweenExecutor> executors = new List<TweenExecutor>();
    private List<string> names = new List<string>();
    private string[] namesArray => names.ToArray(); 
    
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return 40f;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.serializedObject.isEditingMultipleObjects)
        {
            EditorGUI.LabelField(position, "Cannot edit multiple Execution steps.");
            return;
        }
        
        target = property.FindPropertyRelative("target");
        executor = property.FindPropertyRelative("executor");
        names.Clear();
        
        if (target.objectReferenceValue is GameObject obj)
        {
            executors = obj.GetComponents<TweenExecutor>().ToList();
            executors = executors.FindAll(x => x.UsedByComposite);

            foreach (var _executor in executors)
            {
                string name = _executor.identifier == "" ? _executor.GetType().Name : _executor.identifier;
                names.Add(name);
            }
        }
        
        EditorGUI.BeginProperty(position, label, property);


        var rect_one = new Rect(position.x, position.y, position.width, position.height / 2);
        var rect_two = new Rect(position.x, position.y + 2 + position.height / 2, position.width, position.height / 2);


        EditorGUI.PropertyField(rect_one, target);
        if (target.objectReferenceValue != null)
        {
            if (executor.objectReferenceValue != null)
            {
                var _executor = executor.objectReferenceValue as TweenExecutor;
                if (executors.Contains(_executor))
                {
                    index = executors.ToList().IndexOf(_executor);
                }
                else
                {
                    executor.objectReferenceValue = null;
                    index = -1;
                }
            }

            if (executors.Count == 0)
            {
                EditorGUI.LabelField(rect_two, "     ", "The target has no executors marker as Used By Composite.");
            }
            else
            {
                index = EditorGUI.Popup(rect_two, "Executor", index, namesArray);
            }

            if (index != -1 && index < executors.Count)
            {
                executor.objectReferenceValue = executors[index];
            }
        }
        else
        {
            EditorGUI.LabelField(rect_two, "     ", "Assign a target to find an executor.");
        }

        EditorGUI.EndProperty();
    }
}
