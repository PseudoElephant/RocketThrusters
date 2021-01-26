using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class CheckpointBehaviour : MonoBehaviour
{
    public LayerMask detectMask;
    public float maxRadiusActive = 20f;
    
    private bool active = false;
    public Transform spawnPosition;
    private const int MAXLength = 60; 
    // Start is called before the first frame update
    void Start()
    {
        if (GameSession.Instance.Data?.playerPosition[0] == MathUtility.VectorToFloatArray(spawnPosition.position)[0] && GameSession.Instance.Data?.playerPosition[1] == MathUtility.VectorToFloatArray(spawnPosition.position)[1])
        {
            active = true;
            //LoadCheckPoint();
        }
    }

    
    // Tune for player to attach
    public void LoadCheckPoint()
    {
        if (!active) return;
        SectionManager.instance.player.transform.position = spawnPosition.position;
    }

    public bool RaycastToPlayer()
    {
        RaycastHit2D hit;
        Vector2 pos = transform.position;
        hit = Physics2D.Raycast(pos, Vector3.up, MAXLength,  detectMask);
        

        if (hit)
        {
            return true;
        }
        
        return false;

    }

    public bool PlayerInRange()
    {
        return (transform.position - SectionManager.instance.player.transform.position).magnitude <
               maxRadiusActive;
    }
    private void Update()
    {
        if (!active && PlayerInRange())
        {
            if (RaycastToPlayer())
            {
                // Saving Data
                GameSession.Instance.SaveData(spawnPosition.position , 0);
                active = true;
            }
        }
    }

  
    
    
}
