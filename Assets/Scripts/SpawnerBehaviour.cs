using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class SpawnerBehaviour : MonoBehaviour
{
    [SerializeField]
    GameObject[] prefabSpawnList;
    [HideInInspector]
    public float spawnRadius;
    [HideInInspector]
    public bool selectRandomlyFromList;
    [HideInInspector]
    public int specificSpawnIndex;
    [HideInInspector]
    public int minSpawnAmmount;
    [HideInInspector]
    public int maxSpawnAmmount;
    [HideInInspector]
    public bool constantTimeBetweenGroupSpawns;
    [HideInInspector]
    public float timeBetweenGroupSpawns;
    [HideInInspector]
    public float minTimeBetweenGroupSpawns;
    [HideInInspector]
    public float maxTimeBetweenGroupSpawns;
    [HideInInspector]
    public bool constantTimeBetweenIndividualSpawns;
    [HideInInspector]
    public float timeBetweenIndividualSpawns;
    [HideInInspector]
    public float minTimeBetweenIndividualSpawns;
    [HideInInspector]
    public float maxTimeBetweenIndividualSpawns;
   
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
            if (constantTimeBetweenGroupSpawns)
            {
                yield return new WaitForSecondsRealtime(timeBetweenGroupSpawns);
            }
            else
            {
                yield return new WaitForSecondsRealtime(Random.Range(minTimeBetweenGroupSpawns,maxTimeBetweenGroupSpawns));
            }

            // Spawning
            int spawnAmount = Random.Range(minSpawnAmmount, maxSpawnAmmount + 1); //To account for exclusiveness;
            int index = specificSpawnIndex;
            for (int i = 0; i < spawnAmount; i++)
            {
                if (selectRandomlyFromList) {
                    index = Random.Range(0, prefabSpawnList.Length);
                }

                Vector2 randomPoint = Random.insideUnitCircle;
                Vector3 position = new Vector3(randomPoint.x, randomPoint.y, 0) * spawnRadius;

                Instantiate(prefabSpawnList[index], transform.position+position, Quaternion.Euler(Vector3.forward * Random.Range(0,360)));

                if (constantTimeBetweenIndividualSpawns)
                {
                    yield return new WaitForSecondsRealtime(timeBetweenIndividualSpawns);
                }
                else
                {
                    yield return new WaitForSecondsRealtime(Random.Range(minTimeBetweenIndividualSpawns, maxTimeBetweenIndividualSpawns));
                }
            }
        }
    }
}
