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
    private Vector3 locOff;
    private Vector3 temp;
    
    // "Bobbing" animation from 1D Perlin noise.
    public bool perlinNoise = true;
    // Range over which height varies.
    public float heightScale = 1.0f;
    
    public float widthScale = 1.0f;

    // Distance covered per second along X axis of Perlin plane.
    public float xScale = 1.0f;

    public float yScale = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        posOffset = transform.position;
        locOff = transform.localPosition;
    }
    

    // Update is called once per frame
    public void Update()
    {
        if (perlinNoise)
        {

            float height = heightScale * Mathf.PerlinNoise(Time.time * yScale, 0f);
            float width = widthScale * Mathf.PerlinNoise(Time.time * xScale + 100f, 0f);
            Vector3 pos = locOff;
            pos.y += height;
            pos.x += width;
            transform.localPosition = pos;
        }

        else
        {
            temp = posOffset;
            temp.y  += Mathf.Sin (Time.fixedTime * Mathf.PI * yFrequency) * yAmplitud;
            temp.x  += Mathf.Cos (Time.fixedTime * Mathf.PI * xFrequency) * xAmplitud;
            transform.position = temp;      
        }
    }

}
