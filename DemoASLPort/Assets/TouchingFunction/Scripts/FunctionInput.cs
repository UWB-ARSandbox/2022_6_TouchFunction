using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FunctionInput : MonoBehaviour
{
    InputField inputField;
    FunctionTextDisplay functionTextDisplay;
    WolframAlpha wolframAlpha;
    void Awake()
    {
        inputField = GetComponent<InputField>();
    }

    void Start()
    {
        wolframAlpha = FindObjectOfType<WolframAlpha>();
        functionTextDisplay = FindObjectOfType<FunctionTextDisplay>();
    }

    public void TriggerGraphRender()
    {
        string function = inputField.text;
        functionTextDisplay.UpdateText(function);
        functionTextDisplay.SendFunctionToNetwork(function);
        wolframAlpha.Solve(function, 0, 20, 0.25f);
        wolframAlpha.GetFunctionInfo(function);
    } 
}
