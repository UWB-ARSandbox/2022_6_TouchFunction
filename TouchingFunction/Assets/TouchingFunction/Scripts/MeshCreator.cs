using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ASL;
using System;


[RequireComponent(typeof(MeshFilter))]
public class MeshCreator : MonoBehaviour
{
    public GameObject graphAxes;
    public GraphManipulation graphManScript;

    #region Graph Attributes
    // public Vector3 origin;
    public int GraphIndex;
    public Color c;              // Render color of the graph
    public string functionText;  // function expression string
    public string functionProp;  // function property details
    #endregion

    #region Mesh Settings
    float[] zVals; // z index of vertices
    bool meshIsEmpty = true;
    public int minVal = 0;       // minimum of X to render
    public int maxVal = 20;      // maximum of X to render
    public int MeshPerX;     // number of vertices per increment of X;
    #endregion

    #region Mesh Data
    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;
    public float[] yVals;
    public Vector3[] normals;
    #endregion


    string cachedFunction;

    public void onGraphChanged() {
        Vector3 meshScale = transform.localScale;
        Vector3 meshPos = transform.position;
        
        double xUnitSpace = graphManScript.GetXUnitSpace();
        float xScale = (float) xUnitSpace;
        float xPos = (xScale - 1) * maxVal;
        Debug.Log("xscale: " + xScale);
        Debug.Log("xPos: " + xPos);

        double yUnitSpace = graphManScript.GetYUnitSpace();
        float yScale = (float) yUnitSpace;
        float yPos = graphAxes.transform.localPosition.y - (yScale / 2);
        Debug.Log("yscale: " + yScale);
        Debug.Log("yPos: " + yPos);

        transform.localScale = new Vector3(xScale, yScale, meshScale.z);
        transform.position = graphAxes.transform.localPosition;
    }

    public void InitGraphParameters(int min, int max, int width, float increment)
    {
        /*MeshPerX = Mathf.RoundToInt(1f / increment);
        MeshM = 2 * width + 1;
        minVal = min;
        maxVal = max;
        //MeshPerX = (int)(1 / increment);
        zVals = new float[MeshM];
        for (int i = 0; i < MeshM; i++)
        {
            zVals[i] = -(width / 2f) + i * 0.5f;
        }*/
        MeshPerX = Mathf.RoundToInt(1f / increment);
        //Debug.LogError(increment);
        minVal = min;
        maxVal = max;
        //MeshPerX = (int)(1 / increment);
        zVals[0] = -(width / 2f);
        zVals[1] = width / 2f;
    }

    public void RenderGraph(float[] values) {

        //Array.Copy(values, yVals, maxVal*MeshPerX);
        yVals = values;
        createGraphMesh();
        UpdateMesh();
        //GetComponent<PointsCreator>().CreatePoints();
        onGraphChanged();

         //FindObjectOfType<MeshManager>().addMesh(GetComponent<MeshCreator>());
        
    }

    // Start is called before the first frame update
    void Start()
    {
        graphManScript = graphAxes.GetComponent<GraphManipulation>();
        //WolframAlpha.onObtainPoints += RenderGraph;
        //WolframAlpha.onObtainPoints += SendPointsToNetwork;

        //GetComponent<ASLObject>()._LocallySetFloatCallback(ReceivePointsFromNetwork);
        MeshPerX = FindObjectOfType<FunctionInput>().MeshResolution;
        mesh = new Mesh();
        zVals = new float[2];
        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;
        transform.localPosition = graphAxes.transform.localPosition;
    }

    /*public void ReceivePointsFromNetwork(string _id, float[] values)
    {
        RenderGraph(values);
    }

    
    public void SendPointsToNetwork(float[] values)
    {
        GetComponent<ASLObject>().SendAndSetClaim(() =>
        {
            GetComponent<ASLObject>().SendFloatArray(values);
        });
    } */

    // using function y = 5sin(x/3) + 5
    void createGraphMesh()
    {
        meshIsEmpty = false;
        vertices = new Vector3[2 * MeshPerX * (maxVal - minVal)];
        triangles = new int[(MeshPerX * (maxVal - minVal) - 1) * 6];
        normals = new Vector3[vertices.Length];

        //float[] zArr = {-1f, -0.9f, -0.8f, -0.7f, -0.6f, -0.5f, -0.4f, -0.3f, -0.2f, -0.1f, 0f, 0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 0.9f};
        float xIncrement = 1f / MeshPerX;
        float xOrigin = 0f;
        float yOrigin = 0f;
        float xVal, yVal;
        
        for (int i = 0; i < MeshPerX * (maxVal - minVal); i++)
        {
            xVal = xOrigin + minVal + (i * xIncrement);
            yVal = yOrigin + getY(i);
            for (int j = 0; j < 2; j++)
            {
                vertices[2 * i + j] = new Vector3(xVal, yVal, zVals[j]);
            }
        }

        for (int i = 0; i < MeshPerX * (maxVal - minVal) - 1; i++)
        {

             triangles[6 * i ] = 2 * i;
             triangles[6 * i + 1] = 2 * i + 1;
             triangles[6 * i + 2] = 2 * (i + 1) ;

             triangles[6 * i + 3] = 2 * i + 1;
             triangles[6 * i + 4] = 2 * (i + 1) + 1;
             triangles[6 * i + 5] = 2 * (i + 1);
            
        }

        int vlength = vertices.Length;
        // edge cases
        for (int i = 2; i < vlength - 3; i+=2)
        {
            Vector3 norm = (Vector3.Cross((vertices[i] - vertices[i-1]), (vertices[i+1] - vertices[i])) + 
                                Vector3.Cross((vertices[i+2] - vertices[i]), (vertices[i + 1] - vertices[i]))).normalized;
            normals[i] = -norm;
            normals[i + 1] = -norm;
        }
        normals[0] = -Vector3.Cross((vertices[2] - vertices[0]), (vertices[1] - vertices[0])).normalized;
        normals[1] = normals[0];
        normals[vlength - 2] = -Vector3.Cross((vertices[vlength-3] - vertices[vlength-1]), (vertices[vlength-2] - vertices[vlength-1])).normalized;
        normals[vlength - 1] = normals[vlength - 2];
    }

    public bool isEmpty()
    {
        return meshIsEmpty;
    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.normals = normals;
        GetComponent<MeshCollider>().sharedMesh = mesh;
        //GetComponent<Renderer>().material.color = c;

    }

    float getY(int x)
    {
        return yVals[x];
        // switch (functionSelection) {
        //     case 0: 
        //         return 5 * Mathf.Sin(x / 3f) + 5;
        //     case 1:
        //         return -0.5f * x + 10f;
        //     case 2:
        //         return x * x / 2;
        //     default:
        //         return 5 * Mathf.Sin(x / 3f) + 5;
        // }
    }


    public void clearMesh()
    {
        mesh.Clear();
        functionText = "";
        meshIsEmpty = true;
        
        // Reset mesh collider
        GetComponent<MeshCollider>().sharedMesh = mesh; 
        
        // Delete all the points associate with the mesh
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }
    }

    public Vector3 FindClosestPoint(Vector3 point)
    {
        Vector3 graphOrigin = graphAxes.transform.localPosition;
        
        float xDistance = point.x - graphOrigin.x;
        float yDistance = point.y - graphOrigin.y;

        float x = xDistance / (float) graphManScript.GetXUnitSpace();
        float y = yDistance / (float) graphManScript.GetYUnitSpace();

        return new Vector3(x, y, 0);
    }

/*    public float EstimateY(Vector3 p) 
    {
        Vector3 cp = FindClosestPoint(p);

        int x1i = Mathf.FloorToInt(cp.x*MeshPerX);
        int x2i = x1i + 1;
        float y1 = yVals[x1i];
        float y2 = yVals[x2i];
        return (y2 - y1) / (x2i / MeshPerX - x1i / MeshPerX) * (cp.x - ( x1i / MeshPerX )) + y1 ;
    }

    public bool IsAboveGraph(Vector3 p)
    {

        *//*Vector3 p1 = FindClosestPoint(p);
        Vector3 p2 = FindClosestPoint(new Vector3(p.x + (float)graphManScript.GetXUnitSpace(), p.y, p.z));
        //Debug.Log(Vector3.SignedAngle(p - p1, p2 - p1, Vector3.up));
        Debug.DrawRay(p1,p2,Color.blue,0.01f);
        Debug.DrawRay(p1, p, Color.black,0.01f) ;
        return Vector3.SignedAngle(p - p1, p2 - p1, Vector3.up) > 0;


        //return FindClosestPoint(p).y < p.y;*//*
        Debug.DrawRay(p, new Vector3(p.x, EstimateY(p), p.z), Color.blue, 0.01f);
        return EstimateY(p) < p.y;
    }
*/
}
