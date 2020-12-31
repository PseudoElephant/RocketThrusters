﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class MeshUpdater : MonoBehaviour
{
    private PolygonCollider2D _polygonCollider2D;

    private MeshFilter _meshFilter;
    // Start is called before the first frame update
    void Start()
    {
        _polygonCollider2D = GetComponent<PolygonCollider2D>();
        _meshFilter = GetComponent<MeshFilter>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMesh();
    }

    private void UpdateMesh()
    {
        Mesh currentMesh = new Mesh();
        currentMesh.Clear();
        Vector2[] points = _polygonCollider2D.points;
        Vector3[] vertices = new Vector3[points.Length+1];
        int[] triangles = new int[(points.Length)*3];

        
        vertices[0] = Vector3.zero;
        for (int i = 1; i < points.Length + 1; i++)
        {
            // Set Vertices
            vertices[i] = points[i-1];
            
            // Set Triangles
            triangles[i * 3 - 3] = 0;
            triangles[i * 3 - 2] = i;
            triangles[i * 3 - 1] = MathUtility.Mod(i,points.Length) + 1;

        }

        currentMesh.vertices = vertices;
        currentMesh.triangles = triangles;
        
  
        _meshFilter.mesh = currentMesh;
    }
}
