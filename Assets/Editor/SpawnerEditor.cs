using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(SpawnerBehaviour))]
    public class SpawnerEditor : Editor
    {
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
                spawner.specificSpawnIndex = EditorGUILayout.IntField("specificSpawnIndex", spawner.specificSpawnIndex);
            }
            
            spawner.constantTimeBetweenSpawns = GUILayout.Toggle(spawner.constantTimeBetweenSpawns, "Constant Spawn Rate");
            if (spawner.constantTimeBetweenSpawns)
            {
                spawner.timeBetweenSpawns = EditorGUILayout.FloatField("Spawn Rate",spawner.timeBetweenSpawns);
            }
            else
            {
                spawner.minTimeBetweenSpawns = EditorGUILayout.FloatField( "Min Spawn Rate",spawner.minTimeBetweenSpawns);
                spawner.maxTimeBetweenSpawns = EditorGUILayout.FloatField("Max Spawn Rate",spawner.maxTimeBetweenSpawns );
                
            }
            EditorGUILayout.Separator();
            spawner.minSpawnAmmount = EditorGUILayout.IntField("Min Spawn Amount",spawner.minSpawnAmmount);
            spawner.maxSpawnAmmount = EditorGUILayout.IntField("Max Spawn Amount",spawner.maxSpawnAmmount);
            
        }
    }