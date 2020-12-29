using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class SpawnerBehaviour : MonoBehaviour
{
    public GameObject[] PrefabSpawnList;
    public float SpawnRadius;
    public bool SelectRandomlyFromList;
    public int SpecificSpawnIndex;
    public int MinSpawnAmmount;
    public int MaxSpawnAmmount;
    public bool ConstantTimeBetweenGroupSpawns;
    public float TimeBetweenGroupSpawns;
    public float MinTimeBetweenGroupSpawns;
    public float MaxTimeBetweenGroupSpawns;
    public bool ConstantTimeBetweenIndividualSpawns;
    public float TimeBetweenIndividualSpawns;
    public float MinTimeBetweenIndividualSpawns;
    public float MaxTimeBetweenIndividualSpawns;
   
    // State
    private bool _spawning = true;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Spawn());
    }
    
    IEnumerator Spawn()
    {
        while (_spawning)
        {
           
            // Time Between Spawns
            if (ConstantTimeBetweenGroupSpawns)
            {
                yield return new WaitForSecondsRealtime(TimeBetweenGroupSpawns);
            }
            else
            {
                yield return new WaitForSecondsRealtime(Random.Range(MinTimeBetweenGroupSpawns,MaxTimeBetweenGroupSpawns));
            }

            // Spawning
            int spawnAmount = Random.Range(MinSpawnAmmount, MaxSpawnAmmount + 1); //To account for exclusiveness;
            int index = SpecificSpawnIndex;
            for (int i = 0; i < spawnAmount; i++)
            {
                if (SelectRandomlyFromList) {
                    index = Random.Range(0, PrefabSpawnList.Length);
                }

                Vector2 randomPoint = Random.insideUnitCircle;
                Vector3 position = new Vector3(randomPoint.x, randomPoint.y, 0) * SpawnRadius;

                Instantiate(PrefabSpawnList[index], transform.position+position, Quaternion.Euler(Vector3.forward * Random.Range(0,360)));

                if (ConstantTimeBetweenIndividualSpawns)
                {
                    yield return new WaitForSecondsRealtime(TimeBetweenIndividualSpawns);
                }
                else
                {
                    yield return new WaitForSecondsRealtime(Random.Range(MinTimeBetweenIndividualSpawns, MaxTimeBetweenIndividualSpawns));
                }
            }
        }
    }
    
    // Helper Methods

    public void EnableSpawner()
    {
        StopCoroutine(Spawn());
        _spawning = true;
        StartCoroutine(Spawn());
    }
    public void DisableSpawner()
    {
        _spawning = false;
    }   
    public void ToggleSpawner()
    {
        if (_spawning)
            DisableSpawner();
        else
            EnableSpawner();
    }
}
