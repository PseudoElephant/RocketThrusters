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
    // Cache
    private Rigidbody2D _myRigidBody;
    private AudioSource _audioSource;
    
    // Start is called before the first frame update
    void Start()
    {
        _myRigidBody = GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Thrust();
        Rotate();   
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        switch (other.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Fuel":
                break;
            default:
                Die();
                break;
        }
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
            
            _myRigidBody.AddRelativeForce(Vector3.up*thrustPush);

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
