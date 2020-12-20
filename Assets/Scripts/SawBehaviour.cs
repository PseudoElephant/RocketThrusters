using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawBehaviour : MonoBehaviour
{
    public bool rotateRight;

    [Range(0f, 10f)]
    public float rotationSpeed;

    private bool prevRotateRight;
    private float multiplier = 10;

    // Start is called before the first frame update
    void Start()
    {
        prevRotateRight = rotateRight;

        if (rotateRight)
        {
            multiplier *= -1;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (rotateRight != prevRotateRight)
        {
            prevRotateRight = rotateRight;
            multiplier *= -1;
        }

        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime * multiplier, Space.Self);
    }
}
