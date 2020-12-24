using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CableBridge : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;

    private LineRenderer lineRenderer;
    private List<CableSegment> _cableSegmentsSegments = new List<CableSegment>();
    public float cableSegmentLength = 0.25f;
    public int segmentLength = 35;
    public float lineWidth = 0.1f;

    public Vector2 gravity;
    public int contraintFactor = 50;
    
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        Vector3 ropeStartPoint = startPoint.position;

        for (int i = 0; i < segmentLength; i++)
        {
            _cableSegmentsSegments.Add(new CableSegment(ropeStartPoint));
            ropeStartPoint.y -= cableSegmentLength;
        }
    }

    // Update is called once per frame
    void Update()
    {
        this.DrawCable();
    }

    private void FixedUpdate()
    {
        this.Simulate();
    }

    private void Simulate()
    {
        Vector2 grav = new Vector2(0f,-2f);
        // Forces Being Applied
        for (int i = 1; i < segmentLength; i++)
        {
            CableSegment firstCable = _cableSegmentsSegments[i];
            Vector2 velocity = firstCable.posNow - firstCable.posOld;
            firstCable.posNow += velocity;
            // Here we can add more forces
            firstCable.posNow += grav * Time.fixedDeltaTime;
            _cableSegmentsSegments[i] = firstCable;
        }
        
        // Constraints
        for (int i = 0; i < contraintFactor; i++)
        {
            ApplyContraint();
        }
    }

    private void ApplyContraint()
    {
        // Contraint To First Point
        CableSegment firstCable = _cableSegmentsSegments[0];
        firstCable.posNow = startPoint.position;
        _cableSegmentsSegments[0] = firstCable;
        
        // Contraint To End Point
        CableSegment endCable = _cableSegmentsSegments[segmentLength - 1];
        endCable.posNow = endPoint.position;
        _cableSegmentsSegments[segmentLength - 1] = endCable;
        
        // Fix Positions Based On Constraints
        for (int i = 0; i < segmentLength - 1; i++)
        {
            CableSegment first = _cableSegmentsSegments[i];
            CableSegment second = _cableSegmentsSegments[i+1];
            

            float dist = (first.posNow - second.posNow).magnitude;
            float error = Mathf.Abs(dist - this.cableSegmentLength);
            Vector2 changeDir = Vector2.zero;

            if (dist > cableSegmentLength)
            {
                changeDir = (first.posNow - second.posNow).normalized;
            }
            else if (dist < cableSegmentLength)
            {
                changeDir = (second.posNow - first.posNow).normalized;
            }

            Vector2 changeAmount = changeDir * error;
            if (i != 0)
            {
                first.posNow -= changeAmount * 0.5f;
                _cableSegmentsSegments[i] = first;
                second.posNow += changeAmount * 0.5f;
                _cableSegmentsSegments[i + 1] = second;
            }
            else
            {
                second.posNow += changeAmount;
                _cableSegmentsSegments[i + 1] = second;
            }
        }

    }

    private void DrawCable()
    {
        float lineWidth = this.lineWidth;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        Vector3[] ropePositions = new Vector3[segmentLength];
        for (int i = 0; i < segmentLength; i++)
        {
            ropePositions[i] = _cableSegmentsSegments[i].posNow;
        }

        lineRenderer.positionCount = ropePositions.Length;
        lineRenderer.SetPositions(ropePositions);
    }
    
    
    // Simple Constructor
    public struct CableSegment
    {
        public Vector2 posNow;
        public Vector2 posOld;

        public CableSegment(Vector2 pos)
        {
            this.posNow = pos;
            this.posOld = pos;
        }
    }
}
