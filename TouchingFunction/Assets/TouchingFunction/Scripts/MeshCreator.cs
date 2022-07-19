using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ASL;
using System;


[RequireComponent(typeof(MeshFilter))]
public class MeshCreator : MonoBehaviour
{
    public GameObject graphAxes;
    GraphManipulation graphManScript;

    #region Graph Attributes
    public Vector3 origin;
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
        float yPos = -(yScale / 2 - 0.5f);
        Debug.Log("yscale: " + yScale);
        Debug.Log("yPos: " + yPos);

        transform.localScale = new Vector3(xScale, yScale, meshScale.z);
        transform.position = new Vector3(xPos, yPos, meshPos.z);
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

        //float[] zArr = {-1f, -0.9f, -0.8f, -0.7f, -0.6f, -0.5f, -0.4f, -0.3f, -0.2f, -0.1f, 0f, 0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 0.9f};
        float xIncrement = 1f / MeshPerX;
        float xOrigin = origin.x;
        float yOrigin = origin.y;
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
    }


}
