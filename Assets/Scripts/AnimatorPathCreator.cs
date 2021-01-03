using System.Collections;
using System.Collections.Generic;
using PathCreation;
using UnityEngine;

public class AnimatorPathCreator : MonoBehaviour
{
    // Moves along a path at constant speed.
    // Depending on the end of path instruction, will either loop, reverse, or stop at the end of the path.
    public PathCreator pathCreator;
    public EndOfPathInstruction endOfPathInstruction;
    public AnimationCurve animationMovement;
    //public float speed = 5;
    // Currently In Terms Of Speed
    [Header("Curve In Terms of Percentage of Path vs Time")]
    public float timeStep;
    float distanceTravelled;
    private float timeElapsed;
    
    private bool _isMoving  = true;

    void Start() {
        if (pathCreator != null)
        {
            // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
            pathCreator.pathUpdated += OnPathChanged;
        }
    }

    void Update()
    {
        
        if (pathCreator != null && _isMoving)
        {
            // Constant Time
            timeElapsed += timeStep * Time.deltaTime;
            
            // Animation Evaluated At Time T (Speed Multiplier)
            float normalizedAnimationTime = animationMovement.Evaluate(AnimationAtTime(timeElapsed));
           
            // Get Displacement
            //distanceTravelled += speedValue * timeStep *  Time.deltaTime;
            transform.position = pathCreator.path.GetPointAtTime(normalizedAnimationTime, endOfPathInstruction);
            //transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
            //transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
        }
    }

    // If the path changes during the game, update the distance travelled so that the follower's position on the new path
    // is as close as possible to its position on the old path
    void OnPathChanged() {
        distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
    }
    
    // Get Percentage Of Distance Travelled 
    float PercentageTravelled()
    {
        return distanceTravelled / pathCreator.path.length ;
    }
    
    // Calculate Animation Curve Value
    float AnimationAtTime(float t)
    {
        // Constrain t based on the end of path instruction
        switch (endOfPathInstruction) {
            case EndOfPathInstruction.Loop:
                // If t is negative, make it the equivalent value between 0 and 1
                if (t < 0) {
                    t += Mathf.CeilToInt (Mathf.Abs (t));
                }
                t %= 1;
                break;
            case EndOfPathInstruction.Reverse:
                t = Mathf.PingPong (t, 1);
                break;
            case EndOfPathInstruction.Stop:
                t = Mathf.Clamp01 (t);
                break;
        }

        return t;

    }
    
    
    // Public Methods

    public void EnableMovement()
    {
        _isMoving = true;
    }

    public void DisableMovement()
    {
        _isMoving = false;
    }

    public void ToggleMovement()
    {
        _isMoving = !_isMoving;
    }
}

