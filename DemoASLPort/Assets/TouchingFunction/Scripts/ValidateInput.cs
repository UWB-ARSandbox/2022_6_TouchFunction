using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ValidateInput : MonoBehaviour
{
    InputField numInput;

    // Start is called before the first frame update
    void Start()
    {
        numInput = gameObject.GetComponent<InputField>();
        Debug.Assert(numInput != null);

        numInput.onValueChanged.AddListener(ValidateInputHandler);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ValidateInputHandler(string input) 
    {
        float num;
        bool isFloat = float.TryParse(input, out num);

        if (!isFloat) {
            numInput.text = "";
            Debug.Log("invalid input");
        }
        else {
            Debug.Log("input: " + num);
        }
    }
}
