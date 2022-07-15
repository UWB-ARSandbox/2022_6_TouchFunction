using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using ASL;

public class FunctionTextDisplay : MonoBehaviour
{
    ASLObject aslObj;
    void Awake()
    {
        aslObj = GetComponent<ASLObject>();
    }

    // if we received 
    /*public void ReceiveFunctionFromNetwork(string _id, float[] fn)
    {
        if (fn[0] == 2f)
        {
            float[] fnText = new float[fn.Length-1];
            Array.Copy(fn, 1, fnText, 0, fn.Length-1);
            string funcTxt = StringToFloatArray.FToS(fnText);
            Debug.Log("In FTD:   " + funcTxt);
            //UpdateText(funcTxt);
            MeshManager _mm = FindObjectOfType<MeshManager>();
            _mm.meshes[_mm.findFirstSpace()].functionText = funcTxt;
        }     
    }

    // send a function text message in the form of a float array, set first element of the array to 2f;
    public void SendFunctionToNetwork(string fn)
    {
        aslObj.SendAndSetClaim(() =>
        {
            float[] fnTextFloat = StringToFloatArray.SToF(fn);
            float[] msg = new float[fnTextFloat.Length+1];
            msg[0] = 2f;
            Array.Copy(fnTextFloat, 0, msg, 1, fnTextFloat.Length);
            aslObj.SendFloatArray(msg);
        });
    }*/

    public void UpdateText(string fn)
    {
        GetComponent<TextMesh>().text = fn;
    }
    

}
