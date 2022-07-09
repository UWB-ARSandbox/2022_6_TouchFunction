using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetInputOnClick : MonoBehaviour
{
    public Button btnClick;

    // get input fields
    public InputField quarticInput;
    public InputField cubicInput;
    public InputField quadraticInput;
    public InputField linearInput;
    public InputField constantInput;
    
    // Start is called before the first frame update
    void Start()
    {
        // attach button event
        btnClick.onClick.AddListener(GetInputOnClickHandler);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Runs on button click
    public void GetInputOnClickHandler() 
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
        FindObjectOfType<WolframAlpha>().Solve(functionExpression, 0, 20, 0.25f);
        ClearInput();
    }

    private void ValidateInput(InputField numInput) 
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
