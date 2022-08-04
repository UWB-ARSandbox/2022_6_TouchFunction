using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundMesh : MonoBehaviour
{
    public Vector3[] vertices;
    public Vector2[] uvs;
    // Start is called before the first frame update
    void Start()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;
        //uvs = mesh.uv;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
