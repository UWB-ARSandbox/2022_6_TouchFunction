using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FunctionInput : MonoBehaviour
{
    public static Func<string> obtainFunctionEvent;

    public TMP_InputField min;
    public TMP_InputField max;

    WolframAlpha wolframAlpha;
    MeshManager meshManager;
    
    void Start()
    {
        wolframAlpha = FindObjectOfType<WolframAlpha>();
        meshManager = FindObjectOfType<MeshManager>();
    }

    public void TriggerGraphRender()
    {
        string function = obtainFunctionEvent?.Invoke();
        
        if(function != null)
        {
            meshManager.SendFunctionToNetwork(function);
            wolframAlpha.Solve(function, float.Parse(min.text), float.Parse(max.text), 0.25f);
            wolframAlpha.GetFunctionInfo(function);
        }
    } 
}
