using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(SpawnerBehaviour))]
public class SpawnerEditor : Editor
{
    private bool showSpawnRate = false;
    private bool showSpawnQuantities = false;

    private void OnSceneGUI()
    {
        SpawnerBehaviour spawner = (SpawnerBehaviour)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(spawner.transform.position, Vector3.forward, Vector3.up, 360, spawner.spawnRadius);
    }

    public override void OnInspectorGUI()
    {
        
        SpawnerBehaviour spawner = (SpawnerBehaviour) target;
            
        // Editor Parameters
        serializedObject.Update();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("prefabSpawnList"), true);
        serializedObject.ApplyModifiedProperties();

        spawner.selectRandomlyFromList = GUILayout.Toggle(spawner.selectRandomlyFromList, "Select Randomly");
        if (!spawner.selectRandomlyFromList)
        {
            spawner.specificSpawnIndex = EditorGUILayout.IntField("Spawn Index", spawner.specificSpawnIndex);
        }

        //Spawn Rates Section
        EditorGUILayout.Separator();

        showSpawnRate = EditorGUILayout.Foldout(showSpawnRate, "Spawn Rates:");

        if (showSpawnRate)
        {
            spawner.constantTimeBetweenGroupSpawns = GUILayout.Toggle(spawner.constantTimeBetweenGroupSpawns, "Constant Group Spawn Rate");

            if (spawner.constantTimeBetweenGroupSpawns)
            {
                spawner.timeBetweenGroupSpawns = EditorGUILayout.FloatField("Spawn Rate", spawner.timeBetweenGroupSpawns);
            }
            else
            {
                spawner.minTimeBetweenGroupSpawns = EditorGUILayout.FloatField("Min Spawn Rate", spawner.minTimeBetweenGroupSpawns);
                spawner.maxTimeBetweenGroupSpawns = EditorGUILayout.FloatField("Max Spawn Rate", spawner.maxTimeBetweenGroupSpawns);
            }

            EditorGUILayout.Separator();
           
            spawner.constantTimeBetweenIndividualSpawns = GUILayout.Toggle(spawner.constantTimeBetweenIndividualSpawns, "Constant Time Between Individual Spawns");

            if (spawner.constantTimeBetweenIndividualSpawns)
            {
                spawner.timeBetweenIndividualSpawns = EditorGUILayout.FloatField("Spawn Rate", spawner.timeBetweenIndividualSpawns);
            }
            else
            {
                spawner.minTimeBetweenIndividualSpawns = EditorGUILayout.FloatField("Min Spawn Rate", spawner.minTimeBetweenIndividualSpawns);
                spawner.maxTimeBetweenIndividualSpawns = EditorGUILayout.FloatField("Max Spawn Rate", spawner.maxTimeBetweenIndividualSpawns);
            }
        }

        EditorGUILayout.Separator();
        showSpawnQuantities = EditorGUILayout.Foldout(showSpawnQuantities, "Spawn Details:");
        if (showSpawnQuantities)
        {
            EditorGUILayout.LabelField("Spawn Quantity: ");
            spawner.minSpawnAmmount = EditorGUILayout.IntField("Min", spawner.minSpawnAmmount);
            spawner.maxSpawnAmmount = EditorGUILayout.IntField("Max", spawner.maxSpawnAmmount);

            EditorGUILayout.Separator();
            spawner.spawnRadius = EditorGUILayout.FloatField("Spawn Radius", spawner.spawnRadius);
        }

        // Redraw Scene
        UnityEditor.SceneView.lastActiveSceneView.Repaint();
    }
}