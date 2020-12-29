using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    // Parameters
    [SerializeField] private bool followTarget;
    [SerializeField] private float maxTime = 10f;
    
    
    // State
    private GameObject _target;
    private float _speed;

    public float Speed
    {
        get => _speed;
        set => _speed = value;
    }

    // Constants
    private const float EPSILON = 0.1f;

    public GameObject Target
    {
        get => _target;
        set => _target = value;
    }

    // Cache
    private Rigidbody2D _body;
    // Start is called before the first frame update
    void Start()
    {
        _body = GetComponent<Rigidbody2D>();
        Invoke(nameof(Destroy),maxTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (!followTarget || _target == null) { return; }
        RotateTowards();
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // TODO:  COULD ADD BOUNCE 
       Destroy();
    }

    // TODO Increase Radius For Sharp Turns
    private void RotateTowards()
    {
        // Should Have a max angle and minimum angle
        var dir = _target.transform.position - transform.position;
        // Fix -90 deg
        float angleToLookAt = -Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
        transform.localRotation = Quaternion.AngleAxis(angleToLookAt, Vector3.forward);
        // Change Velocity Vector
        float rot = (transform.rotation.eulerAngles.z + 90f) % 360 * Mathf.Deg2Rad;
        _body.velocity  = new Vector2(Mathf.Cos(rot)*_speed,Mathf.Sin(rot)*_speed);
    }
    
    private void Destroy()
    {
        Destroy(gameObject);
    }
}
