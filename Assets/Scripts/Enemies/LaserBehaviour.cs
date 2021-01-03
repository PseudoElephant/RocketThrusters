using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class LaserBehaviour : MonoBehaviour
{
    // Public To Be Worked From The Editor
    //[Header("Laser Emission")]
    public bool StartOn;
    public float TimeActive;
    public float TimeBetweenActivations;
    
    public Transform TransformFirePoint;
    public LineRenderer LineRenderer;
    public float MaxLength;
    public GameObject EndVFX;
    public float EndVFXOffset;
    
    public float LaserHitStrength;
    public LaserEmission EmissionType;

    public enum LaserEmission
    {
        ConstantEmission,
        BurstEmission
    }
    
    // State
    private bool _isActive = true;

    // If it is working behaviour
    private bool _isActiveLaser = true;

    // Start is called before the first frame update
    void Start()
    {
        if(EmissionType == LaserEmission.BurstEmission)
        {
            StartCoroutine(LaserToggle());
        }
    }
    
    // Coroutines
    // COULD USE DELEGATES FOR FUNCTION CALLBACKS 
    IEnumerator LaserToggle()
    {
        
        // Disables Laser or not
        if (!StartOn) {   DisableLaser(); }
        
        // While IsActive or Destroyed
        while (_isActive)
        {
            if (StartOn)
            {
                // Time Between Activations
                yield return new WaitForSecondsRealtime(TimeBetweenActivations);
                EnableLaser();
                // Time Active
                yield return new WaitForSecondsRealtime(TimeActive);
                DisableLaser();
            }
            else
            {
                // Time Active
                yield return new WaitForSecondsRealtime(TimeActive);
                DisableLaser();
                // Time Between Activations
                yield return new WaitForSecondsRealtime(TimeBetweenActivations);
                EnableLaser();
            }

            
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!LineRenderer.enabled)
        {
            return;
        }

        UpdateLaser();
    }

    void UpdateLaser()
    {
        LineRenderer.SetPosition(0, TransformFirePoint.localPosition);
        
        float rot = (transform.rotation.eulerAngles.z + 90f) % 360 * Mathf.Deg2Rad;
        Vector2 direction = new Vector2(Mathf.Cos(rot), Mathf.Sin(rot));

        RaycastHit2D hit = Physics2D.Raycast(TransformFirePoint.position, direction, MaxLength);

        if(hit)
        {
            EndVFX.transform.localPosition = new Vector3(0, hit.distance - EndVFXOffset, 0);
            LineRenderer.SetPosition(1, new Vector3(0,hit.distance,0));
            
            if (hit.collider.CompareTag("Player"))
            {
                // Delete if rocket
                RocketMovement rocket = hit.collider.gameObject.GetComponentInParent<RocketMovement>();
                if (rocket)
                {
                    rocket.InvokeDeath(direction*LaserHitStrength);
                }
                else
                {
                    Destroy(hit.collider.gameObject);
                }
            }
        } else
        {
            EndVFX.transform.localPosition = new Vector3(0, MaxLength - EndVFXOffset, 0);
            LineRenderer.SetPosition(1, new Vector3(0, MaxLength, 0));
        }
    }

    void EnableLaser()
    {
        LineRenderer.enabled = true;

        //Turn On Particles
        ParticleSystem[] particleSystems = GetComponentsInChildren<ParticleSystem>();
        for (int i = 0; i < particleSystems.Length; i++)
        {
            particleSystems[i].Play();
        }
    }

    void DisableLaser()
    {
        LineRenderer.enabled = false;

        //Turn Off All Particles
        ParticleSystem[] particleSystems = GetComponentsInChildren<ParticleSystem>();
        for (int i = 0; i < particleSystems.Length; i++)
        {
            particleSystems[i].Stop();
        }
    }
    
    // Public Methods

    public void StopLaser()
    {
        if (_isActiveLaser)
        {
            if (EmissionType == LaserEmission.BurstEmission)
            {
                StopAllCoroutines();
            }
            DisableLaser();
            _isActiveLaser = false;
        }

    }

    public void StartLaser()
    {
        if (!_isActiveLaser)
        {
            if (EmissionType == LaserEmission.BurstEmission)
            {
                StartCoroutine(LaserToggle());
            }
            else
            {
                EnableLaser();
            }

            _isActive = true;
        }
    }

    public void ToggleLaser()
    {
        if (_isActiveLaser)
        {
           StopLaser();
        }
        else
        {
            StartLaser();
        }
    }
}
