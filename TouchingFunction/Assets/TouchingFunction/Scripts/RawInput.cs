using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RawInput : MonoBehaviour
{
    public TMP_InputField inputField;
    
    void OnEnable()
    {
        FunctionInput.obtainFunctionEvent += GetInput;
    }

    void OnDisable()
    {
        FunctionInput.obtainFunctionEvent -= GetInput;
    }

    public string GetInput()
    {
        return inputField.text;
    }
}
