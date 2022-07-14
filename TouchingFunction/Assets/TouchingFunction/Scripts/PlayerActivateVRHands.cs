using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.XR;
using ASL;

public class PlayerActivateVRHands : MonoBehaviour
{

    private PlayerInput playerInput;
    private InputAction vrLook;

    public GameObject LeftHand;
    public GameObject RightHand;


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

    private void togVRHands(InputAction.CallbackContext obj)
    {
        LeftHand.SetActive(true);
        RightHand.SetActive(true);
    }


}
