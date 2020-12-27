using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(SpawnerBehaviour))]
public class SpawnerEditor : Editor
{
    private bool _showSpawnRate = true;
    private bool _showSpawnQuantities = true;
    private bool _showList = true;

    private void OnSceneGUI()
    {
        SpawnerBehaviour spawner = (SpawnerBehaviour)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(spawner.transform.position, Vector3.forward, Vector3.up, 360, spawner.SpawnRadius);
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        SpawnerBehaviour spawner = (SpawnerBehaviour) target;

        // Editor Parameters
        _showList = EditorGUILayout.Foldout(_showList, "Spawn List Settings");

        if (_showList)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("PrefabSpawnList"), true);

            EditorGUILayout.PropertyField(serializedObject.FindProperty("SelectRandomlyFromList"), new GUIContent("Select Randomly"));

            if (!spawner.SelectRandomlyFromList)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("SpecificSpawnIndex"), new GUIContent("Spawn Index"));
            }

            EditorGUILayout.Separator();
        }

        //Spawn Rates Section
        _showSpawnRate = EditorGUILayout.Foldout(_showSpawnRate, "Spawn Rates");

        if (_showSpawnRate)
        {
            EditorGUILayout.LabelField("Group Spawn Rate:");
            EditorGUILayout.PropertyField(serializedObject.FindProperty("ConstantTimeBetweenGroupSpawns"), new GUIContent("Constant Rate"));

            if (spawner.ConstantTimeBetweenGroupSpawns)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("TimeBetweenGroupSpawns"), new GUIContent("Spawn Rate"));
            }
            else
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("MinTimeBetweenGroupSpawns"), new GUIContent("Min Spawn Rate"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("MaxTimeBetweenGroupSpawns"), new GUIContent("Max Spawn Rate"));
            }

            EditorGUILayout.Separator();

            EditorGUILayout.LabelField("Individual Spawn Rate:");
            EditorGUILayout.PropertyField(serializedObject.FindProperty("ConstantTimeBetweenIndividualSpawns"), new GUIContent("Constant Rate"));

            if (spawner.ConstantTimeBetweenIndividualSpawns)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("TimeBetweenIndividualSpawns"), new GUIContent("Spawn Rate"));
            }
            else
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("MinTimeBetweenIndividualSpawns"), new GUIContent("Min Spawn Rate"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("MaxTimeBetweenIndividualSpawns"), new GUIContent("Max Spawn Rate"));
            }

            EditorGUILayout.Separator();
        }

        _showSpawnQuantities = EditorGUILayout.Foldout(_showSpawnQuantities, "Spawn Details");
        if (_showSpawnQuantities)
        {
            EditorGUILayout.LabelField("Spawn Quantity: ");
            
            EditorGUILayout.PropertyField(serializedObject.FindProperty("MinSpawnAmmount"), new GUIContent("Min"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("MaxSpawnAmmount"), new GUIContent("Max"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("SpawnRadius"), new GUIContent("Spawn Radius"));

            EditorGUILayout.Separator();
        }

        // Redraw Scene
        UnityEditor.SceneView.lastActiveSceneView.Repaint();

        //Update Properties
        serializedObject.ApplyModifiedProperties();
    }
}