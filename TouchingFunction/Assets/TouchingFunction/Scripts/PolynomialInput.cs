using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PolynomialInput : MonoBehaviour
{
    // get input fields
    public TMP_InputField quarticInput;
    public TMP_InputField cubicInput;
    public TMP_InputField quadraticInput;
    public TMP_InputField linearInput;
    public TMP_InputField constantInput;

    void OnEnable()
    {
        FunctionInput.obtainFunctionEvent += GetInput;
    }

    void OnDisable()
    {
        FunctionInput.obtainFunctionEvent -= GetInput;
    }

    // Runs on button click
    public string GetInput() 
    {
        ValidateInput(quarticInput);
        ValidateInput(cubicInput);
        ValidateInput(quadraticInput);
        ValidateInput(linearInput);
        ValidateInput(constantInput);

        string functionExpression = quarticInput.text + "x^4 + " + 
                                    cubicInput.text + "x^3 + " + 
                                    quadraticInput.text + "x^2 + " + 
                                    linearInput.text + "x +" +
                                    constantInput.text;
        
        Debug.Log(functionExpression);
        ClearInput();
        return functionExpression;
    }

    private void ValidateInput(TMP_InputField numInput) 
    {
        float num;
        bool isFloat = float.TryParse(numInput.text, out num);

        if (!isFloat) {
            numInput.text = "0";
            Debug.Log("invalid input");
        }
        else {
            Debug.Log("input: " + num);
        }
    }

    private void ClearInput()
    {
        quarticInput.text = "";
        cubicInput.text = "";
        quadraticInput.text = "";
        linearInput.text = "";
        constantInput.text = ""; 
    }
}
