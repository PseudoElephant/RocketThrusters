using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class DollyCameraBehaviour : MonoBehaviour
{
    public Rigidbody2D rocket;
    public float offsetFactor;
    public float rocketTresholdSpeed;
    
    private CinemachineVirtualCamera _dollyTrackPath;
    private CinemachineSmoothPath _path;
    
    // Update is called once per frame
    private void Start()
    {
        _dollyTrackPath = GetComponent<CinemachineVirtualCamera>();
    }

    void FixedUpdate()
    {
        if (rocket.velocity.magnitude > rocketTresholdSpeed)
        {
            
        }
    }
}
