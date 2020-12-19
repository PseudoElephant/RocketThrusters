using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RocketMovement : MonoBehaviour
{
// Parameters
    [SerializeField] private float rotationValue;
    [SerializeField] private float thrustPush;
    [SerializeField] private float velocityDeathThreshHold = Mathf.Epsilon;
    // Cache
    private Rigidbody2D _myRigidBody;
    private AudioSource _audioSource;
    private BoxCollider2D _feet;
    private CapsuleCollider2D _nose;
    
    // State
    private enum State
    {
        Alive, Dying, Transcending 
    }
    private bool _inPlatform = false;
    private State _state = State.Alive;
    
    // Start is called before the first frame update
    void Start()
    {
        _myRigidBody = GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();
        _feet = GetComponentInChildren<BoxCollider2D>();
        _nose = GetComponentInChildren<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    { 
        if (_state != State.Alive) { return; } 
        Thrust();
        Rotate();   
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_state != State.Alive)
        {
            return;
        }
        // If player is landing
        if ((_feet.IsTouching(other) && _myRigidBody.velocity.magnitude > velocityDeathThreshHold) || _nose.IsTouching(other))
        {
            InvokeDeath();
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        // Activates in platform state (could check for velocity)
        if (_feet.IsTouching(other))
        {
            _inPlatform = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Deactivates in platform state
        _inPlatform = false;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (_state != State.Alive)
        {
            return;
        }

        // Assuming Death
        String colliderTag = _feet.IsTouching(other.collider) ? other.gameObject.tag :  "";
        
        
        switch (colliderTag)
        {
            case "Friendly":
                break;
            case "Fuel":
                break;
            case "EndZone":
                _state = State.Transcending;
                print("End Level");
                break;
            default:
                // Feet Touching
              InvokeDeath();
                break;
        }
    }

    private void InvokeDeath()
    {
        _state = State.Dying;
        _myRigidBody.AddForce(Vector2.up*thrustPush);
        _myRigidBody.angularVelocity = thrustPush;
        Invoke(nameof(Die),1f);
    }
    private void Die()
    {
       Destroy(gameObject);
       SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Input Layer (No Multilayer)
    private void Rotate()
    {
        // Only if it has not landed
        if (_inPlatform) { return; }
        // Freeze before getting control
        _myRigidBody.freezeRotation = true;
        
        float rotationSpeed = rotationValue * Time.deltaTime;
        
        // Rotate Left
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward*rotationSpeed);
          //  _myRigidBody.angularVelocity = new Vector3(0,0,rotationValue);
          
         
        }
        
        // Rotate Right
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.back*rotationSpeed);
        }
        
        // UnFreeze before after control
        _myRigidBody.freezeRotation = false;
    }

    private void Thrust()
    {
        // Thrust
        if (Input.GetKey(KeyCode.Space))
        {
            float forceToAdd = thrustPush;
            _myRigidBody.AddRelativeForce(Vector3.up*forceToAdd);

            if (!_audioSource.isPlaying)
            {
                _audioSource.Play();
            }
        }
        else
        {
            if (_audioSource.isPlaying)
            {
                _audioSource.Stop();
            }
        }
    }
}
