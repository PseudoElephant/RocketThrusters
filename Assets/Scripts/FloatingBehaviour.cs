using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingBehaviour : MonoBehaviour
{
   public  float xFrequency;
   public  float yFrequency;
   public float xAmplitud;
   public float yAmplitud;
    
    private Vector3 posOffset;
    private Vector3 temp;
    // Start is called before the first frame update
    void Start()
    {
        posOffset = transform.position;
    }
    

    // Update is called once per frame
    public void Update()
    {
        temp = posOffset;
        temp.y  += Mathf.Sin (Time.fixedTime * Mathf.PI * yFrequency) * yAmplitud;
        temp.x  += Mathf.Cos (Time.fixedTime * Mathf.PI * xFrequency) * xAmplitud;
        transform.position = temp;


    }

}
