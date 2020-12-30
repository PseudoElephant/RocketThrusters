using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BulletBehaviour))]
public class BulletEditor : Editor
{
    public override void OnInspectorGUI()
    {
        BulletBehaviour bullet = (BulletBehaviour) target;
        
        serializedObject.Update();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("bulletType"), new GUIContent("Bullet Type"));

        if (bullet.bulletType == BulletBehaviour.BulletType.FollowTarget)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("turnTresholdAngle"), new GUIContent("Turn Treshold Angle"));
        }
        
        EditorGUILayout.PropertyField(serializedObject.FindProperty("maxTime"), new GUIContent("Life Time"));

        serializedObject.ApplyModifiedProperties();
    }
}
