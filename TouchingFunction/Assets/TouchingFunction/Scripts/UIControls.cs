using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.XR;

public class UIControls : MonoBehaviour
{
    #region Funciton Input
    public GameObject FunctionInputPanel;
    public GameObject FunctionInputButton;
    #endregion

    #region Graph Control
    public GameObject GraphControlPanel;
    public GameObject GraphControlButton;
    #endregion

    #region Flexible Color Picker
    public GameObject FlexibleColorPickerPanel;
    public GameObject FlexibleColorPickerButton;
    public GameObject TargetSelector;
    public ChangeColor CColor;
    public GameObject FaceChange;
    #endregion

    #region GraphList
    public GameObject FunctionInfo;
    public GameObject GraphList;
    #endregion

    #region 
    public GameObject ControlsPanel;
    public GameObject Controlbutton;
    #endregion

    #region QuitWindow
    public GameObject QuitWindowPanel;
    public GameObject QuitWindowButton;
    #endregion

    #region Virtual Keyboard
    public GameObject VKeyboard;
    public GameObject VKeyboardButton;
    #endregion

    #region PlayerControls
    private PlayerInput playerInput;
    private InputAction summonUI;
    private InputAction toggleUI;
    #endregion

    //public Canvas canvas;
    //public Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = new PlayerInput();
        // Delay needed to ensure all MeshGen have been instantiated before UI is reset
        Invoke("ResetUI", .1f);
        UISetup();
        Invoke("SetControlsActive", .11f);
        //canvas.worldCamera = camera;
    }

    void UISetup()
    {
        summonUI = playerInput.PlayerControls.SummonUI;
        summonUI.performed += TeleportUI;
        summonUI.Enable();

        toggleUI = playerInput.PlayerControls.ToggleUI;
        toggleUI.performed += ToggleUI;
        toggleUI.Enable();

    }



    // Update is called once per frame
    void Update()
    {
        gameObject.transform.rotation = Quaternion.LookRotation(gameObject.transform.position - Camera.main.transform.position, Vector3.up);
    }

    public void DisableBaseUI()
    {
        FunctionInputButton.SetActive(false);
        GraphControlButton.SetActive(false);
        FlexibleColorPickerButton.SetActive(false);
        Controlbutton.SetActive(false);
        QuitWindowButton.SetActive(false);
    }

    public void ResetUI()
    {
        // Function Input
        FunctionInputPanel.SetActive(false);
        FunctionInputButton.SetActive(true);

        // GraphControl
        GraphControlPanel.SetActive(false);
        GraphControlButton.SetActive(true);

        // Flexible Color Picker
        FlexibleColorPickerPanel.SetActive(false);
        TargetSelector.SetActive(false);
        FaceChange.SetActive(false);
        CColor.player.isThinking = false;
        FlexibleColorPickerButton.SetActive(true);

        // User controls
        ControlsPanel.SetActive(false);
        Controlbutton.SetActive(true);

        // Quit Window
        QuitWindowPanel.SetActive(false);
        QuitWindowButton.SetActive(true);

        VKeyboardButton.SetActive(false);
    }

    public void SetFunctionInputActive()
    {
        DisableBaseUI();
        FunctionInputPanel.SetActive(true);
        VKeyboardButton.SetActive(true);
        
    }

    public void SetGraphControlActive()
    {
        DisableBaseUI();
        GraphControlPanel.SetActive(true);
        VKeyboardButton.SetActive(true);
    }

    public void SetFlexibleColorPickerActive()
    {
        DisableBaseUI();
        FlexibleColorPickerPanel.SetActive(true);
        FaceChange.SetActive(true);
        CColor.player.isThinking = true;
        TargetSelector.SetActive(true);
        VKeyboardButton.SetActive(true);
    }
    public void SetControlsActive()
    {
        DisableBaseUI();
        ControlsPanel.SetActive(true);
    }

    public void SetQuitWindowActive()
    {
        DisableBaseUI();
        QuitWindowPanel.SetActive(true);
    }

    public void ToggleVirtualKeyboard()
    {
        VKeyboard.GetComponent<VKeyboard>().TrackPlayer = !VKeyboard.GetComponent<VKeyboard>().TrackPlayer;
 
    }

    private void TeleportUI(InputAction.CallbackContext obj)
    {
        GameObject player = GameObject.Find("PlayerPre(Clone)");
        float playerScale = 1;
        Debug.Log(player.transform.localScale.x);
        if(player.transform.localScale.x == 1)
        {
            playerScale = 1f;
        }
        else if (player.transform.localScale.x <= 2.5f)
        {
            playerScale = 1.1f;
        }
        else if (player.transform.localScale.x <= 4.5)
        {
            playerScale = 1.3f;
        }
        else if (player.transform.localScale.x <= 6.5f)
        {
            playerScale = 1.5f;
        }
        else 
        {
            playerScale = 2;
        }
        Debug.Log("UI Summoned");
        gameObject.transform.position = Camera.main.transform.position + Camera.main.transform.forward * (7f * playerScale);
        if(gameObject.transform.position.y < 1)
        {
            Transform tempLoc = gameObject.transform;
            tempLoc.position = new Vector3(tempLoc.position.x, 1, tempLoc.position.z);
        }
        GetComponent<Canvas>().enabled = true;
    }

    private void ToggleUI(InputAction.CallbackContext obj)
    {
        GetComponent<Canvas>().enabled = !GetComponent<Canvas>().enabled;
    }

}