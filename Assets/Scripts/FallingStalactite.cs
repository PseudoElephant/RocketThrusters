using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingStalactite : MonoBehaviour
{
    public LayerMask detectMask;

    public LayerMask groundMask;

    public float maxRayLength = 10;

    public float gravity = -0.10f;
    
    // Cache

    private Rigidbody2D _rigidbody2D;

    private BoxCollider2D _collider2D;
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _collider2D = GetComponent<BoxCollider2D>();
        StartCoroutine(CheckRayHit());
    }
    
    public IEnumerator CheckRayHit()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();
            RaycastHit2D hit;
            Vector2 pos = transform.position;
            Vector2 direction = pos - new Vector2(pos.x, pos.y*maxRayLength);
            hit  = Physics2D.Raycast(pos, direction, maxRayLength, detectMask);
            if (hit)
            {
                StartCoroutine(Fall());
                break;
            }
        }
    

    }

    public IEnumerator Fall()
    {
        while (true)
        {
            _rigidbody2D.velocity = new Vector2(0, _rigidbody2D.velocity.y + gravity);
            yield return new WaitForFixedUpdate();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Kill();
    }
    

    public void Kill()
    {
        Destroy(gameObject);
    }
    
    
    // Checks floor collision
    private void FixedUpdate()
    {
        if (_collider2D.IsTouchingLayers(groundMask))
        {
            Kill();
        }
    }
}
