using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(ExplosiveBehaviour))]
public class ExplosiveEditor : Editor
{
    private bool showExplosionDetails = true;
    SerializedProperty particleSystem;
    SerializedProperty proximityTrigger;

    private void OnEnable()
    {
        particleSystem = serializedObject.FindProperty("ExplosionParticles");
        proximityTrigger = serializedObject.FindProperty("ProximityTrigger");
    }

    private void OnSceneGUI()
    {
        ExplosiveBehaviour explosive = (ExplosiveBehaviour)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(explosive.transform.position, Vector3.forward, Vector3.up, 360, explosive.ExplosionRadius);

        if(explosive.Type == ExplosiveBehaviour.ExplosionType.ProximityTriggered)
        {
            Handles.color = Color.blue;
            Handles.DrawWireArc(explosive.transform.position, Vector3.forward, Vector3.up, 360, explosive.ProximityActivationRadius);
        }
    }

    public override void OnInspectorGUI()
    {
        ExplosiveBehaviour explosive = (ExplosiveBehaviour)target;

        explosive.Type = (ExplosiveBehaviour.ExplosionType)EditorGUILayout.EnumPopup("Explosion Type", explosive.Type);

        if (explosive.Type == ExplosiveBehaviour.ExplosionType.ProximityTriggered)
        {
            EditorGUILayout.PropertyField(proximityTrigger, new GUIContent("Proximity Trigger"));
            explosive.ProximityActivationRadius = EditorGUILayout.FloatField("Activation Radius", explosive.ProximityActivationRadius);
        }

        EditorGUILayout.Separator();
        EditorGUILayout.PropertyField(particleSystem, new GUIContent("Explosion Particles"));

        EditorGUILayout.Separator();
        showExplosionDetails = EditorGUILayout.Foldout(showExplosionDetails, "Explosion Details");

        if (showExplosionDetails)
        { 
            explosive.FuseTime = EditorGUILayout.FloatField("Fuse Time", explosive.FuseTime);
            explosive.ExplosionDuration = EditorGUILayout.FloatField("Explosion Duration", explosive.ExplosionDuration);
            explosive.ExplosionRadius = EditorGUILayout.FloatField("Explosion Radius", explosive.ExplosionRadius);
            explosive.ExplosionStrength = EditorGUILayout.FloatField("Explosion Strength", explosive.ExplosionStrength);
        }
        
        // Redraw Scene
        UnityEditor.SceneView.lastActiveSceneView.Repaint();

        //Reapply Properties
        serializedObject.ApplyModifiedProperties();
    }
}
