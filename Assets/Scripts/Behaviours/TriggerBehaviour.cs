using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerBehaviour : MonoBehaviour
{
    // Parameters
    [SerializeField] protected UnityEvent triggerEvent;
    public TriggerType type;
    public float time;
    public bool singleUse;
    public LayerMask layerMask;
    public bool multipleContacts;

    
    // Cache
    private Collider2D _collider;
    private List<Collider2D> _colliderCache = new List<Collider2D>();
    
    public virtual void Start()
    {
        _collider = GetComponent<Collider2D>(); 
        
    }
    
  

    // Extra
    public enum TriggerType
    {
        InstantTrigger,
        DelayedTrigger,
        OnStayTrigger
    }

    private bool _activated = false;
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_activated || collision.isTrigger || !_collider.IsTouchingLayers(layerMask)) { return; }
        _activated = true;
        switch (type)
        {
            case TriggerType.InstantTrigger:
                InstantTriggerAction(collision);
                break;
            case TriggerType.DelayedTrigger:
                DelayedTriggerAction(collision);
                break;
            case TriggerType.OnStayTrigger:
                OnStayTriggerAction(collision);
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.isTrigger) { return; }
        // Disable Trigger
        Collider2D[] colliders = other.GetComponents<Collider2D>();
        DisableTriggerStoreCache(colliders);
        
        switch (type)
        {
            case TriggerType.OnStayTrigger:
                if (!_collider.IsTouchingLayers(layerMask))
                    StopAllCoroutines();
                break;
        }
        
        // Check if trigger can be deactivated
        CanRunAgain();
        
        // Enable Trigger
        EnableTriggerClearCache();
    }

    // Helper Methods
    private void EnableTriggerClearCache()
    {
        foreach (var coll in _colliderCache)
        {
            coll.enabled = true;
        }

        // Clear Cache
        _colliderCache.Clear();
    }

    private void DisableTriggerStoreCache(Collider2D[] colliders)
    {
        foreach (Collider2D coll in colliders)
        {
            if (coll.isTrigger && coll.enabled)
            {
                coll.enabled = false;
                _colliderCache.Add(coll);
            }
        }
    }

    // Methods
    void InstantTriggerAction(Collider2D collision)
    {
        triggerEvent.Invoke();
        // Check if we can deactivate trigger
        CanRunAgain();
    }

    void DelayedTriggerAction(Collider2D collision)
    {
        StartCoroutine(OnStayTriggerRoutine());
    }

    void OnStayTriggerAction(Collider2D collision)
    {
        StartCoroutine(OnStayTriggerRoutine());
    }

    IEnumerator OnStayTriggerRoutine()
    {
        
        yield return new WaitForSecondsRealtime(time);
        print("Exit Trigger");
        triggerEvent.Invoke();
        // Check if we can deactivate trigger
        CanRunAgain();
        yield return null;
    }

    void CanRunAgain()
    {
        // If trigger is active and can handle multiple touches
        if (!singleUse)
        {
            if (multipleContacts)
            {
                _activated = false;
            }else if (_collider.IsTouchingLayers(layerMask))
            {
                _activated = true;
            }
            else
            {
                _activated = false;
            }
           
        }
        
    }
    
}
