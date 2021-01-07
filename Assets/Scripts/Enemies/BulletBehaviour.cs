using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Utility;

public class BulletBehaviour : MonoBehaviour
{
    // Parameters
    public float maxTime = 10f;
    public float turnTresholdAngle;
    public BulletType bulletType;
    
    private Vector3 _lastDir;
    private GameObject _target; //State
    private float _speed;

    public enum BulletType
    {
        Regular,
        FollowTarget
    } 
    
    //Getters/Setters
    public float Speed
    {
        get => _speed;
        set => _speed = value;
    }
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
        SectionManager.instance.SetTarget(ref _target);
        
        _body = GetComponent<Rigidbody2D>();
        Invoke(nameof(Destroy),maxTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (bulletType != BulletType.FollowTarget || _target == null) { return; }
        RotateTowards();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // TODO:  COULD ADD BOUNCE 
       Destroy();
    }
    
    private void RotateTowards()
    {
        // Should Have a max angle and minimum angle
        Vector3 dir = _target.transform.position - transform.position;
        float angle = Vector3.SignedAngle(_lastDir, dir, Vector3.forward);

        if (Mathf.Abs(angle) > turnTresholdAngle)
        {
            float angleOffset = angle - turnTresholdAngle*Mathf.Sign(angle);
            dir = MathUtility.RotateVectorBy(dir, -angleOffset);
        }
        
        // Fix -90 deg
        float angleToLookAt = -Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
        transform.localRotation = Quaternion.AngleAxis(angleToLookAt, Vector3.forward);
        
        
        // Change Velocity Vector
        float rot = (transform.rotation.eulerAngles.z + 90f) % 360 * Mathf.Deg2Rad;
        _body.velocity  = new Vector2(Mathf.Cos(rot)*_speed,Mathf.Sin(rot)*_speed);

        //Update last direction
        _lastDir = dir;
    }
    
    private void Destroy()
    {
        Destroy(gameObject);
    }
}
