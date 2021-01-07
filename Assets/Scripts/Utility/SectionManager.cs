using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class SectionManager : MonoBehaviour
{
    public CinemachineVirtualCamera defaultCamera;
    public RocketMovement player;
    public LevelLoader loader;
    public static SectionManager instance;

    private void Awake()
    {
        // Singleton pattern
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        loader.gameObject.SetActive(true);
    }


    public void SetTarget(ref GameObject target)
    {
        if (target == null)
        {
            target = player.gameObject;
        }
    }
}
