using UnityEditor;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LaserBehaviour))]
    public class LaserEditor : Editor
    {
        public override void OnInspectorGUI()
        {
          
            base.OnInspectorGUI();
            
            LaserBehaviour laser = (LaserBehaviour) target;
            // Setting Variables
            GUIStyle style = new GUIStyle();
            style.fontStyle = FontStyle.Bold;
            style.richText = true;
            GUILayout.Label("\n<color=white>Laser Emission</color>",style);
            laser.alwaysActive = EditorGUILayout.Toggle("alwaysActive",laser.alwaysActive);

            if (laser.alwaysActive)
            {
                laser.startOn = EditorGUILayout.Toggle("startOn", laser.startOn);
                laser.timeActive = EditorGUILayout.FloatField("timeActive", laser.timeActive);
                laser.timeBetweenActivations = EditorGUILayout.FloatField("timeBetweenActivations", laser.timeBetweenActivations);
                
            }
            
        }
    }
