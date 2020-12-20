using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterBehaviour : MonoBehaviour
{
    [SerializeField] GameObject target;
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
        float angleToLookAt = Mathf.Atan(targetTransform.position.x - parentTransform.position.x / targetTransform.position.y - parentTransform.position.y);
        float angleAdd = (transform.rotation.z + angleToLookAt) / 2f;
        transform.Rotate(0, 0, angleAdd * Time.deltaTime * 100, Space.Self);
    }

    void Shoot()
    {
        
    }
}
