using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ASL;
using UnityEngine.UI;

public class ChangeColor : MonoBehaviour
{
    PlayerASL playerASL;
    public FlexibleColorPicker ColorPicker;
    public TMPro.TMP_Dropdown TargetDropdown;
    public Button EditButton;


    int targetSelected = 0;

    /*  index        attribute
        0       head material index
        1       hair color.r     
        2       hair color.g  
        3       hair color.b  
        4       hair color.a  
        5       shirt color.r  
        6       shirt color.g  
        7       shirt color.b  
        8       shirt color.a  
        9       pants color.r
        10      pants color.g
        11      pants color.b
        12      pants color.a
        13      skin color.r
        14      skin color.g
        15      skin color.b
        16      skin color.a        */                                        
    float[] apperanceArray;


    public void SetPlayer(PlayerASL playerASL)
    {
        this.playerASL = playerASL;
    }
    // Start is called before the first frame update
    void Awake()
    {
        //colorPicker = GetComponent<FlexibleColorPicker>();
    }

    public void ChangePlayerColor()
    {
        if(playerASL != null)
        {
            Color c = ColorPicker.color;
            apperanceArray[4 * targetSelected + 1] = c.r;
            apperanceArray[4 * targetSelected + 2] = c.g;
            apperanceArray[4 * targetSelected + 3] = c.b;
            apperanceArray[4 * targetSelected + 4] = c.a;

        }
            //playerASL.SendColor(ColorPicker.color);
    }

    // rotate camera to point to the player 
    public void PointCameraToSelf()
    {

    }
    

}
