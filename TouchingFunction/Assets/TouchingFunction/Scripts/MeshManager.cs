using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ASL;

public class MeshManager : MonoBehaviour
{
    

    public int MaxMeshes = 4;
    public MeshCreator[] meshes;

    private void Start()
    {
        //meshes = new MeshCreator[MaxMeshes];
        
        // set up WA call backs on receiving float array (y indexes)
        WolframAlpha.onObtainPoints += ReceivePointsFromWA;
        WolframAlpha.onObtainPoints += SendPointsToNetwork;
        
        // set up local call back on receiving float array (y indexes) from ASL network
        GetComponent<ASLObject>()._LocallySetFloatCallback(ReceivePointsFromNetwork);
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

    public void ReceivePointsFromWA(float[] values)
    {
        meshes[findFirstSpace()].RenderGraph(values);
    }

    public void ReceivePointsFromNetwork(string _id, float[] values)   
    {
        //Debug.Log(_id);
        //if (GetComponent<ASLObject>().m_Id != _id)
        //{   
            ReceivePointsFromWA(values);
        //}
        
    }

    private void SendPointsToNetwork(float[] values)
    {
        GetComponent<ASLObject>().SendAndSetClaim(() =>
        {
            GetComponent<ASLObject>().SendFloatArray(values);
        });
    }
}
