using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FunctionInput : MonoBehaviour
{
    InputField inputField;
    void Awake()
    {
        inputField = GetComponent<InputField>();
    }

    public void TriggerGraphRender()
    {
        string function = inputField.text;
        FindObjectOfType<FunctionTextDisplay>().UpdateText(function);
        FindObjectOfType<WolframAlpha>().Solve(function, 0, 20, 0.25f);
    } 
}
