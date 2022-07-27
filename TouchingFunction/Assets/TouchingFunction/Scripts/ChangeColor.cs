using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ASL;
using UnityEngine.UI;

public class ChangeColor : MonoBehaviour
{
    PlayerASL playerASL;
    public Player player;
    FlexibleColorPicker ColorPicker;
    public TMPro.TMP_Dropdown TargetDropdown;
    public Canvas canvas;
    public Button LeftButton;
    public Button RightButton;

    // 0:hair, 1:shirt, 2:pants, 3:skin
    public Color[] BodyColors;
    public Camera camera1;
    public Camera camera2;
    int targetSelected = 0;

    /*  index     attribute
     *  0       set to 3 all the time, float array identifier
        1       head material index
        2       hair color.r     
        3       hair color.g  
        4       hair color.b  
        5       hair color.a  
        6       shirt color.r  
        7       shirt color.g  
        8       shirt color.b  
        9       shirt color.a  
        10      pants color.r
        11      pants color.g
        12      pants color.b
        13      pants color.a
        14      skin color.r
        15      skin color.g
        16      skin color.b
        17      skin color.a        
        18      pigtail enabled
     */                                        
    float[] apperanceArray;


    public void SetPlayer(PlayerASL playerASL)
    {
        this.playerASL = playerASL;
    }
    // Start is called before the first frame update
    void Awake()
    {
        ColorPicker = GetComponent<FlexibleColorPicker>();
        apperanceArray = new float[]{ 3, 2, 
                                    BodyColors[1].r, BodyColors[0].g, BodyColors[0].b, BodyColors[0].a,
                                    BodyColors[1].r, BodyColors[1].g, BodyColors[1].b, BodyColors[1].a,
                                    BodyColors[2].r, BodyColors[2].g, BodyColors[2].b, BodyColors[2].a,
                                    BodyColors[3].r, BodyColors[3].g, BodyColors[3].b, BodyColors[3].a, 1    };
        ChangeColorTarget(0);
    }

    public void ChangePlayerColor()
    {
        if(playerASL != null)
        {
            Color c = ColorPicker.color;
            apperanceArray[4 * targetSelected + 2] = c.r;
            apperanceArray[4 * targetSelected + 3] = c.g;
            apperanceArray[4 * targetSelected + 4] = c.b;
            apperanceArray[4 * targetSelected + 5] = c.a;
            BodyColors[targetSelected] = c;
            playerASL.SendLook(apperanceArray);
        }
        
    }
    public void SetCamera(Camera cam, Player p)
    {
        camera1 = cam;
        player = p;
    }

    
    public void ChangeColorTarget(int targetIndex)
    {
        targetSelected = targetIndex;
        ColorPicker.SetColor(BodyColors[targetSelected]);
    }

    public void ChangeHeadMaterial(int matIndex)
    {
        apperanceArray[1] = matIndex;
        playerASL.SendLook(apperanceArray);
    }

    public void onLeftButtonClicked()
    {
        apperanceArray[1]--;
        if(apperanceArray[1] < 0)
        {
            apperanceArray[1] = 4;
        }
        playerASL.SendLook(apperanceArray);
    }

    public void onRightButtonClicked()
    {
        apperanceArray[1]++;
        if (apperanceArray[1] > 4)
        {
            apperanceArray[1] = 0;
        }
        playerASL.SendLook(apperanceArray);
    }

    public void TogglePigtail(bool isOn)
    {
        apperanceArray[18] = isOn ? 1 : -1;
        playerASL.SendLook(apperanceArray);
    }
}
