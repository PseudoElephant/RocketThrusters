using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingBehaviour : MonoBehaviour
{
    [HideInInspector, SerializeField]
    public Vector2 Frequency;
    [HideInInspector, SerializeField]
    public Vector2 Amplitude;

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
        temp.y  += Mathf.Sin (Time.fixedTime * Mathf.PI * Frequency.y) * Amplitude.y;
        temp.x  += Mathf.Cos (Time.fixedTime * Mathf.PI * Frequency.x) * Amplitude.x;
        transform.position = temp;
    }

}
