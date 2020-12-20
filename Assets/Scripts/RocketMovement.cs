using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor.U2D.Path;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Matrix4x4 = UnityEngine.Matrix4x4;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class RocketMovement : MonoBehaviour
{
// Parameters
    [SerializeField] private float rotationValue;
    [SerializeField] private float thrustPush;
    [SerializeField] private float velocityDeathThreshHold = Mathf.Epsilon;
    [SerializeField] private float angleThreshHold = 30;
    // Fx
    [SerializeField] private GameObject thrustVfXprefab;
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
    private Vector2 _normFloor;
    private ParticleSystem thrustVFX;
    
    // Constants
    const float TrailOffset = 2.5f;
    
    // Start is called before the first frame update
    void Start()
    {
        _myRigidBody = GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();
        _feet = GetComponentInChildren<BoxCollider2D>();
        _nose = GetComponentInChildren<CapsuleCollider2D>();

        StartTrail();
   

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

    private void OnCollisionStay2D(Collision2D other)
    {
        _normFloor = other.GetContact(0).normal;
    }

    private void InvokeDeath()
    {
        _state = State.Dying;
        _myRigidBody.AddForce(Vector2.up*thrustPush);
        _myRigidBody.angularVelocity = thrustPush;
        ResetSoundAndFX();
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
   
        // Freeze before getting control
        _myRigidBody.freezeRotation = true;
        
        float rotationSpeed = rotationValue * Time.deltaTime;
        
        // Limits angle when in ground
        float angleThreshHoldRad = Mathf.Deg2Rad * angleThreshHold;
        float rotation = Mathf.Deg2Rad*_myRigidBody.rotation;
        // Direction Vector
        Vector2 rotVector = new Vector2(-Mathf.Sin(rotation),Mathf.Cos(rotation));
        
        // Left Bound
        Vector2 left = new Vector2(Mathf.Cos(-angleThreshHoldRad)*_normFloor.x - Mathf.Sin(-angleThreshHoldRad)*_normFloor.y,
            Mathf.Cos(-angleThreshHoldRad)*_normFloor.y + Mathf.Sin(-angleThreshHoldRad)*_normFloor.x );
        
        // Right Bound
        Vector2 right = new Vector2(Mathf.Cos(angleThreshHoldRad)*_normFloor.x - Mathf.Sin(angleThreshHoldRad)*_normFloor.y,
            Mathf.Cos(angleThreshHoldRad)*_normFloor.y + Mathf.Sin(angleThreshHoldRad)*_normFloor.x );
        
        // Rotate Left
        if (Input.GetKey(KeyCode.A))
        {
            // Only if it has not landed
            if (_inPlatform)
            {
                if (Vector2.Angle(left,rotVector) < 2*angleThreshHold)
                {
                    transform.Rotate(Vector3.forward*rotationSpeed);
                }
            }
            else
            {
                transform.Rotate(Vector3.forward*rotationSpeed);
                //  _myRigidBody.angularVelocity = new Vector3(0,0,rotationValue);

            }
        }
        
        // Rotate Right
        else if (Input.GetKey(KeyCode.D))
        {
          
            // Only if it has not landed
            if (_inPlatform)
            {
                
                if (Vector2.Angle(right,rotVector) < 2*angleThreshHold)
                {
                    transform.Rotate(Vector3.back*rotationSpeed);
                }
            }
            else
            {
                transform.Rotate(Vector3.back*rotationSpeed);
                //  _myRigidBody.angularVelocity = new Vector3(0,0,rotationValue);

            }
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

            // Start Trail
            if (thrustVFX == null)
            {
                StartTrail();
            }

           
        }
        else
        {
            ResetSoundAndFX();
        }
    }

    private void ResetSoundAndFX()
    {
        if (_audioSource.isPlaying)
        {
            _audioSource.Stop();
        }

        // Stop Trail
        if (thrustVFX == null || thrustVFX.particleCount <= 0) return;
        thrustVFX.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        thrustVFX = null;
    }


    private void StartTrail()
    {
        GameObject temp = Instantiate(thrustVfXprefab, transform);
        temp.transform.localPosition = new Vector3(0,-TrailOffset,0);
        thrustVFX  = temp.GetComponent<ParticleSystem>();
        
    }

    
    

}
