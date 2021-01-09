using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LaserBehaviour))]
public class LaserEditor : Editor
{
    private bool _showBurstSettings = true;
    private bool _showLaserSettings = true;
    private bool _showVFXSettings = true;

    private void OnSceneGUI()
    {
        Handles.color = Color.white;
        LaserBehaviour laser = (LaserBehaviour) target;
        float rot = (laser.transform.rotation.eulerAngles.z + 90f) % 360 * Mathf.Deg2Rad;
        Vector2 direction = new Vector2(Mathf.Cos(rot), Mathf.Sin(rot)) * laser.MaxLength;
        Vector2 temp = laser.TransformFirePoint.position;

        // Vector Addition to get the offset in world space
        Handles.DrawLine(temp,(Vector2)temp +  direction);
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        //Get Newest Version
       serializedObject.Update();

        LaserBehaviour laser = (LaserBehaviour) target;

        //Creating Style
        GUIStyle style = new GUIStyle();
        style.fontStyle = FontStyle.Bold;
        //style.richText = true;

        //Set Up
        EditorGUILayout.PropertyField(serializedObject.FindProperty("EmissionType"));

        

        if (laser.EmissionType == LaserBehaviour.LaserEmission.BurstEmission)
        {
            _showBurstSettings = EditorGUILayout.Foldout(_showBurstSettings, "Burst Settings");
            if (_showBurstSettings)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("StartOn"), new GUIContent("Start Activated"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("TimeActive"), new GUIContent("Time Active"));
                EditorGUILayout.LabelField("Time Between Activations:");
                EditorGUILayout.PropertyField(serializedObject.FindProperty("TimeBetweenActivations"), new GUIContent(""));
                EditorGUILayout.Separator();
            }
        }


        _showLaserSettings = EditorGUILayout.Foldout(_showLaserSettings, "Laser Settings");

        if(_showLaserSettings)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("MaxLength"), new GUIContent("Max Length"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("LaserHitStrength"), new GUIContent("Strength"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("layerMask"), new GUIContent("Mask"));
            EditorGUILayout.Separator();
        }


        _showVFXSettings = EditorGUILayout.Foldout(_showVFXSettings, "VFX's");

        if (_showVFXSettings)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("TransformFirePoint"), new GUIContent("Fire Point"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("LineRenderer"), new GUIContent("Line Renderer"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("EndVFX"), new GUIContent("End VFX"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("EndVFXOffset"), new GUIContent("End VFX Offset"));
            EditorGUILayout.Separator();
        }

        // Update laser length
        UpdateLaser(laser);

       serializedObject.ApplyModifiedProperties();
    }

    private void UpdateLaser(LaserBehaviour laser)
    {
        if (laser.LineRenderer == null) return;
        
        laser.LineRenderer.SetPosition(1, new Vector3(0, laser.MaxLength, 0));
        float rot = (laser.transform.rotation.eulerAngles.z + 90f) % 360 * Mathf.Deg2Rad;
        Vector2 direction = new Vector2(Mathf.Cos(rot), Mathf.Sin(rot));
        RaycastHit2D hit = Physics2D.Raycast(laser.TransformFirePoint.position, direction, laser.MaxLength,laser.layerMask);

        if(hit && !hit.collider.isTrigger)
        {
            laser.LineRenderer.SetPosition(1, new Vector3(0, hit.distance, 0));
        }
    }
}
