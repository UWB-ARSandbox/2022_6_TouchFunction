using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ASL;
using UnityEngine.UI;
public class MeshManager : MonoBehaviour
{

    public MeshCreator[] meshes;

    public GameObject GraphList;
    public ListEntry ListEntryPrefab;
    public Color[] ColorList;

    // change to private later
    // 0 if the entry is not used, 
    public int[] colorSelected;

    private void Start()
    {
        //meshes = new MeshCreator[MaxMeshes];
        
        // set up WA call backs on receiving float array (y indexes)
        //WolframAlpha.onObtainPoints += ReceivePointsFromWA;
        WolframAlpha.onObtainPoints += SendPointsToNetwork;
        
        // set up local call back on receiving float array (y indexes) from ASL network
        GetComponent<ASLObject>()._LocallySetFloatCallback(onFloatArrayReceived);

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

    // sending y values retrieved from WA, attach a 0f at the front of array
    public void SendPointsToNetwork(float[] values)
    {
        float[] yArr = new float[values.Length + 1];
        yArr[0] = 0f;
        Array.Copy(values, 0, yArr, 1, values.Length);

        GetComponent<ASLObject>().SendAndSetClaim(() =>
        {
            GetComponent<ASLObject>().SendFloatArray(yArr);
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
                    float[] yArr = new float[value.Length - 1];
                    Array.Copy(value, 1, yArr, 0, value.Length - 1);
                    meshes[fs].RenderGraph(yArr);
                    colorSelected[fs] = nextAvailableColor() + 1;
                    meshes[fs].c = ColorList[colorSelected[fs]-1];
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

    /*
    public void btnClick()
    {
        float[] tst = {1, 0};
        GetComponent<ASLObject>().SendAndSetClaim(() =>
        {
            GetComponent<ASLObject>().SendFloatArray(tst);
        });
    }
    */

    // send a function text message in the form of a float array, set first element of the array to 2f;
    public void SendFunctionToNetwork(string fn)
    {
        GetComponent<ASLObject>().SendAndSetClaim(() =>
        {
            float[] fnTextFloat = StringToFloatArray.SToF(fn);
            float[] msg = new float[fnTextFloat.Length + 1];
            msg[0] = 2f;
            Array.Copy(fnTextFloat, 0, msg, 1, fnTextFloat.Length);
            GetComponent<ASLObject>().SendFloatArray(msg);
        });
    }

    // clear the current list, rebuild using current list
    void updateGraphList()
    {
        foreach (Transform child in GraphList.transform)
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
                ListEntry newEntry = Instantiate(ListEntryPrefab);
                newEntry.transform.parent = GraphList.transform;
                newEntry.GetComponent<RectTransform>().localPosition = new Vector3(0, 35 - 45 * i, 0);

                newEntry.MeshC = meshes[i];
                newEntry.ListIndex = i;
                newEntry.delayUpdate();
                newEntry.TMP.color = ColorList[colorSelected[i]-1];
                
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
        GetComponent<ASLObject>().SendAndSetClaim(() =>
        {
            GetComponent<ASLObject>().SendFloatArray(msg);
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
