using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(BlobBehaviour))]
public class BlobEditor : Editor
{

    
    private void OnSceneGUI()
    {
        BlobBehaviour blob = (BlobBehaviour)target;
        Handles.color = Color.blue;
        Handles.DrawWireArc(blob.transform.position, Vector3.forward, Vector3.up, 360, blob.attackRadius);
        
    }
}
