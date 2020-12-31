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

    // Cache
    private MeshFilter _mesh;
    private PolygonCollider2D _polygonCollider2D;
    
    
    // Start is called before the first frame update
    private void Awake()
    {
        _mesh = GetComponent<MeshFilter>();
        _polygonCollider2D = GetComponent<PolygonCollider2D>();

    }

    void Start()
    {

        GenerateVertices();
        JoinVertices();
        CreateMesh();

    }

    // Update is called once per frame
    void Update()
    {
        // Update Vertices
        UpdateVertices();
        UpdatePolygonCollider();
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

            if (springs.Length == 3)
            {
                // Right
                springs[0].connectedBody = _vertices[MathUtility.Mod(i + 2, numVertices)].GetComponent<Rigidbody2D>();
                // Center
                springs[1].connectedBody = GetComponent<Rigidbody2D>();
                springs[1].autoConfigureDistance = false;
                springs[1].distance = distFromCenter;
                // Left
                springs[2].connectedBody = _vertices[MathUtility.Mod(i - 2, numVertices)].GetComponent<Rigidbody2D>();
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
            vertices[i] = _vertices[i-1].transform.localPosition;
            
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
        _meshVertices[0] =  Vector3.zero;
        for (int i = 1; i < numVertices + 1; i++)
        {
            // Set Vertices
            _meshVertices[i] = _vertices[i-1].transform.localPosition;
            
        }

        var mesh = _mesh.mesh;
        mesh.vertices = _meshVertices;
        mesh.triangles = _meshTriangle;
    }

    private void UpdatePolygonCollider()
    {
        Vector2[] points = new Vector2[numVertices];
        for(int i = 1;i < _meshVertices.Length;i++)
        {
            points[i - 1] = _meshVertices[i];
        }

        _polygonCollider2D.points = points;
    }
}
