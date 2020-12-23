using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerBehaviour : MonoBehaviour
{
    [SerializeField] GameObject[] prefabSpawnList;
    [SerializeField] bool selectRandomlyFromList;
    [SerializeField] int specificSpawnIndex;
    [SerializeField] int minSpawnAmmount;
    [SerializeField] int maxSpawnAmmount;
    [SerializeField] bool constantTimeBetweenSpawns;
    [SerializeField] double timeBetweenSpawns;
    [SerializeField] double minTimeBetweenSpawns;
    [SerializeField] double maxTimeBetweenSpawns;
 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
