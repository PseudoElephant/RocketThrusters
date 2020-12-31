using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMesh : MonoBehaviour
{
    private MeshFilter _meshFilter;

    private void Start()
    {
        _meshFilter = GetComponent<MeshFilter>();
        Mesh mesh = new Mesh();
        mesh.Clear();
        
        Vector3[] verts = new Vector3[3];
        verts[0] = new Vector3(0,0);
        verts[1] = new Vector3(1,1);
        verts[2] = new Vector3(0,1);
        int[] triangles = new int[3];
        triangles[0] = 0;
        triangles[1] = 2;
        triangles[2] = 1;

        mesh.vertices = verts;
        mesh.triangles = triangles;
        _meshFilter.mesh = mesh;
    }
}
