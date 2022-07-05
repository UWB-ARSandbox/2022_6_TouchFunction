using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ASL;

public class FunctionTextDisplay : MonoBehaviour
{
    ASLObject aslObj;
    void Start()
    {
        aslObj = GetComponent<ASLObject>();
        aslObj._LocallySetFloatCallback(ReceiveFunctionFromNetwork);
    }

    public void ReceiveFunctionFromNetwork(string _id, float[] fn)
    {
        Debug.Log("Received function text");
        UpdateText(StringToFloatArray.FToS(fn));
    }

    public void SendFunctionToNetwork(string fn)
    {
        Debug.Log("Sent function text");
        aslObj.SendAndSetClaim(() =>
        {
            aslObj.SendFloatArray(StringToFloatArray.SToF(fn));
        });
    }

    public void UpdateText(string fn)
    {
        GetComponent<TextMesh>().text = fn;
    }
    
}
