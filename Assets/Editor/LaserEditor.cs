using System;
using UnityEditor;
using UnityEngine;

// Todo:  Could Improve Code by adding helper methods in main class
[CustomEditor(typeof(LaserBehaviour))]
    public class LaserEditor : Editor
    {
        private void OnSceneGUI()
        {
            Handles.color = Color.white;
            LaserBehaviour laser = (LaserBehaviour) target;
            float rot = (laser.transform.rotation.eulerAngles.z + 90f) % 360 * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(Mathf.Cos(rot), Mathf.Sin(rot)) * laser.maxLength;
            Vector2 temp = laser.transformFirePoint.position;
            // Vector Addition to get the offset in world space
            Handles.DrawLine(temp,(Vector2)temp +  direction);
        }

        public override void OnInspectorGUI()
        {
          
            base.OnInspectorGUI();
            
            LaserBehaviour laser = (LaserBehaviour) target;
            // Setting Variables
            GUIStyle style = new GUIStyle();
            style.fontStyle = FontStyle.Bold;
            style.richText = true;
            GUILayout.Label("\n<color=white>Laser Emission</color>",style);
            laser.useBursts = EditorGUILayout.Toggle("useBursts",laser.useBursts);
            
            if (laser.useBursts)
            {
                laser.startOn = EditorGUILayout.Toggle("startOn", laser.startOn);
                laser.timeActive = EditorGUILayout.FloatField("timeActive", laser.timeActive);
                laser.timeBetweenActivations = EditorGUILayout.FloatField("timeBetweenActivations", laser.timeBetweenActivations);
                
            }
            
            // Update laser length
            laser.lineRenderer.SetPosition(1,new Vector3(0,laser.maxLength,0));
            float rot = (laser.transform.rotation.eulerAngles.z + 90f) % 360 * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(Mathf.Cos(rot), Mathf.Sin(rot));
            RaycastHit2D hit = Physics2D.Raycast(laser.transformFirePoint.position, direction, laser.maxLength);

            if (hit)
            {
                laser.lineRenderer.SetPosition(1, new Vector3(0, hit.distance, 0));
            }
        }
    }
