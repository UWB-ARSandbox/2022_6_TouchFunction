using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ASL;
using System;


[RequireComponent(typeof(MeshFilter))]
public class MeshCreator : MonoBehaviour
{
    public Vector3 origin;
    public int GraphIndex;
    int MeshM = 20;//widthwise;
    public int MeshPerX = 4;//lengthwise;
    public bool meshIsEmpty = true;
    public int minVal = 0;
    public int maxVal = 20;

    public Color c;
    public string functionText;
    // public int functionSelection = 0;
    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;

    public float[] yVals;

    string cachedFunction;

    public void InitGraphParameters(int min, int max, float increment)
    {
        minVal = min;
        maxVal = max;
        //MeshPerX = (int)(1 / increment);
    }

    public void RenderGraph(float[] values) {

        //Array.Copy(values, yVals, maxVal*MeshPerX);
        yVals = values;
        createGraphMesh();
        UpdateMesh();
        GetComponent<PointsCreator>().CreatePoints();

         //FindObjectOfType<MeshManager>().addMesh(GetComponent<MeshCreator>());
        
    }

    // Start is called before the first frame update
    void Start()
    {
        //WolframAlpha.onObtainPoints += RenderGraph;
        //WolframAlpha.onObtainPoints += SendPointsToNetwork;

        //GetComponent<ASLObject>()._LocallySetFloatCallback(ReceivePointsFromNetwork);

        mesh = new Mesh();

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
        vertices = new Vector3[MeshM * MeshPerX * maxVal];
        triangles = new int[(MeshM - 1) * (MeshPerX * maxVal - 1) * 6];

        float[] zArr = {-1f, -0.9f, -0.8f, -0.7f, -0.6f, -0.5f, -0.4f, -0.3f, -0.2f, -0.1f, 0f, 0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 0.9f};
        float xIncrement = 1f / MeshPerX;
        float xOrigin = origin.x;
        float yOrigin = origin.y;
        float xVal, yVal;
        
        for (int i = 0; i < MeshPerX * maxVal; i++)
        {
            xVal = xOrigin + (i * xIncrement);
            yVal = yOrigin + getY(i);
            for (int j = 0; j < MeshM; j++)
            {
                vertices[MeshM * i + j] = new Vector3(xVal, yVal, zArr[j]);
            }
        }

        for (int i = 0; i < MeshPerX * maxVal - 1; i++)
        {
            for (int j = 0; j < (MeshM-1); j++)
            {
                triangles[6 * ((MeshM - 1) * i + j)] = MeshM * i + j;
                triangles[6 * ((MeshM - 1) * i + j) + 1] = MeshM * i + j + 1;
                triangles[6 * ((MeshM - 1) * i + j) + 2] = MeshM * (i + 1) + j;

                triangles[6 * ((MeshM - 1) * i + j) + 3] = MeshM * i + j + 1;
                triangles[6 * ((MeshM - 1) * i + j) + 4] = MeshM * (i + 1) + j + 1;
                triangles[6 * ((MeshM - 1) * i + j) + 5] = MeshM * (i + 1) + j;
            }
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
        //mesh = new Mesh();

        //GetComponent<MeshFilter>().mesh = mesh;
        //GetComponent<MeshCollider>().sharedMesh = mesh;
        functionText = "";
        meshIsEmpty = true;
    }



    // public void onMaxValChanged(System.Single newMax)
    // {
    //     mesh.Clear();
    //     maxVal = Mathf.RoundToInt(newMax);

    //     vertices = new Vector3[MeshM * MeshPerX * maxVal];

    //     triangles = new int[(MeshM - 1) * (MeshPerX * maxVal - 1) * 6];
    //     createGraphMesh();
  
    //     UpdateMesh();
    // }

    // public void onResolutionChanged(System.Single newRes)
    // {
    //     mesh.Clear();
    //     MeshPerX = Mathf.RoundToInt(newRes);

    //     vertices = new Vector3[MeshM * MeshPerX * maxVal];
    //     triangles = new int[(MeshM - 1) * (MeshPerX * maxVal - 1) * 6];
    //     createGraphMesh();

    //     UpdateMesh();
    // }

}
