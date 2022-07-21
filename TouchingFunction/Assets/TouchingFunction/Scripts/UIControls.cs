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
    #endregion

    #region PlayerControls
    private PlayerInput playerInput;
    private InputAction summonUI;
    #endregion




    // Start is called before the first frame update
    void Start()
    {
        playerInput = new PlayerInput();
        // Delay needed to ensure all MeshGen have been instantiated before UI is reset
        Invoke("ResetUI", .1f);
        UISetup();
        Invoke("SetControlsActive", .11f);
    }

    void UISetup()
    {
        summonUI = playerInput.PlayerControls.SummonUI;
        summonUI.performed += TeleportUI;
        summonUI.Enable();

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
        FlexibleColorPickerButton.SetActive(true);

        // User controls
        ControlsPanel.SetActive(false);
        Controlbutton.SetActive(true);

        // Quit Window
        QuitWindowPanel.SetActive(false);
        QuitWindowButton.SetActive(true);
    }

    public void SetFunctionInputActive()
    {
        DisableBaseUI();
        FunctionInputPanel.SetActive(true);
        
    }

    public void SetGraphControlActive()
    {
        DisableBaseUI();
        GraphControlPanel.SetActive(true);
    }

    public void SetFlexibleColorPickerActive()
    {
        DisableBaseUI();
        FlexibleColorPickerPanel.SetActive(true);
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
        if(VKeyboard.active)
        {
            VKeyboard.SetActive(false);
        }
        else{
            VKeyboard.SetActive(true);
        }
 
    }

    private void TeleportUI(InputAction.CallbackContext obj)
    {
        Debug.Log("UI Summoned");
        gameObject.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 4.5f;
        if(gameObject.transform.position.y < 1)
        {
            Transform tempLoc = gameObject.transform;
            tempLoc.position = new Vector3(tempLoc.position.x, 1, tempLoc.position.z);
        }
    }


}