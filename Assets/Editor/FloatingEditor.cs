using System;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(FloatingBehaviour))]
public class FloatingEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        FloatingBehaviour floating = (FloatingBehaviour) target;

        EditorGUILayout.PropertyField(serializedObject.FindProperty("MovingType"), new GUIContent("Moving Block Type"));

        if (floating.MovingType == FloatingBehaviour.MovingBlockType.Waves)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Frequency"), new GUIContent("Frequency"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Amplitude"), new GUIContent("Amplitude"));
        } else
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("PerlinScale"), new GUIContent("General Scale"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("PerlinHeightScale"), new GUIContent("Directional Scale"));
        }

        serializedObject.ApplyModifiedProperties();
    }
}
