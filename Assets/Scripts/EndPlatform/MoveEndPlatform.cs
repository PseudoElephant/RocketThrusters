using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEndPlatform : MonoBehaviour
{
    [SerializeField] private GameObject lightPrefab;

    [SerializeField] private int numberOfLights;
    [SerializeField] private float minHeight;
    [SerializeField] private float maxHeight;
    [SerializeField] private Transform startPoint;
    [SerializeField] private float minSpeed;
    [SerializeField] private float maxSpeed;

    public Transform StartPoint => startPoint;

    public Transform EndPoint => endPoint;

    [SerializeField] private Transform endPoint;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < numberOfLights; i++)
        {
            GameObject go = Instantiate(lightPrefab, transform);
            // Random Height
            float height = Random.Range(minHeight,maxHeight);
            go.transform.localScale =new Vector3(1,height,0);
            // Random Position
            float value = Random.Range(startPoint.transform.localPosition.x, endPoint.transform.localPosition.x);
   
            go.transform.localPosition =
                new Vector3(value, startPoint.localPosition.y + height / 2, transform.localPosition.z);

            // Set Bounds and Speed
            MoveEndPlatformIndividual mp = go.GetComponent<MoveEndPlatformIndividual>();
            mp.StartBound = startPoint;
            mp.EndBound = endPoint;
            float val = Random.Range(minSpeed, maxSpeed);
            mp.Speed = val != 0 ? val : 1f ;


        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
