using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class ShooterBehaviour : MonoBehaviour
{
    // Parameters
    [SerializeField] private GameObject target;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed;
    [SerializeField, Range(0f,10f)] float shootSpeed;

    [Range(0f,360f)]
    public float viewAngle = 180;
    [Range(0f, 360f)]
    public float angleOffset = 0;
    
    public float attackRadius;

    // Start Shooting
    private bool _isShooting = true;
    
    //TODO: Add FirePointOffset
    public float FirePointOffset;
    
    // Cache
    private Transform _targetTransform;
    private Transform _parentTransform;

    private bool _inAngle = true;

    // Start is called before the first frame update
    void Start()
    {
        //Starting min/max angles
        StartCoroutine(ShootingRoutine());
        _targetTransform = target.GetComponent<Transform>();
        _parentTransform = GetComponentInParent<Transform>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Should Have a max angle and minimum angle
        var dir = target.transform.position - _parentTransform.position;
        
        // Fix -90 deg
        float angleToLookAt = -Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
        
        //Only Clamp If Angle is less than 360
        Quaternion proxRot = Quaternion.AngleAxis(angleToLookAt, Vector3.forward);
        float rot = MathUtility.Mod(proxRot.eulerAngles.z - angleOffset,360);

        if (rot < viewAngle / 2 || rot > 360 - viewAngle / 2)
        {
            transform.rotation = Quaternion.AngleAxis(angleToLookAt, Vector3.forward);
            _inAngle = true;
        }
        else
            _inAngle = false;

    }

    void Shoot()
    {
        // Only shoot if target is within range
        if (Vector2.Distance(_parentTransform.position, _targetTransform.position) > attackRadius || !_inAngle)
        {
            return;
        }

        // Calculate Direction
        Vector3 offsetFirePoint = MathUtility.NormDirFromAngle(transform.rotation.eulerAngles.z) * FirePointOffset;
        
        GameObject ob = Instantiate(bulletPrefab, _parentTransform.position+offsetFirePoint, transform.rotation);
        float rot = (transform.rotation.eulerAngles.z + 90f) % 360 * Mathf.Deg2Rad;
        // Update Velocity And Target
        Vector2 normDirectionTowardsTarget = new Vector2(Mathf.Cos(rot) * bulletSpeed, Mathf.Sin(rot) * bulletSpeed);



        ob.GetComponent<Rigidbody2D>().velocity = normDirectionTowardsTarget;
        BulletBehaviour bullet = ob.GetComponent<BulletBehaviour>();
        bullet.Target = target;
        bullet.Speed = bulletSpeed;
    }

    IEnumerator ShootingRoutine()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(shootSpeed);
            Shoot();
        }
    }

    // Public Interface

    public void EnableShooter()
    {
        if (!_isShooting)
        {
            StartCoroutine(ShootingRoutine());
            _isShooting = true;
        }
    
    }

    public void DisableShooter()
    {
        if (_isShooting)
        {
            StopAllCoroutines();
            _isShooting = false;
        }
 
    }

    public void ToggleShooter()
    {
        if (_isShooting)
        {
            DisableShooter();
        }
        else
        {
            EnableShooter();
        }
    }
}
