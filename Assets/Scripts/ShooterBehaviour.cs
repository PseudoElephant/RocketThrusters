using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed;
    private Transform targetTransform;
    [SerializeField, Range(0f,10f)] float shootSpeed;
    [SerializeField] float treshHoldAngle;
    private Transform parentTransform;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Shoot", 0, shootSpeed);
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
        ob.GetComponent<Rigidbody2D>().AddRelativeForce(Vector3.up*bulletSpeed);
    }
}
