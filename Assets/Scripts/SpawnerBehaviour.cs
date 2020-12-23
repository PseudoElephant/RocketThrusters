using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class SpawnerBehaviour : MonoBehaviour
{
   [SerializeField]
    GameObject[] prefabSpawnList;
    [HideInInspector]
    public bool selectRandomlyFromList;
    [HideInInspector]
    public int specificSpawnIndex;
    [HideInInspector]
    public int minSpawnAmmount;
    [HideInInspector]
    public int maxSpawnAmmount;
    [HideInInspector]
    public bool constantTimeBetweenSpawns;
    [HideInInspector]
    public float timeBetweenSpawns;
    [HideInInspector]
    public float minTimeBetweenSpawns;
    [HideInInspector]
    public float maxTimeBetweenSpawns;
 
    
    // State
    [HideInInspector]
    public bool spawning = true;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        while (spawning)
        {
           
// Time Between Spawns
            if (constantTimeBetweenSpawns)
            {
                yield return new WaitForSecondsRealtime(timeBetweenSpawns);
            }
            else
            {
                yield return new WaitForSecondsRealtime(Random.Range(minTimeBetweenSpawns,maxTimeBetweenSpawns));
            }
// Spawning
            int spawnAmount = Random.Range(minSpawnAmmount, maxSpawnAmmount);
            int index = specificSpawnIndex;
            for (int i = 0; i < spawnAmount; i++)
            {
                if (selectRandomlyFromList){index = Random.Range(0, prefabSpawnList.Length); }

                Instantiate(prefabSpawnList[index],transform.position,Quaternion.identity);
            }

        }
    }
}
