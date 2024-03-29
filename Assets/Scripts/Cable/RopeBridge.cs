﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeBridge : MonoBehaviour
{
    public Transform StartPoint;
    public Transform EndPoint;

    private LineRenderer lineRenderer;
    private List<RopeSegment> ropeSegments = new List<RopeSegment>();
    public float ropeSegLen = 0.25f;
    public int segmentLength = 35;
    public float lineWidth = 0.1f;

    public Vector2 gravity = new Vector2(0f,-1f);
    public int contraintFactor = 50;

    [Header("Perlin Noise Configuration")] public bool perlinNoise = true;
    public Vector2 perlinDirection  = new Vector2(0,1f);
    public float perlinStrength = 10f;
    public float perlinSampleSpeed = 5f;
    
    private Queue<Vector2> queuedForces = new Queue<Vector2>();
    
    
    
    // Use this for initialization
    void Start()
    {
        this.lineRenderer = this.GetComponent<LineRenderer>();
        Vector3 ropeStartPoint = StartPoint.position;

        for (int i = 0; i < segmentLength; i++)
        {
            this.ropeSegments.Add(new RopeSegment(ropeStartPoint));
            ropeStartPoint.y -= ropeSegLen;
        }

        perlinDirection = perlinDirection.normalized;
    }

    // Update is called once per frame
    void Update()
    {
        this.DrawRope();
    }

    private void FixedUpdate()
    {
        this.Simulate();
    }

    
    private void Simulate()
    {
        // SIMULATION
        //Vector2 forceGravity = new Vector2(0f, -1f);
        var arrayForces = queuedForces.ToArray(); // Forces to be applied
        
        for (int i = 1; i < this.segmentLength; i++)
        {
            RopeSegment firstSegment = this.ropeSegments[i];
            Vector2 velocity = firstSegment.posNow - firstSegment.posOld;
            firstSegment.posOld = firstSegment.posNow;
            firstSegment.posNow += velocity;
            // Gravity Force
            firstSegment.posNow += this.gravity * Time.fixedDeltaTime;
            // Adding Perlin (Could Add Strength)
            if (perlinNoise)
                firstSegment.posNow += perlinDirection * (perlinStrength * Mathf.PerlinNoise(Time.time * perlinSampleSpeed,0) * Time.fixedDeltaTime);
            
            foreach(var force in arrayForces)
            {
                firstSegment.posNow += force * Time.fixedDeltaTime;
            }
            
            this.ropeSegments[i] = firstSegment;
        }
        queuedForces.Clear(); // clear forces

        //CONSTRAINTS
        for (int i = 0; i < contraintFactor; i++)
        {
            this.ApplyConstraint();
        }
        
    }
    

    private void ApplyConstraint()
    {
        //Constrant to First Point 
        RopeSegment firstSegment = this.ropeSegments[0];
        firstSegment.posNow = this.StartPoint.position;
        this.ropeSegments[0] = firstSegment;


        //Constrant to Second Point  ( If we remove this the end block contrain is eliminated ) 
        RopeSegment endSegment = this.ropeSegments[this.ropeSegments.Count - 1];
        endSegment.posNow = this.EndPoint.position;
        this.ropeSegments[this.ropeSegments.Count - 1] = endSegment;

        for (int i = 0; i < this.segmentLength - 1; i++)
        {
            RopeSegment firstSeg = this.ropeSegments[i];
            RopeSegment secondSeg = this.ropeSegments[i + 1];

            float dist = (firstSeg.posNow - secondSeg.posNow).magnitude;
            float error = dist - ropeSegLen;
            Vector2 changeDir = (firstSeg.posNow - secondSeg.posNow).normalized;
            Vector2 changeAmount = changeDir * error;
            
            if (i != 0)
            {
                firstSeg.posNow -= changeAmount * 0.5f;
                this.ropeSegments[i] = firstSeg;
                secondSeg.posNow += changeAmount * 0.5f;
                this.ropeSegments[i + 1] = secondSeg;
            }
            else
            {
                secondSeg.posNow += changeAmount;
                this.ropeSegments[i + 1] = secondSeg;
            }
        }
    }

    public void AddForceAt(Vector2 force)
    {
        queuedForces.Enqueue(force);
    }

    private void DrawRope()
    {
        float lineWidth = this.lineWidth;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        Vector3[] ropePositions = new Vector3[this.segmentLength];
        for (int i = 0; i < this.segmentLength; i++)
        {
            ropePositions[i] = this.ropeSegments[i].posNow;
        }

        lineRenderer.positionCount = ropePositions.Length;
        lineRenderer.SetPositions(ropePositions);
    }

    public struct RopeSegment
    {
        public Vector2 posNow;
        public Vector2 posOld;

        public RopeSegment(Vector2 pos)
        {
            this.posNow = pos;
            this.posOld = pos;
        }
    }
}