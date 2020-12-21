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
    [SerializeField] float minAngle;
    [SerializeField] float maxAngle;
    [SerializeField] float attackRadius;
    
    // Cache
    private Transform targetTransform;
    private Transform parentTransform;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Shoot", shootSpeed, shootSpeed);
        targetTransform = target.GetComponent<Transform>();
        parentTransform = GetComponentInParent<Transform>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var dir = Camera.main.WorldToScreenPoint(target.transform.position) - Camera.main.WorldToScreenPoint(parentTransform.position);
        // Fix -90 deg
        float angleToLookAt = -Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angleToLookAt, Vector3.forward);
    }

    void Shoot()
    {
        GameObject ob = Instantiate(bulletPrefab, parentTransform.position, transform.rotation);
        float rot = (transform.rotation.eulerAngles.z + 90f) % 360 * Mathf.Deg2Rad;
        ob.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Cos(rot)*bulletSpeed,Mathf.Sin(rot)*bulletSpeed);
    }
}
