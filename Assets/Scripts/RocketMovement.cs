using System;
using System.CodeDom;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utility;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
using UnityEngine.InputSystem;

public class RocketMovement : MonoBehaviour
{
    // Parameters
    [Header("Rocket Movement")]
    [SerializeField] private float rotationValue;
    [SerializeField] private float thrustPush;
    [SerializeField] private float velocityDeathThreshHold = Mathf.Epsilon;
    [Range(0,360)]
    [SerializeField] private float angleThreshHold = 30;
    // Fx
    [Header("Rocket FX's")]
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
    private ParticleSystem _thrustVFX;
    
    // Constants
    const float TrailOffset = 2.25f;
    
    private InputMaster controls;

    private bool _thrusting = false;
    private float _movementDir;

    private void Awake()
    {
        controls = new InputMaster();
        
        //Thrust
        controls.Rocket.Thrust.started += ctx => _thrusting = true;
        controls.Rocket.Thrust.canceled += ctx => _thrusting = false;
        
        //Rotate
        controls.Rocket.Rotate.performed += ctx => _movementDir = ctx.ReadValue<float>();
        controls.Rocket.Rotate.canceled += ctc => _movementDir = 0;
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void FixedUpdate()
    {
        if (_state != State.Alive) { return; } 
        Thrust();
        Rotate(_movementDir);
    }

    // Start is called before the first frame update
    void Start()
    {
        _myRigidBody = GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();
        _feet = GetComponentInChildren<BoxCollider2D>();
        _nose = GetComponentInChildren<CapsuleCollider2D>();

        StartTrail();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        // TODO : Improve Collision Management
        if (_state != State.Alive || other.CompareTag("Trigger"))
        {
            return;
        }
        
        // If player is landing
        if ((_feet.IsTouching(other) && _myRigidBody.velocity.magnitude > velocityDeathThreshHold) || _nose.IsTouching(other))
        {
            InvokeDeath(Vector2.one);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        // TODO : Improve Collision Management
        if (_state != State.Alive || other.CompareTag("Trigger"))
        {
            return;
        }

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
              InvokeDeath(Vector2.up*thrustPush);
                break;
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        _normFloor = other.GetContact(0).normal;
    }

    // Death
    public void InvokeDeath(Vector2 force)
    {
        _state = State.Dying;
        // Adding death push
        _myRigidBody.AddForce(force);
        _myRigidBody.angularVelocity = thrustPush;
        ResetSoundAndFX();
        Invoke(nameof(Die),1f);
    }

    public void InvokeDeath()
    {
        InvokeDeath(new Vector2(0,0));
    }

    private void Die()
    {
       Destroy(gameObject);
       SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // // Input Layer (No Multilayer)
    private void Rotate(float dir)
    {
        if (_state != State.Alive) { return; } 
        
        // Freeze before getting control
        _myRigidBody.freezeRotation = true;
        
        float rotationSpeed = rotationValue * Time.deltaTime;
        
        // Limits angle when in ground
        float rotation = Mathf.Deg2Rad*_myRigidBody.rotation;
        // Direction Vector
        Vector2 rotVector = new Vector2(-Mathf.Sin(rotation),Mathf.Cos(rotation));
        
        // Left Bound
        Vector2 left = MathUtility.RotateVectorBy(_normFloor, -angleThreshHold);
        
        // Right Bound
        Vector2 right = MathUtility.RotateVectorBy(_normFloor, angleThreshHold);
        
        // Rotate Left
        if (dir < 0)
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
        else if (dir > 0)
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
        if (_state != State.Alive) { return; } 
        
        // Thrust
        if (_thrusting)
        {
            float forceToAdd = thrustPush;
            _myRigidBody.AddRelativeForce(Vector3.up*forceToAdd);

            if (!_audioSource.isPlaying)
            {
                _audioSource.Play();
            }

            // Start Trail
            if (_thrustVFX == null)
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
        if (_thrustVFX == null || _thrustVFX.particleCount <= 0) return;
        _thrustVFX.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        _thrustVFX = null;
    }


    private void StartTrail()
    {
        GameObject temp = Instantiate(thrustVfXprefab, transform);
        temp.transform.localPosition = new Vector3(0,-TrailOffset,0);
        _thrustVFX  = temp.GetComponent<ParticleSystem>();
        
    }

}
