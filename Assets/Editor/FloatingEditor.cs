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
                floating.xAmplitud = 1;
                floating.yAmplitud = 1;
                floating.yFrequency = 1;
                floating.xFrequency = 1;
            }
            base.OnInspectorGUI();
            
        }
    }
