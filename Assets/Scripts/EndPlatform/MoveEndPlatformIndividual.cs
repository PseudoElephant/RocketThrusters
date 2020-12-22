using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEndPlatformIndividual : MonoBehaviour
{
    //State
    private Transform _startBound;

    public Transform StartBound
    {
        get => _startBound;
        set => _startBound = value;
    }

    public Transform EndBound
    {
        get => _endBound;
        set => _endBound = value;
    }

    private Transform _endBound;

    public float speed = 0.5f;

    public float Speed
    {
        get => speed;
        set => speed = value;
    }

 
    // Update is called once per frame
    void FixedUpdate()
    {
        if ((transform.localPosition.x + speed*Time.deltaTime) > _endBound.localPosition.x || (transform.localPosition.x + speed * Time.deltaTime) < _startBound.localPosition.x)
        {
            speed *= -1;
        }
        transform.Translate(speed*Time.deltaTime,0,0);
    }
}
