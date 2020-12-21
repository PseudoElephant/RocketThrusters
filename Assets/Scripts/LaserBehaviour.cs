using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBehaviour : MonoBehaviour
{
    [SerializeField] bool alwaysActive;
    [SerializeField] bool startOn;
    [SerializeField] float timeActive;
    [SerializeField] float timeBetweenActivations;
    [SerializeField] Transform transformFirePoint;
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] float maxLength;
    [SerializeField] GameObject endVFX;
    [SerializeField] float endVFXOffset;
    
    // Start is called before the first frame update
    void Start()
    {
        if(!alwaysActive)
        {
            if (startOn)
            {
                InvokeRepeating("DisableLaser", timeBetweenActivations, timeBetweenActivations * 2);
                InvokeRepeating("EnableLaser", timeBetweenActivations + timeActive, timeBetweenActivations * 2);
            }
            else
            {
                DisableLaser();
                InvokeRepeating("EnableLaser", timeBetweenActivations, timeBetweenActivations * 2);
                InvokeRepeating("DisableLaser", timeBetweenActivations + timeActive, timeBetweenActivations * 2);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!lineRenderer.enabled)
        {
            return;
        }

        UpdateLaser();
    }

    void UpdateLaser()
    {
        lineRenderer.SetPosition(0, transformFirePoint.localPosition);
        
        float rot = (transform.rotation.eulerAngles.z + 90f) % 360 * Mathf.Deg2Rad;
        Vector2 direction = new Vector2(Mathf.Cos(rot), Mathf.Sin(rot));

        RaycastHit2D hit = Physics2D.Raycast(transformFirePoint.position, direction, maxLength);

        if(hit)
        {
            endVFX.transform.localPosition = new Vector3(0, hit.distance - endVFXOffset, 0);
            lineRenderer.SetPosition(1, new Vector3(0,hit.distance,0));
        } else
        {
            endVFX.transform.localPosition = new Vector3(0, maxLength - endVFXOffset, 0);
            lineRenderer.SetPosition(1, new Vector3(0, maxLength, 0));
        }
    }

    void EnableLaser()
    {
        lineRenderer.enabled = true;

        //Turn On Particles
        ParticleSystem[] particleSystems = GetComponentsInChildren<ParticleSystem>();
        for (int i = 0; i < particleSystems.Length; i++)
        {
            particleSystems[i].Play();
        }
    }

    void DisableLaser()
    {
        lineRenderer.enabled = false;

        //Turn Off All Particles
        ParticleSystem[] particleSystems = GetComponentsInChildren<ParticleSystem>();
        for (int i = 0; i < particleSystems.Length; i++)
        {
            particleSystems[i].Stop();
        }
    }
}
