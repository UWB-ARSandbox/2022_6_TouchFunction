using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ASL;

public class MeshManager : MonoBehaviour
{
    ASLObject aslObj;

    public int MaxMeshes = 4;
    public MeshCreator[] meshes;

    private void Start()
    {
        aslObj = GetComponent<ASLObject>();
        //meshes = new MeshCreator[MaxMeshes];
        
        // set up WA call backs on receiving float array (y indexes)
        // WolframAlpha.onObtainPoints += ReceivePointsFromWA;
        WolframAlpha.onObtainPoints += SendPointsToNetwork;
        
        // set up local call back on receiving float array (y indexes) from ASL network
        aslObj._LocallySetFloatCallback(ReceivePointsFromNetwork);
    }

    private int findFirstSpace()
    {
        for (int i = 0; i < MaxMeshes; i++)
        {
            if (meshes[i].isEmpty())
            {
                return i;
            }
        }
        return -1;
    }

    // public void ReceivePointsFromWA(float[] values)
    // {
    //     meshes[findFirstSpace()].RenderGraph(values);
    // }

    public void ReceivePointsFromNetwork(string _id, float[] _f)   
    {
        int index = findFirstSpace();
        var values = new float[_f.Length - 3];
        Array.Copy(_f, 3, values, 0, values.Length);
        
        meshes[index].InitGraphParameters((int)_f[0], (int)_f[1], _f[2]);        
        meshes[index].RenderGraph(values);
    }

    private void SendPointsToNetwork(float[] _f)
    {
        aslObj.SendAndSetClaim(() =>
        {
            aslObj.SendFloatArray(_f);
        });
    }
}
