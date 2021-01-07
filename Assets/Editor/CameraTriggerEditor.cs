using Behaviours;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(CameraTrigger))]
public class CameraTriggerEditor : TriggerEditor
{
    private bool _showCameraTriggerSettings = true;
    public override void OnInspectorGUI()
    {
        EditorGUILayout.Separator();
        
        base.OnInspectorGUI();
        
        serializedObject.Update();

        CameraTrigger camera = (CameraTrigger) target;
        
        EditorGUILayout.Separator();
        _showCameraTriggerSettings = EditorGUILayout.Foldout(_showCameraTriggerSettings, "Camera Trigger Settings");

        if (_showCameraTriggerSettings)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("useCustomBlend"),
                new GUIContent("Custom Blend"));
            
            
            EditorGUILayout.PropertyField(serializedObject.FindProperty("targetZoom"), new GUIContent("Target Zoom"));
            if (camera.useCustomBlend)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("zoomCurve"), new GUIContent("Zoom Curve"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("zoomTime"), new GUIContent("Zoom Time"));
            }
        }
        
        serializedObject.ApplyModifiedProperties();
    }
}
