using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangedByCable : MonoBehaviour
{
    public LineRenderer lineRenderer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = lineRenderer.GetPosition(lineRenderer.positionCount-1);
    }
}
