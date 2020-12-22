using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterBehaviour : MonoBehaviour
{
// Parameters
    [SerializeField] private GameObject target;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed;
    [SerializeField, Range(0f,10f)] float shootSpeed;

    [Range(0f,360f)]
    public float ViewAngle = 180;
    [Range(0f, 360f)]
    public float AngleOffset = 0;

    private float _realAngleOffset;
    private float _minAngle;
    private float _maxAngle;
    public float AttackRadius;
    
    // Cache
    private Transform targetTransform;
    private Transform parentTransform;

    // Start is called before the first frame update
    void Start()
    {
        //Starting min/max angles
        _realAngleOffset = AngleOffset-90f;
        _minAngle = _realAngleOffset - (ViewAngle / 2);
        _maxAngle = _realAngleOffset + (ViewAngle / 2);

        InvokeRepeating("Shoot", shootSpeed, shootSpeed);
        targetTransform = target.GetComponent<Transform>();
        parentTransform = GetComponentInParent<Transform>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Should Have a max angle and minimum angle
        var dir = Camera.main.WorldToScreenPoint(target.transform.position) - Camera.main.WorldToScreenPoint(parentTransform.position);
        // Fix -90 deg
        float angleToLookAt = -Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;

        //Only Clamp If Angle is less than 360
        if(ViewAngle < 360)
        {
            angleToLookAt = Mathf.Clamp(angleToLookAt, _minAngle, _maxAngle);
        }
       
        transform.rotation = Quaternion.AngleAxis(angleToLookAt, Vector3.forward);
    }

    void Shoot()
    {
        // Only shoot if target is within range
        if (Vector2.Distance(parentTransform.position, targetTransform.position) > AttackRadius)
        {
            return; 
        }
        
        GameObject ob = Instantiate(bulletPrefab, parentTransform.position, transform.rotation);
        float rot = (transform.rotation.eulerAngles.z + 90f) % 360 * Mathf.Deg2Rad;
        // Update Velocity And Target
        ob.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Cos(rot)*bulletSpeed,Mathf.Sin(rot)*bulletSpeed);
        BulletBehaviour bullet =  ob.GetComponent<BulletBehaviour>();
        bullet.Target = target;
        bullet.Speed = bulletSpeed;
    }
}
