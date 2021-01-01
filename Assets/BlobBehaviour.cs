using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class BlobBehaviour : MonoBehaviour
{
    public int attackRadius;
    public GameObject target;
    public float speed;
    
    private JelloBody _body;
    private bool _grounded = false;
    private Rigidbody2D _rigidbody2D;
    
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _body = GetComponent<JelloBody>();
        _body.JelloCollisionEvent += ProcessCollisionEvent; 
        StartCoroutine(Attack());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // _grounded = false;
        //find the direction from the point masses curerrent position to its respective xformed base shape position.
    }

    IEnumerator Attack()
    {
        while (true)
        {
            if (_grounded)
            {
                yield return new WaitForSeconds(1);
                Vector2 direction = (target.transform.position - transform.position).normalized;
                _body.AddForce(direction * speed);
                _grounded = false;
            }
            else
            {
                yield return new WaitForSeconds(1);
            }
            
            //_body.AddForce(direction * speed, _body.getInternalPointMass(0).Position, true);
            //_body.getInternalPointMass(0);
        }
    }
    private void ProcessCollisionEvent(JelloCollision jelloCollision)
    {
        //grounded logic
        if(!_grounded) {
            //loop through each contact in the collision.
            for (int i = 0; i < jelloCollision.contacts.Length; i++)
            {
                //if the hit point of this contact is below the center of the body, count as grounded.
                //This works well for the JelloCharacter in the SpiffyDemoScene but may not be the best for all cases.
                if (jelloCollision.contacts[i].hitPoint.y < transform.position.y)
                {
                    _grounded = true;
                }
            }
        }
    }
    

    

}
