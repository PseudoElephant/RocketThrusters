using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class SoftBodyCreation : MonoBehaviour
{
    public int numVertices;
    public float distFromCenter;
    public GameObject vertexPrefab;

    private GameObject[] _vertices;
    private Vector3[] _meshVertices;
    private int[] _meshTriangle;
    private float _radiusCollider;
    private int _offsetConnection;

    // Cache
    private MeshFilter _mesh;
    
    
    // Start is called before the first frame update
    private void Awake()
    {
        _mesh = GetComponent<MeshFilter>();
        
    }

    void Start()
    {
        //Setting Perfect Spheres
        float angle = 360 / (2*(numVertices+0f));
        _radiusCollider = distFromCenter * Mathf.Sin(angle * Mathf.Deg2Rad);;
        vertexPrefab.GetComponent<CircleCollider2D>().radius = _radiusCollider;
        GetComponent<CircleCollider2D>().radius = _radiusCollider;
        _offsetConnection = numVertices / 4;
        
        //Calling Methods
        GenerateVertices();
        JoinVertices();
        CreateMesh();
    }

    // Update is called once per frame
    void Update()
    {
        // Update Vertices
        UpdateVertices();
    }

    private void GenerateVertices()
    {
        _vertices = new GameObject[numVertices];
        
        //Initiate all vertices
        Vector2 currentPos = new Vector2(distFromCenter, 0);
        float angleBetweenVertices = 360 / (numVertices+0f);
        
        for (int i = 0; i < numVertices; i++)
        {
            GameObject currentVertex = Instantiate(vertexPrefab, transform);
            currentVertex.transform.localPosition = currentPos;
            currentPos = MathUtility.RotateVectorBy(currentPos, -angleBetweenVertices);

            _vertices[i] = currentVertex;
        }
    }

    private void JoinVertices()
    {
        for (int i = 0; i < numVertices; i++)
        {
            // Basic Circular Structure
            DistanceJoint2D distanceJoint = _vertices[i].GetComponent<DistanceJoint2D>();
            distanceJoint.connectedBody = _vertices[(i + 1) % numVertices].GetComponent<Rigidbody2D>();
            
            // Create Diamonds
            SpringJoint2D[] springs = _vertices[i].GetComponents<SpringJoint2D>();

            if (springs.Length == 4)
            {
                // Right
                springs[0].connectedBody = _vertices[MathUtility.Mod(i + _offsetConnection, numVertices)].GetComponent<Rigidbody2D>();
                
                // Center
                springs[1].connectedBody = GetComponent<Rigidbody2D>();
                springs[1].autoConfigureDistance = false;
                springs[1].distance = distFromCenter;
                
                // Left
                springs[2].connectedBody = _vertices[MathUtility.Mod(i - _offsetConnection, numVertices)].GetComponent<Rigidbody2D>();
                
                //Across
                springs[3].connectedBody = _vertices[MathUtility.Mod(i - _offsetConnection*2, numVertices)].GetComponent<Rigidbody2D>();
            }
            
            HingeJoint2D hinge =  _vertices[i].GetComponent<HingeJoint2D>();
            hinge.connectedBody = _vertices[(i + 1) % numVertices].GetComponent<Rigidbody2D>();
            
        }
    }

    private void CreateMesh()
    {
        Mesh currentMesh = new Mesh();
        currentMesh.Clear();
        Vector3[] vertices = new Vector3[numVertices+1];
        int[] triangles = new int[(numVertices)*3];

        
        vertices[0] = Vector3.zero;
        for (int i = 1; i < numVertices + 1; i++)
        {
            // Set Vertices
            Vector3 posVertex = _vertices[i - 1].transform.localPosition;
            Vector3 newPos = ((posVertex.magnitude + _radiusCollider) / posVertex.magnitude) * posVertex;
            
            vertices[i] = newPos;
            
            // Set Triangles
            triangles[i * 3 - 3] = 0;
            triangles[i * 3 - 2] = i;
            triangles[i * 3 - 1] = MathUtility.Mod(i,numVertices) + 1;

        }

        currentMesh.vertices = vertices;
        currentMesh.triangles = triangles;
        
        _meshVertices = vertices;
        _meshTriangle = triangles;
        _mesh.mesh = currentMesh;
    }

    private void UpdateVertices()
    {
        _mesh.mesh.Clear(true);
        _meshVertices[0] = Vector3.zero;
        for (int i = 1; i < numVertices + 1; i++)
        {
            // Set Vertices
            Vector3 posVertex = _vertices[i - 1].transform.localPosition;
            Vector3 newPos = ((posVertex.magnitude + _radiusCollider) / posVertex.magnitude) * posVertex;

            _meshVertices[i] = newPos;
        }

        var mesh = _mesh.mesh;
        mesh.vertices = _meshVertices;
        mesh.triangles = _meshTriangle;
    }
}
