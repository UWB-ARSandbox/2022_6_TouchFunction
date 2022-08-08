using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR.Interaction.Toolkit;
using ASL;

public class PlayerActivateVRHands : MonoBehaviour
{

    private PlayerInput playerInput;
    private InputAction vrLook;

    public GameObject LeftHand;
    public GameObject RightHand;
    public bool VRActive; 


    // Start is called before the first frame update
    void Awake()
    {
        playerInput = new PlayerInput();

    }

    // Update is called once per frame
    void OnEnable()
    {
        vrLook = playerInput.PlayerControls.VRLook;
        vrLook.performed += togVRHands;
        vrLook.Enable();

        
    }

    void Update()
    {
        if(VRActive)
        {
            LeftHand.GetComponent<XRRayInteractor>().maxRaycastDistance = 10f * transform.localScale.x;
            LeftHand.GetComponent<XRInteractorLineVisual>().lineWidth = .01f * transform.localScale.x;

            RightHand.GetComponent<XRRayInteractor>().maxRaycastDistance = 10f * transform.localScale.x;
            RightHand.GetComponent<XRInteractorLineVisual>().lineWidth = .01f * transform.localScale.x;
        }
    }

    private void togVRHands(InputAction.CallbackContext obj)
    {
        LeftHand.SetActive(true);
        RightHand.SetActive(true);

        VRActive = true;

        GameObject.Find("GraphAxes").GetComponent<GraphManipulation>().RightCon = RightHand;
        GameObject.Find("GraphAxes").GetComponent<GraphManipulation>().LeftCon = LeftHand;
    }

    public bool IsLeftHandOverUI()
    {
        return LeftHand.GetComponent<XRRayInteractor>().TryGetCurrentUIRaycastResult(out _);
    }

    public bool IsRightHandOverUI()
    {
        return RightHand.GetComponent<XRRayInteractor>().TryGetCurrentUIRaycastResult(out _);
    }



}
