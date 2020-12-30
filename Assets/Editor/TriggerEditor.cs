using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TriggerBehaviour))]
public class TriggerEditor : Editor
{
    private bool _showTriggerSettings = true;
    
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        TriggerBehaviour trigger = (TriggerBehaviour) target;

        EditorGUILayout.PropertyField(serializedObject.FindProperty("type"), new GUIContent("Trigger Type"));
        
        EditorGUILayout.PropertyField(serializedObject.FindProperty("layerMask"), new GUIContent("Layer Mask"));

        EditorGUILayout.Separator();
        
        _showTriggerSettings = EditorGUILayout.Foldout(_showTriggerSettings, "Trigger Settings");

        if (_showTriggerSettings)
        {
            if (trigger.type == TriggerBehaviour.TriggerType.DelayedTrigger)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("time"), new GUIContent("Delay Time"));
            }
            else if (trigger.type == TriggerBehaviour.TriggerType.OnStayTrigger)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("time"), new GUIContent("Time On Trigger"));
            }

            EditorGUILayout.PropertyField(serializedObject.FindProperty("singleUse"), new GUIContent("Single Use"));

            if (!trigger.singleUse)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("multipleContacts"),
                    new GUIContent("Multiple Contacts"));
            }
        }

        EditorGUILayout.Separator();
        
        EditorGUILayout.PropertyField(serializedObject.FindProperty("triggerEvent"), new GUIContent("Events"));
        
        serializedObject.ApplyModifiedProperties();
    }
}
