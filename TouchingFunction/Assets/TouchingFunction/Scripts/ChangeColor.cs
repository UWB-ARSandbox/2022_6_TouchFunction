using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ASL;

public class ChangeColor : MonoBehaviour
{
    public PlayerASL playerASL;
    FlexibleColorPicker colorPicker;
    
    // Start is called before the first frame update
    void Awake()
    {
        colorPicker = GetComponent<FlexibleColorPicker>();
    }

    public void ChangePlayerColor()
    {
        // Debug.Log("color changed");
        // playerASL.GetComponentInChildren<MeshRenderer>().material.color = colorPicker.color;
        if(playerASL != null) 
            playerASL.SendColor(colorPicker.color);
    }
}
