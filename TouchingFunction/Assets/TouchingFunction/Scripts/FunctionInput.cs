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

    FunctionTextDisplay functionTextDisplay;
    WolframAlpha wolframAlpha;
    
    void Start()
    {
        wolframAlpha = FindObjectOfType<WolframAlpha>();
        functionTextDisplay = FindObjectOfType<FunctionTextDisplay>();
    }

    public void TriggerGraphRender()
    {
        string function = obtainFunctionEvent?.Invoke();
        
        if(function != null)
        {
            functionTextDisplay.SendFunctionToNetwork(function);
            wolframAlpha.Solve(function, float.Parse(min.text), float.Parse(max.text), 0.25f);
            wolframAlpha.GetFunctionInfo(function);
        }
    } 
}
