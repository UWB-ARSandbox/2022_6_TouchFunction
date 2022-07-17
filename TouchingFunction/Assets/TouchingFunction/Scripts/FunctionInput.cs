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
    public TMP_InputField width;

    public int MeshResolution = 4;
    WolframAlpha wolframAlpha;
    MeshManager meshManager;
    
    void Start()
    {
        wolframAlpha = FindObjectOfType<WolframAlpha>();
        meshManager = FindObjectOfType<MeshManager>();
    }

    public void TriggerGraphRender()
    {
        if (!meshManager.ListIsFull ())
        {
            string function = obtainFunctionEvent?.Invoke();
        
            if(function != null)
            {
                meshManager.SendFunctionToNetwork(function);
                wolframAlpha.Solve(function, float.Parse(min.text), float.Parse(max.text), float.Parse(width.text), 1f /MeshResolution);
                wolframAlpha.GetFunctionInfo(function);
            }
        } else
        {
            // list is full prompt
            Debug.Log("LIST IS FULL");
        }
        
    } 
}
