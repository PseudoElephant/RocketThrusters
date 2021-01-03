using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestructBehaviour : MonoBehaviour
{
    public bool useTime;
    public float selfDestructTime;
    // Start is called before the first frame update
    void Start()
    {
        if (useTime)
        {
            StartCoroutine(DestroySelf());
        }
    }

    IEnumerator DestroySelf()
    {
        yield return new WaitForSecondsRealtime(selfDestructTime);
        Destroy(gameObject);
    }

    public void SelfDestroy()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        SelfDestroy();
    }
}
