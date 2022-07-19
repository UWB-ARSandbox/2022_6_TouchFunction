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

    #region PlayerControls
    private PlayerInput playerInput;
    private InputAction summonUI;
    private InputAction rightTrigger;

    private LayerMask layerMask;
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

        layerMask = LayerMask.GetMask("UI");

        // rightTrigger = playerInput.PlayerControls.RightTrigger;
        // rightTrigger.performed += RightVRclick;
        // rightTrigger.Enable();
        rightTrigger = new InputAction(binding: "<XRController>{RightHand}/triggerPressed");
        rightTrigger.performed += RightVRclick;
        rightTrigger.Enable();
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

    private void TeleportUI(InputAction.CallbackContext obj)
    {
        Debug.Log("UI Summoned");
        gameObject.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 6;
        if(gameObject.transform.position.y < 1)
        {
            Transform tempLoc = gameObject.transform;
            tempLoc.position = new Vector3(tempLoc.position.x, 1, tempLoc.position.z);
        }
    }

    private void RightVRclick(InputAction.CallbackContext obj)
    {
        Debug.Log("In trigger");
        //test code

        GameObject rightCon = GameObject.Find("RightHandController");
        Debug.Log(rightCon != null);
        RaycastHit hit;
        if(Physics.Raycast(rightCon.transform.position, rightCon.transform.TransformDirection(Vector3.forward), out hit, 10, layerMask))
        {
            Debug.Log("Raycast successful");
            //tryHit(hit);
            if(hit.collider.gameObject != null)
            {
                Debug.Log("Object found");
                IPointerClickHandler clickHandler = hit.collider.gameObject.GetComponent<IPointerClickHandler>();
                PointerEventData ped = new PointerEventData(EventSystem.current);
                clickHandler.OnPointerClick(ped);
            }
        }
    }
}