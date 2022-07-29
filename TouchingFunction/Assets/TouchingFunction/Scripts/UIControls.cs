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
<<<<<<< HEAD
    public MirrorCamera MirrorCam;
=======
>>>>>>> main
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
<<<<<<< HEAD
=======
    public GameObject VKeyboardButton;
>>>>>>> main
    #endregion

    #region PlayerControls
    private PlayerInput playerInput;
    private InputAction summonUI;
<<<<<<< HEAD
=======
    private InputAction toggleUI;
>>>>>>> main
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

<<<<<<< HEAD
=======
        toggleUI = playerInput.PlayerControls.ToggleUI;
        toggleUI.performed += ToggleUI;
        toggleUI.Enable();

>>>>>>> main
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
<<<<<<< HEAD
        MirrorCam.StopMirrorCamera();
=======
>>>>>>> main

        // User controls
        ControlsPanel.SetActive(false);
        Controlbutton.SetActive(true);

        // Quit Window
        QuitWindowPanel.SetActive(false);
        QuitWindowButton.SetActive(true);
<<<<<<< HEAD
=======

        VKeyboardButton.SetActive(false);
>>>>>>> main
    }

    public void SetFunctionInputActive()
    {
        DisableBaseUI();
        FunctionInputPanel.SetActive(true);
<<<<<<< HEAD
=======
        VKeyboardButton.SetActive(true);
>>>>>>> main
        
    }

    public void SetGraphControlActive()
    {
        DisableBaseUI();
        GraphControlPanel.SetActive(true);
<<<<<<< HEAD
=======
        VKeyboardButton.SetActive(true);
>>>>>>> main
    }

    public void SetFlexibleColorPickerActive()
    {
        DisableBaseUI();
        FlexibleColorPickerPanel.SetActive(true);
        FaceChange.SetActive(true);
        CColor.player.isThinking = true;
        TargetSelector.SetActive(true);
<<<<<<< HEAD
        MirrorCam.StartMirrorCamera();
=======
        VKeyboardButton.SetActive(true);
>>>>>>> main
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
<<<<<<< HEAD
        if(VKeyboard.active)
        {
            VKeyboard.SetActive(false);
        }
        else{
            VKeyboard.SetActive(true);
        }
=======
        VKeyboard.GetComponent<VKeyboard>().TrackPlayer = !VKeyboard.GetComponent<VKeyboard>().TrackPlayer;
>>>>>>> main
 
    }

    private void TeleportUI(InputAction.CallbackContext obj)
    {
<<<<<<< HEAD
        Debug.Log("UI Summoned");
        gameObject.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 4.5f;
=======
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
>>>>>>> main
        if(gameObject.transform.position.y < 1)
        {
            Transform tempLoc = gameObject.transform;
            tempLoc.position = new Vector3(tempLoc.position.x, 1, tempLoc.position.z);
        }
<<<<<<< HEAD
    }

=======
        GetComponent<Canvas>().enabled = true;
    }

    private void ToggleUI(InputAction.CallbackContext obj)
    {
        GetComponent<Canvas>().enabled = !GetComponent<Canvas>().enabled;
    }
>>>>>>> main

}