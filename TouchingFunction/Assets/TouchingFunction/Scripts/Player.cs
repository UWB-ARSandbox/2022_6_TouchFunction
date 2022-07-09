using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.XR;
using ASL;

public partial class Player : MonoBehaviour
{
    #region Player movement variables
    public float speed = 5.0f;
    public float jumpSpeed = 10.0f;
    public float gravity = 10.0f;
    public int onPlatform = -1;
    public float verticalSpeed = 0f;
    public bool inAir = false;
    MeshCreator currentStandingMesh;
    public MeshManager m_MeshManager;
    #endregion

    #region Player animation
    PlayerAnimation playerAnimation;

    bool playerMoving = false;
    bool gravityEnabled = true;
    # endregion

    private CharacterController controller;
    private PlayerInput playerInput;
    private InputAction movement;
    private InputAction jump;
    private InputAction toggleCursorLock;
    private InputAction disableGravity;
    private InputAction enableGravity;
    private InputAction look;
    private InputAction vrLook;
    private InputAction vrLookCon;
    private Vector3 velocity;

    private bool gravityFall;
    private bool gravityRise;
    private bool mouseLook;
    private bool vrLookB;



    // Start is called before the first frame update
    void Start()
    {
        LockCursor();
        velocity = new Vector3(0, 0, 0);
        gravityFall = false;
        gravityRise = false;
    }

    void Awake()
    {
        playerInput = new PlayerInput();
        controller = gameObject.GetComponent<CharacterController>();
        Debug.Assert(controller != null);

        playerAnimation = GetComponent<PlayerAnimation>();
    }

    private void OnEnable()
    {
        //position
        movement = playerInput.PlayerControls.Movement;
        movement.Enable();

        // jump
        jump = playerInput.PlayerControls.Jump;
        jump.started += StartJump;
        jump.canceled += EndJump;
        jump.Enable();

        //cursor Lock
        toggleCursorLock = playerInput.PlayerControls.ToggleCursorLock;
        toggleCursorLock.performed += TogCursorLock;
        toggleCursorLock.Enable();

        //gravity
        disableGravity = playerInput.PlayerControls.DisableGravity;
        disableGravity.performed += DisableGravity;
        disableGravity.Enable();

        enableGravity = playerInput.PlayerControls.EnableGravity;
        enableGravity.started += BeginEnableGravity;
        enableGravity.canceled += EndEnableGravity;
        enableGravity.Enable();

        // camera rotation
        look = playerInput.PlayerControls.Look;
        look.performed += togMouseLook;
        look.Enable();
        vrLook = playerInput.PlayerControls.VRLook;
        vrLook.performed += togVRLook;
        vrLook.Enable();

    }

    private void togMouseLook(InputAction.CallbackContext obj)
    {
        mouseLook = true;
    }

    private void togVRLook(InputAction.CallbackContext obj)
    {
        vrLookB = true;
    }


    void LateUpdate()
    {
        if (IsCursorLocked())
        {
            if (vrLookB)
            {
                //RotateCameraMouse();
                RotateCameraVR();
            }
            else
            {
                RotateCameraMouse();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (gravityFall)
        {
            EnableGravity();
        }
        else if (gravityRise)
        {
            DoJump();
        }

        Vector3 currPos = transform.position;

        controller.Move(velocity * Time.deltaTime);
        
        if (currPos.y <= 0 || onPlatform >= 0)
        {
            inAir = false;
        }
        else
        {
            inAir = true;
        }

        if (IsCursorLocked())
        {
            MovePlayer();

        }
        //if gravity enabled, drag player to platform
        if (gravityEnabled && inAir)
        {
            velocity.y -= gravity * Time.deltaTime;
        }
        else
        {
            if (velocity.y > 0)
            {
                velocity.y -= gravity * Time.deltaTime;
            }
            else if (velocity.y < 0)
            {
                velocity.y += gravity * Time.deltaTime;
            }
            if (velocity.y < .05f && velocity.y > -.05f)
            {
                velocity.y = 0;
            }
        }

        // in the case when the player accidentally clip through the floor, reset y to 0
        if (currPos.y < 0)
        {
            velocity.y = 0;
            transform.position = new Vector3(currPos.x, 0, currPos.z);
        }
    }

    void MovePlayer()
    {

        Vector2 moveValue = movement.ReadValue<Vector2>();
        Vector3 move = transform.right * moveValue.x + transform.forward * moveValue.y;

        controller.Move(move * speed * Time.deltaTime);

        if (move.z != 0 || move.x != 0) // movement in x or z axis
        {
            playerMoving = true;
        }
        else
        {
            playerMoving = false;
        }

        if (onPlatform >= 0)
        {
            MovePositionOnGraph();
        }
    }

    public bool IsFlappingEnabled()
    {
        return inAir && !gravityEnabled;
    }

    public bool IsWalkingEnabled()
    {
        return !IsFlappingEnabled() && playerMoving;
    }

    // Toggle cursor lock
    private void TogCursorLock(InputAction.CallbackContext obj)
    {
        ToggleCursorLock();
    }

    // disable gravity
    private void DisableGravity(InputAction.CallbackContext obj)
    {
        if (IsCursorLocked())
        {
            gravityEnabled = !gravityEnabled;
            velocity.y = 0;
        }
    }

    // drop flying player drown (leaves gravity turned off unless hits ground)
    private void EnableGravity()
    {
        if (IsCursorLocked())
        {

            if (inAir && !gravityEnabled)
            {
                velocity.y = -Mathf.Sqrt(jumpSpeed * 2f * (gravity)) / 2;
            }
        }
    }

    private void BeginEnableGravity(InputAction.CallbackContext obj)
    {
        if (IsCursorLocked())
        {
            gravityFall = true;
        }
    }
    private void EndEnableGravity(InputAction.CallbackContext obj)
    {
        if (IsCursorLocked())
        {
            gravityFall = false;
        }
    }

    // jump. if player gravity = false, moves 
    private void StartJump(InputAction.CallbackContext obj)
    {
        if (IsCursorLocked())
        {
            // if not in air and gravity enabled, normal jump
            if (!inAir && gravityEnabled)
            {
                velocity.y = Mathf.Sqrt(jumpSpeed * 2f * (gravity));
            }//else enter gravity jump
            else if (!gravityEnabled)
            {
                gravityRise = true;

            }
        }

    }

    private void DoJump()
    {
        if (IsCursorLocked())
        {
            velocity.y = Mathf.Sqrt(jumpSpeed * 2f * (gravity)) / 2;
        }
    }

    private void EndJump(InputAction.CallbackContext obj)
    {
        gravityRise = false;
    }

    // change Y value when player is moving on a graph, the Y value will be based on the resolution (MeshPerX)
    private void MovePositionOnGraph()
    {
        float xVal = transform.position.x - currentStandingMesh.origin.x;
        int wn = (int)Mathf.Floor(xVal / currentStandingMesh.MeshPerX);
        int snapXVal = wn + (int)Mathf.Floor((xVal - wn) * currentStandingMesh.MeshPerX);
        float newY = currentStandingMesh.origin.y + currentStandingMesh.yVals[snapXVal] + .5f;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        Debug.Log("snapX: " + snapXVal);
        Debug.Log("Y: " + currentStandingMesh.yVals[snapXVal]);
    }

    // Mount the platform currently standing on to currentStandingMesh
    public void mountMesh(int meshIndex)
    {
        currentStandingMesh = m_MeshManager.meshes[meshIndex];
        //Debug.Log("Index mount: " + meshIndex);
    }


    public void land()
    {
        inAir = false;
        velocity = Vector3.zero;
        Debug.Log("landed!!!!!!!!!");
    }

    public bool isFalling()
    {
        return velocity.y < 0;
    }
}


