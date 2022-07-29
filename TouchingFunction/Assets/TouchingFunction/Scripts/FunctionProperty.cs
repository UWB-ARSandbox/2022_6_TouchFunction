using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FunctionProperty : MonoBehaviour
{
    Text text;
    public FunctionPropertyDisplay FPD;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponentInChildren<Text>();
        text.text = FPD.GetFPDText();
    }

    // Update is called once per frame
    void Update()
    {
        if(text.text == "")
        {
            text.text = FPD.GetFPDText();
        }
        
    }

    public void ClearText()
    {
        if(text != null)
        {
            text.text = "";
        }
    }
}
