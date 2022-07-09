//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.3.0
//     from Assets/TouchingFunction/Scripts/PlayerInput.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerInput : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInput"",
    ""maps"": [
        {
            ""name"": ""PlayerControls"",
            ""id"": ""5f5d5491-52ec-4f4a-92ea-a6fdc86a0d07"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""cf450b4c-f998-4562-905f-14fd37133971"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""af35b552-81d4-49a5-ad78-66970c750730"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Click"",
                    ""type"": ""Button"",
                    ""id"": ""087aa9f4-698c-4f46-a137-b1b82733f383"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Quit"",
                    ""type"": ""Button"",
                    ""id"": ""df5d1c5c-8c0c-42c6-8d7b-edf5214b5ea2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""EnableGravity"",
                    ""type"": ""Button"",
                    ""id"": ""c62400bd-75fd-4ac6-af50-073d26790530"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""DisableGravity"",
                    ""type"": ""Button"",
                    ""id"": ""7f13f536-e4ea-416a-87eb-883164bc1bb1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ToggleCursorLock"",
                    ""type"": ""Button"",
                    ""id"": ""0539639e-1670-41f3-9cf9-2513cbb1b296"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""e5011cc6-9d76-42ea-8530-d7b74961c1f8"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""VRLook"",
                    ""type"": ""Value"",
                    ""id"": ""f68ddda0-fea9-41ea-a356-b799f65fab69"",
                    ""expectedControlType"": ""Quaternion"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""VRLookCont"",
                    ""type"": ""Value"",
                    ""id"": ""a7ea5d70-929a-4589-9c70-98e6b109a6a5"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""3c1df8f2-00ce-4498-a5ba-cc01a30e73cc"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""12eb4ddf-9c0a-41ca-91d4-dba8993f0acd"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""9475c7f3-2122-43af-856a-70f8fc705183"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""591c4173-b28b-4c21-ac6b-345355dfc440"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""f3b91422-8972-41ed-aa1d-ca144ac6a293"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""e4fa9aab-a444-4401-8347-6bead04d30f8"",
                    ""path"": ""<XRController>{LeftHand}/joystick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""VR"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""875fc18b-e973-4cc1-941d-731dd52fe879"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1cd3ee25-f2c8-4918-b9c9-ec6dc5a156b4"",
                    ""path"": ""<XRController>{RightHand}/primaryButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""VR"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4b31cda6-6a6e-4efb-9b29-184cc73c8e6d"",
                    ""path"": ""<XRController>{RightHand}/touchpadClicked"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""VR"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""42140ba1-560d-4fb6-a5ae-5fa8f51ff2a3"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4ad20b07-d10b-4123-88ef-d30d5980f3cb"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Quit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6d09a05f-50ce-4320-8598-c972ed74ac43"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""EnableGravity"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""eeb7afe4-c071-4d2a-9020-a3f076cc251b"",
                    ""path"": ""<XRController>{LeftHand}/primaryButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""VR"",
                    ""action"": ""EnableGravity"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""28d243bc-252d-4c77-9a35-eb9d89ecff07"",
                    ""path"": ""<XRController>{LeftHand}/touchpadClicked"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""VR"",
                    ""action"": ""EnableGravity"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""dd12fbc3-c318-4339-9514-130893f8cbca"",
                    ""path"": ""<Keyboard>/g"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""DisableGravity"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9ba9a152-4885-45ff-aaea-7b50087449aa"",
                    ""path"": ""<XRController>{LeftHand}/secondaryButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""VR"",
                    ""action"": ""DisableGravity"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c5ad06cc-c6f0-4753-bf0d-452244836a68"",
                    ""path"": ""<Keyboard>/leftCtrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""ToggleCursorLock"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cce191ea-5648-4751-82be-e8891c50aeaa"",
                    ""path"": ""<WMRSpatialController>{RightHand}/joystickClicked"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""VR"",
                    ""action"": ""ToggleCursorLock"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2f92ab67-e85d-447f-8ac4-1d3e34dec7fb"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c49df9df-9d6d-45f8-840c-6b304d6d0b8a"",
                    ""path"": ""<XRHMD>/centerEyeRotation"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""VR"",
                    ""action"": ""VRLook"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6bc378ad-2ad6-4718-aa4e-fac89b40adf6"",
                    ""path"": ""<XRController>{RightHand}/joystick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""VR"",
                    ""action"": ""VRLookCont"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""PC"",
            ""bindingGroup"": ""PC"",
            ""devices"": []
        },
        {
            ""name"": ""VR"",
            ""bindingGroup"": ""VR"",
            ""devices"": [
                {
                    ""devicePath"": ""<XRHMD>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<XRController>{LeftHand}"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<XRController>{RightHand}"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // PlayerControls
        m_PlayerControls = asset.FindActionMap("PlayerControls", throwIfNotFound: true);
        m_PlayerControls_Movement = m_PlayerControls.FindAction("Movement", throwIfNotFound: true);
        m_PlayerControls_Jump = m_PlayerControls.FindAction("Jump", throwIfNotFound: true);
        m_PlayerControls_Click = m_PlayerControls.FindAction("Click", throwIfNotFound: true);
        m_PlayerControls_Quit = m_PlayerControls.FindAction("Quit", throwIfNotFound: true);
        m_PlayerControls_EnableGravity = m_PlayerControls.FindAction("EnableGravity", throwIfNotFound: true);
        m_PlayerControls_DisableGravity = m_PlayerControls.FindAction("DisableGravity", throwIfNotFound: true);
        m_PlayerControls_ToggleCursorLock = m_PlayerControls.FindAction("ToggleCursorLock", throwIfNotFound: true);
        m_PlayerControls_Look = m_PlayerControls.FindAction("Look", throwIfNotFound: true);
        m_PlayerControls_VRLook = m_PlayerControls.FindAction("VRLook", throwIfNotFound: true);
        m_PlayerControls_VRLookCont = m_PlayerControls.FindAction("VRLookCont", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // PlayerControls
    private readonly InputActionMap m_PlayerControls;
    private IPlayerControlsActions m_PlayerControlsActionsCallbackInterface;
    private readonly InputAction m_PlayerControls_Movement;
    private readonly InputAction m_PlayerControls_Jump;
    private readonly InputAction m_PlayerControls_Click;
    private readonly InputAction m_PlayerControls_Quit;
    private readonly InputAction m_PlayerControls_EnableGravity;
    private readonly InputAction m_PlayerControls_DisableGravity;
    private readonly InputAction m_PlayerControls_ToggleCursorLock;
    private readonly InputAction m_PlayerControls_Look;
    private readonly InputAction m_PlayerControls_VRLook;
    private readonly InputAction m_PlayerControls_VRLookCont;
    public struct PlayerControlsActions
    {
        private @PlayerInput m_Wrapper;
        public PlayerControlsActions(@PlayerInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_PlayerControls_Movement;
        public InputAction @Jump => m_Wrapper.m_PlayerControls_Jump;
        public InputAction @Click => m_Wrapper.m_PlayerControls_Click;
        public InputAction @Quit => m_Wrapper.m_PlayerControls_Quit;
        public InputAction @EnableGravity => m_Wrapper.m_PlayerControls_EnableGravity;
        public InputAction @DisableGravity => m_Wrapper.m_PlayerControls_DisableGravity;
        public InputAction @ToggleCursorLock => m_Wrapper.m_PlayerControls_ToggleCursorLock;
        public InputAction @Look => m_Wrapper.m_PlayerControls_Look;
        public InputAction @VRLook => m_Wrapper.m_PlayerControls_VRLook;
        public InputAction @VRLookCont => m_Wrapper.m_PlayerControls_VRLookCont;
        public InputActionMap Get() { return m_Wrapper.m_PlayerControls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerControlsActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerControlsActions instance)
        {
            if (m_Wrapper.m_PlayerControlsActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnMovement;
                @Jump.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnJump;
                @Click.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnClick;
                @Click.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnClick;
                @Click.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnClick;
                @Quit.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnQuit;
                @Quit.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnQuit;
                @Quit.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnQuit;
                @EnableGravity.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnEnableGravity;
                @EnableGravity.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnEnableGravity;
                @EnableGravity.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnEnableGravity;
                @DisableGravity.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnDisableGravity;
                @DisableGravity.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnDisableGravity;
                @DisableGravity.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnDisableGravity;
                @ToggleCursorLock.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnToggleCursorLock;
                @ToggleCursorLock.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnToggleCursorLock;
                @ToggleCursorLock.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnToggleCursorLock;
                @Look.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnLook;
                @Look.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnLook;
                @Look.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnLook;
                @VRLook.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnVRLook;
                @VRLook.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnVRLook;
                @VRLook.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnVRLook;
                @VRLookCont.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnVRLookCont;
                @VRLookCont.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnVRLookCont;
                @VRLookCont.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnVRLookCont;
            }
            m_Wrapper.m_PlayerControlsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Click.started += instance.OnClick;
                @Click.performed += instance.OnClick;
                @Click.canceled += instance.OnClick;
                @Quit.started += instance.OnQuit;
                @Quit.performed += instance.OnQuit;
                @Quit.canceled += instance.OnQuit;
                @EnableGravity.started += instance.OnEnableGravity;
                @EnableGravity.performed += instance.OnEnableGravity;
                @EnableGravity.canceled += instance.OnEnableGravity;
                @DisableGravity.started += instance.OnDisableGravity;
                @DisableGravity.performed += instance.OnDisableGravity;
                @DisableGravity.canceled += instance.OnDisableGravity;
                @ToggleCursorLock.started += instance.OnToggleCursorLock;
                @ToggleCursorLock.performed += instance.OnToggleCursorLock;
                @ToggleCursorLock.canceled += instance.OnToggleCursorLock;
                @Look.started += instance.OnLook;
                @Look.performed += instance.OnLook;
                @Look.canceled += instance.OnLook;
                @VRLook.started += instance.OnVRLook;
                @VRLook.performed += instance.OnVRLook;
                @VRLook.canceled += instance.OnVRLook;
                @VRLookCont.started += instance.OnVRLookCont;
                @VRLookCont.performed += instance.OnVRLookCont;
                @VRLookCont.canceled += instance.OnVRLookCont;
            }
        }
    }
    public PlayerControlsActions @PlayerControls => new PlayerControlsActions(this);
    private int m_PCSchemeIndex = -1;
    public InputControlScheme PCScheme
    {
        get
        {
            if (m_PCSchemeIndex == -1) m_PCSchemeIndex = asset.FindControlSchemeIndex("PC");
            return asset.controlSchemes[m_PCSchemeIndex];
        }
    }
    private int m_VRSchemeIndex = -1;
    public InputControlScheme VRScheme
    {
        get
        {
            if (m_VRSchemeIndex == -1) m_VRSchemeIndex = asset.FindControlSchemeIndex("VR");
            return asset.controlSchemes[m_VRSchemeIndex];
        }
    }
    public interface IPlayerControlsActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnClick(InputAction.CallbackContext context);
        void OnQuit(InputAction.CallbackContext context);
        void OnEnableGravity(InputAction.CallbackContext context);
        void OnDisableGravity(InputAction.CallbackContext context);
        void OnToggleCursorLock(InputAction.CallbackContext context);
        void OnLook(InputAction.CallbackContext context);
        void OnVRLook(InputAction.CallbackContext context);
        void OnVRLookCont(InputAction.CallbackContext context);
    }
}
