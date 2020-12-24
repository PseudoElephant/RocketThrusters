using System;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(FloatingBehaviour))]
public class FloatingEditor : Editor
{
    public override void OnInspectorGUI()
    {
        FloatingBehaviour floating = (FloatingBehaviour) target;

        if (GUILayout.Button("Generate Unit Circle"))
        {
            floating.Frequency = new Vector2(1, 1);
            floating.Amplitude = new Vector2(1, 1);
        }

        base.OnInspectorGUI();

        floating.Frequency = EditorGUILayout.Vector2Field("Speed", floating.Frequency);
        floating.Amplitude = EditorGUILayout.Vector2Field("Amplitude", floating.Amplitude);
  
    }
}
