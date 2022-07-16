using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ASL;
using UnityEngine.UI;
public class MeshManager : MonoBehaviour
{
    ASLObject aslObj;
    public MeshCreator[] meshes;

    public GameObject graphList;
    public ListEntry listEntryPrefab;
    public Color[] colorList;
    public int[] colorSelected;

    private void Start()
    {
        aslObj = GetComponent<ASLObject>();
        //meshes = new MeshCreator[MaxMeshes];
        
        // set up WA call backs on receiving float array (y indexes)
        // WolframAlpha.onObtainPoints += ReceivePointsFromWA;
        WolframAlpha.onObtainPoints += SendPointsToNetwork;
        
        // set up local call back on receiving float array (y indexes) from ASL network
        aslObj._LocallySetFloatCallback(onFloatArrayReceived);

        colorSelected = new int[meshes.Length];
    }

    public int findFirstSpace()
    {
        for (int i = 0; i < meshes.Length; i++)
        {
            if (meshes[i].isEmpty())
            {
                return i;
            }
        }
        return -1;
    }

    private void SendPointsToNetwork(float[] _f)
    {
        float[] yArr = new float[_f.Length + 1];
        yArr[0] = 0;

        Array.Copy(_f, 0, yArr, 1, _f.Length);

        aslObj.SendAndSetClaim(() =>
        {
            aslObj.SendFloatArray(yArr);
        });
    }

    /*  Determine what we do with the array based on value[0]
        *  0: Use the float array to render graph
        *  1: Erase the graph with index of value[1]
        *  2: Displaying the function text
        * 
        */
    public void onFloatArrayReceived(string _id, float[] value)
    {
        Debug.Log("ARRAY RECEIVED!!!!!!!!!!!!!!");
     
        switch (value[0])
        {
            case 0f:
                Debug.Log("Received Y coordinates");
                int fs = findFirstSpace();
                if (fs >= 0)
                {
                    float[] yArr = new float[value.Length - 4];
                    Array.Copy(value, 4, yArr, 0, yArr.Length);
                    meshes[fs].InitGraphParameters((int)value[1], (int)value[2], value[3]);   
                    meshes[fs].RenderGraph(yArr);
                    colorSelected[fs] = nextAvailableColor() + 1;
                    meshes[fs].c = colorList[colorSelected[fs]-1];
                    updateGraphList();
                }
                break;

            case 1f:
                Debug.Log("Deleting Mesh: " + (int)value[1]);
                meshes[(int)value[1]].clearMesh();
                colorSelected[(int)value[1]] = 0;
                //updateGraphList();
                deleteEntry((int)value[1]);
                break;

            case 2f:
                float[] fnText = new float[value.Length - 1];
                Array.Copy(value, 1, fnText, 0, value.Length - 1);
                meshes[findFirstSpace()].functionText = StringToFloatArray.FToS(fnText);
                //UpdateText(StringToFloatArray.FToS(fnText));
                //updateGraphList();
                break;

            default:
                break;      
        }

    }

    // send a function text message in the form of a float array, set first element of the array to 2f;
    public void SendFunctionToNetwork(string fn)
    {
        float[] fnTextFloat = StringToFloatArray.SToF(fn);
        float[] msg = new float[fnTextFloat.Length + 1];
        msg[0] = 2f;
        Array.Copy(fnTextFloat, 0, msg, 1, fnTextFloat.Length);

        aslObj.SendAndSetClaim(() =>
        {
            aslObj.SendFloatArray(msg);
        });
    }

    // clear the current list, rebuild using current list
    void updateGraphList()
    {
        foreach (Transform child in graphList.transform)
        {
            Debug.Log("CHILD NAME: " + child.name);
            child.GetComponent<ListEntry>().SelfExplode();
        }

        for (int i = 0; i < meshes.Length; i++)
        {
            /*if (!meshes[i].isEmpty())
            {
                GameObject newEntry = Instantiate(ListEntryPrefab);
                newEntry.transform.parent = GraphList.transform;
                newEntry.transform.position = new Vector3(0, 35 - 45 * getFirstFreeEntrySlot(), 0);
                
            }*/
            if (!meshes[i].isEmpty())
            {
                ListEntry newEntry = Instantiate(listEntryPrefab);
                newEntry.transform.parent = graphList.transform;
                newEntry.GetComponent<RectTransform>().localPosition = new Vector3(0, 35 - 45 * i, 0);

                newEntry.MeshC = meshes[i];
                newEntry.ListIndex = i;
                newEntry.delayUpdate();
                newEntry.TMP.color = colorList[colorSelected[i]-1];   
            }
        }
    }

    // return the index of the first slot on GraphList that's available
    private int getFirstFreeEntrySlot()
    {
        for (int i = 0; i < meshes.Length; i++)
        {
            if (colorSelected[i] == 0)
            {
                return i;
            }
        }
        return -1;
    }

    // return the index of the first material that hasn't been used currently
    private int nextAvailableColor()
    {
        int[] m = new int[meshes.Length];
        for (int i = 0; i < meshes.Length; i++)
        {
            if (colorSelected[i] > 0)
            {
                m[colorSelected[i] - 1] = 1;
            }
        }
        for (int i = 0; i < meshes.Length; i++)
        {
            if (m[i] == 0)
            {
                return i;
            }
        }
        return -1;
    }

    public void SendDeleteEntry(int index)
    {
        float[] msg = { 1, index };
        aslObj.SendAndSetClaim(() =>
        {
            aslObj.SendFloatArray(msg);
        });

    }

    private void deleteEntry(int index)
    {
        ListEntry[] le = (ListEntry[])GameObject.FindObjectsOfType(typeof(ListEntry));
        foreach(ListEntry li in le)
        {
            if (li.ListIndex == index)
            {
                li.SelfExplode();
            }
        }
    }

}
