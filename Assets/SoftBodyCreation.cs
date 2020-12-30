using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class SoftBodyCreation : MonoBehaviour
{
    public int numVertices;
    public float distFromCenter;
    public GameObject vertexPrefab;
    public GameObject centerPrefab;

    private GameObject[] _vertices;
    private GameObject _center;
    
    // Start is called before the first frame update
    void Start()
    {
        GenerateVertices();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GenerateVertices()
    {
        _vertices = new GameObject[numVertices];
        //Create the center
        _center = Instantiate(centerPrefab, transform);
        
        //Initiate all vertices
        Vector2 currentPos = new Vector2(distFromCenter, 0);
        float angleBetweenVertices = 360 / (numVertices+0f);
        
        for (int i = 0; i < numVertices; i++)
        {
            GameObject currentVertex = Instantiate(vertexPrefab, transform);
            currentVertex.transform.localPosition = currentPos;
            currentPos = MathUtility.RotateVectorBy(currentPos, angleBetweenVertices);

            _vertices[i] = currentVertex;
        }
    }

    private void JoinVertices()
    {
        for (int i = 0; i < numVertices; i++)
        {
            SpringJoint2D spring = _vertices[i].GetComponent<SpringJoint2D>();
            // spring.attachedRigidbody = _vertices[(i + 1) % numVertices].GetComponent<Rigidbody2D>();
            
        }
    }
}
