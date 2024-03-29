﻿using System;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(ExplosiveBehaviour))]
public class ExplosiveEditor : Editor
{
    private bool _showExplosionDetails = true;
    private SerializedProperty _particleSystem;

    private void OnEnable()
    {
        _particleSystem = serializedObject.FindProperty("ExplosionParticles");
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
            SpriteRenderer sp = explosive.GetComponentsInChildren<SpriteRenderer>()[1];
            if (sp.sprite.rect.width*explosive.gameObject.transform.localScale.x != explosive.ProximityActivationRadius*2);
                UpdateRadialView(sp,explosive.ProximityActivationRadius);
        }
    }

    //public override void OnInspectorGUI()
    //{
    //    serializedObject.Update();

    //    ExplosiveBehaviour explosive = (ExplosiveBehaviour)target;

    //    explosive.Type = (ExplosiveBehaviour.ExplosionType)EditorGUILayout.EnumPopup("Explosion Type", explosive.Type);

    //    if (explosive.Type == ExplosiveBehaviour.ExplosionType.ProximityTriggered)
    //    {
    //        explosive.ProximityActivationRadius = EditorGUILayout.FloatField("Activation Radius", explosive.ProximityActivationRadius);
    //    }

    //    EditorGUILayout.Separator();
    //    EditorGUILayout.PropertyField(particleSystem, new GUIContent("Explosion Particles"));

    //    EditorGUILayout.Separator();
    //    showExplosionDetails = EditorGUILayout.Foldout(showExplosionDetails, "Explosion Details");

    //    if (showExplosionDetails)
    //    { 
    //        explosive.FuseTime = EditorGUILayout.FloatField("Fuse Time", explosive.FuseTime);
    //        explosive.ExplosionDuration = EditorGUILayout.FloatField("Explosion Duration", explosive.ExplosionDuration);
    //        explosive.ExplosionRadius = EditorGUILayout.FloatField("Explosion Radius", explosive.ExplosionRadius);
    //        explosive.ExplosionStrength = EditorGUILayout.FloatField("Explosion Strength", explosive.ExplosionStrength);
    //    }

    //    // Redraw Scene
    //    UnityEditor.SceneView.lastActiveSceneView.Repaint();

    //    //Reapply Properties
    //    serializedObject.ApplyModifiedProperties();
    //}

    public override void OnInspectorGUI()
    {
        //Update to make sure latest version
        serializedObject.Update();
        ExplosiveBehaviour explosive = (ExplosiveBehaviour)target;

        EditorGUILayout.PropertyField(serializedObject.FindProperty("Type"), new GUIContent("Explosion Type"));

        if (explosive.Type == ExplosiveBehaviour.ExplosionType.ProximityTriggered)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("ProximityActivationRadius"), new GUIContent("Activation Radius"));
        }

        EditorGUILayout.Separator();
        EditorGUILayout.PropertyField(_particleSystem, new GUIContent("Explosion Particles"));

        EditorGUILayout.Separator();
        _showExplosionDetails = EditorGUILayout.Foldout(_showExplosionDetails, "Explosion Details");

        if (_showExplosionDetails)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("FuseTime"), new GUIContent("Fuse Time"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("ExplosionDuration"), new GUIContent("Explosion Duration"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("ExplosionRadius"), new GUIContent("Explosion Radius"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("ExplosionStrength"), new GUIContent("Explosion Strength"));
        }

        // Redraw Scene
        UnityEditor.SceneView.lastActiveSceneView.Repaint();

        //Apply All Changes
        serializedObject.ApplyModifiedProperties();
    }
    
    private void UpdateRadialView(SpriteRenderer radialView,float proximityActivationRadius)
    {
        Bounds b = radialView.sprite.bounds;
        radialView.gameObject.transform.localScale /= ((b.max - b.min).x / (proximityActivationRadius*2))*radialView.gameObject.transform.localScale.x;
        
    }
}
