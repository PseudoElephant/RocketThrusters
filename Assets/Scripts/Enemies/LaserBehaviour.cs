﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class LaserBehaviour : MonoBehaviour
{
    // Public To Be Worked From The Editor
    //[Header("Laser Emission")]
   [HideInInspector]
    public  bool useBursts;
    [HideInInspector]
    public bool startOn;
    [HideInInspector]
    public float timeActive;
    [HideInInspector]
    public float timeBetweenActivations;
    
    [Header("Laser FX's")]
    [SerializeField] Transform transformFirePoint;
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] float maxLength;
    [SerializeField] GameObject endVFX;
    [SerializeField] float endVFXOffset;
    
    [Header("Laser Interaction")]
    [SerializeField] private float laserHitStrength;
    
    // State
    private bool _isActive = true;
    // Start is called before the first frame update
    void Start()
    {
        if(useBursts)
        {
            StartCoroutine(LaserToggle());
        }
    }
    
    // Coroutines
    // COULD USE DELEGATES FOR FUNCTION CALLBACKS 
    IEnumerator LaserToggle()
    {
        
        // Disables Laser or not
        if (!startOn) {   DisableLaser(); }
        
        // While IsActive or Destroyed
        while (_isActive)
        {
            if (startOn)
            {
                // Time Between Activations
                yield return new WaitForSecondsRealtime(timeBetweenActivations);
                EnableLaser();
                // Time Active
                yield return new WaitForSecondsRealtime(timeActive);
                DisableLaser();
            }
            else
            {
                // Time Active
                yield return new WaitForSecondsRealtime(timeActive);
                DisableLaser();
                // Time Between Activations
                yield return new WaitForSecondsRealtime(timeBetweenActivations);
                EnableLaser();
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

            // Kill Player TODO:Maybe not as scalable
            if (hit.collider.CompareTag("Player"))
            {
                // Delete if rocket
                RocketMovement rocket = hit.collider.gameObject.GetComponentInParent<RocketMovement>();
                if (rocket)
                {
                    rocket.InvokeDeath(direction*laserHitStrength);
                }
                else
                {
                    Destroy(hit.collider.gameObject);
                }
            }
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