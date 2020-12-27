using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingBehaviour : MonoBehaviour
{
    public MovingBlockType MovingType;

    public Vector2 Frequency;
    public Vector2 Amplitude;
    
    private Vector3 posOffset;
    private Vector3 locOff;
    private Vector3 temp;

    public enum MovingBlockType
    {
        Waves,
        PerlinNoise
    }

    // Range over which height varies.
    public Vector2 PerlinHeightScale = new Vector2(1f, 1f);

    // Distance covered per second along X axis of Perlin plane.
    public Vector2 PerlinScale = new Vector2(1f, 0.5f);

    // Start is called before the first frame update
    void Start()
    {
        posOffset = transform.position;
        locOff = transform.localPosition;
    }
    
    // Update is called once per frame
    public void Update()
    {
        if (MovingType == MovingBlockType.PerlinNoise)
        {

            float height = PerlinHeightScale.y * Mathf.PerlinNoise(Time.time * PerlinScale.y, 0f);
            float width = PerlinHeightScale.x * Mathf.PerlinNoise(Time.time * PerlinScale.x + 100f, 0f);
            Vector3 pos = locOff;
            pos.y += height;
            pos.x += width;
            transform.localPosition = pos;
        }
        else
        {
            temp = posOffset;
            temp.y  += Mathf.Sin (Time.fixedTime * Mathf.PI * Frequency.y) * Amplitude.y;
            temp.x  += Mathf.Cos (Time.fixedTime * Mathf.PI * Frequency.x) * Amplitude.x;
            transform.position = temp;      
        }
    }
}
