using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TemplateInput : MonoBehaviour
{
    public enum FunctionType
    {
        sin,
        cos,
        tan,
        log,
        ln,
        exp,
        reciprocal,
        sqrt,
        abs,
    }

    [SerializeField] FunctionType functionType;
    [SerializeField] TMP_Text functionText;
    [SerializeField] TMP_InputField[] ABCD = new TMP_InputField[4];

    void OnEnable()
    {
        FunctionInput.obtainFunctionEvent += GetInput;
    }

    void OnDisable()
    {
        FunctionInput.obtainFunctionEvent -= GetInput;
    }

    public void ChangeInput(int value)
    {
        switch((FunctionType)value)
        {
            case FunctionType.sin:
                functionText.text = "sin("; break;
            case FunctionType.cos:
                functionText.text = "cos("; break;
            case FunctionType.tan:
                functionText.text = "tan("; break;
            case FunctionType.log:
                functionText.text = "log("; break;
            case FunctionType.ln:
                functionText.text = "ln("; break;
            case FunctionType.exp:
                functionText.text = "e^("; break;
            case FunctionType.reciprocal:
                functionText.text = "/("; break;
            case FunctionType.sqrt:
                functionText.text = "sqrt("; break;
            case FunctionType.abs:
                functionText.text = "abs("; break;
        }
    }

    public string GetInput()
    {
        ValidateInput(ABCD[0], "1");
        ValidateInput(ABCD[1], "1");
        ValidateInput(ABCD[2], "0");
        ValidateInput(ABCD[3], "0");

        switch(functionType)
        {
            case FunctionType.sin:
                return String.Format("{0}sin({1}x+{2})+{3}", ABCD[0].text, ABCD[1].text, ABCD[2].text, ABCD[3].text);
            case FunctionType.cos:
                return String.Format("{0}cos({1}x+{2})+{3}", ABCD[0].text, ABCD[1].text, ABCD[2].text, ABCD[3].text);
            case FunctionType.tan:
                return String.Format("{0}tan({1}x+{2})+{3}", ABCD[0].text, ABCD[1].text, ABCD[2].text, ABCD[3].text);
            case FunctionType.log:
                return String.Format("{0}log({1}x+{2})+{3}", ABCD[0].text, ABCD[1].text, ABCD[2].text, ABCD[3].text);
            case FunctionType.ln:
                return String.Format("{0}ln({1}x+{2})+{3}", ABCD[0].text, ABCD[1].text, ABCD[2].text, ABCD[3].text);
            case FunctionType.exp:
                return String.Format("{0}e^({1}x+{2})+{3}", ABCD[0].text, ABCD[1].text, ABCD[2].text, ABCD[3].text);
            case FunctionType.reciprocal:
                return String.Format("{0}/({1}x+{2})+{3}", ABCD[0].text, ABCD[1].text, ABCD[2].text, ABCD[3].text);
            case FunctionType.sqrt:
                return String.Format("{0}sqrt({1}x+{2})+{3}", ABCD[0].text, ABCD[1].text, ABCD[2].text, ABCD[3].text);
            case FunctionType.abs:
                return String.Format("{0}abs({1}x+{2})+{3}", ABCD[0].text, ABCD[1].text, ABCD[2].text, ABCD[3].text);
        }

        return null;
    }

    void ValidateInput(TMP_InputField numInput, string defaultString) 
    {
        bool isFloat = float.TryParse(numInput.text, out _);

        if (!isFloat) {
            numInput.text = defaultString;
        }
    }
}
