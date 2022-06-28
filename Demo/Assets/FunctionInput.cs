using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FunctionInput : MonoBehaviour
{
    TMP_InputField inputField;
    void Awake()
    {
        inputField = GetComponent<TMP_InputField>();
    }

    public void TriggerGraphRender()
    {
        string function = inputField.text;

        FindObjectOfType<WolframTest>().Solve(function, 0, 20, 0.25f);
    } 
}
