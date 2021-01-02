using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(BlobBehaviour))]
public class BlobEditor : Editor
{
    private bool _showAttackSettings = true; 
    
    private void OnSceneGUI()
    {
        BlobBehaviour blob = (BlobBehaviour)target;
        Handles.color = Color.blue;
        Handles.DrawWireArc(blob.transform.position, Vector3.forward, Vector3.up, 360, blob.attackRadius);
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("groundMask"), new GUIContent("Ground Mask"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("target"), new GUIContent("Target"));

        _showAttackSettings = EditorGUILayout.Foldout(_showAttackSettings, "Attack Settings");

        if (_showAttackSettings)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("attackRadius"), new GUIContent("Attack Radius"));   
            EditorGUILayout.PropertyField(serializedObject.FindProperty("speed"), new GUIContent("Attack Speed"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("timeBetweenAttacks"), new GUIContent("Time Between Attacks"));
            EditorGUILayout.Separator();
        }
        
        EditorGUILayout.PropertyField(serializedObject.FindProperty("stickDisabledFrames"), new GUIContent("Disable Stick Frames"));

        serializedObject.ApplyModifiedProperties();
    }
}
