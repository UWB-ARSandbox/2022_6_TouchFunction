using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.XR;

public partial class Player : MonoBehaviour
{
    #region Player movement variables
    public float speed = 5.0f;
    public float jumpSpeed = 10.0f;
    public float SlideSpeed = 0.1f;
    public float gravity = 10.0f;
    public int onPlatform = -1;
    public float verticalSpeed = 0f;
    public float SlideLimit = 45f;
    public Vector3 SlidingVector;
    
    public Vector3 CollidePosition;

    private float scalingFactor = 0.2f;
    private float positionChangeFactor = 0.3f;
    public bool scalingUp = false;
    public bool scalingDown = false;

    #endregion

    #region Player animation
    //PlayerAnimation playerAnimation;

    
    

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
    private InputAction scaleUp;
    private InputAction scaleDown;
    public Vector3 velocity;

    private bool gravityFall;
    private bool gravityRise;
    private bool mouseLook;
    private bool vrLookB;

    public PlayerASL playerASL;

    #region Animator Booleans
    public bool isSliding = false;
    public bool isMoving = false;
    public bool isFalling = false;
    public bool isFlying = false;
    public bool isThinking = false;
    #endregion
    Animator PlayerAnimator;
    

    // Start is called before the first frame update
    void Start()
    {
        LockCursor();
        velocity = Vector3.zero;
        gravityFall = false;
        gravityRise = false;
    }

    void Awake()
    {
        playerInput = new PlayerInput();
        controller = gameObject.GetComponent<CharacterController>();
        Debug.Assert(controller != null);
        PlayerAnimator = GetComponentInChildren<Animator>();

        //playerAnimation = GetComponent<PlayerAnimation>();

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
        disableGravity.performed += TogGravity;
        disableGravity.Enable();

        enableGravity = playerInput.PlayerControls.EnableGravity;
        enableGravity.started += StartFall;
        enableGravity.canceled += EndFall;
        enableGravity.Enable();

        // camera rotation
        look = playerInput.PlayerControls.Look;
        look.performed += togMouseLook;
        look.Enable();
        vrLook = playerInput.PlayerControls.VRLook;
        vrLook.performed += togVRLook;
        vrLook.Enable();

        // scale up player's size
        scaleUp = playerInput.PlayerControls.ScaleUp;
        scaleUp.started += BeginScalingUp;
        scaleUp.canceled += EndScalingUp;
        scaleUp.Enable();

        // scale down player's size
        scaleDown = playerInput.PlayerControls.ScaleDown;
        scaleDown.started += BeginScalingDown;
        scaleDown.canceled += EndScalingDown;
        scaleDown.Enable();

        // script to check for VR input to activate VR hands
        gameObject.GetComponent<PlayerActivateVRHands>().enabled = true;

        // Script to attatch PlayerCamera to Player as a child of Head
        GameObject tmpCam = GameObject.Find("PlayerCamera");
        tmpCam.transform.parent = head;
        tmpCam.transform.position = head.position;//new Vector3(0,2f,0);
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

        setAnimatorBool();

        /*if (controller.isGrounded)
        {
            Debug.Log("IS GROUNDED");
        }
        else
        {
            Debug.Log("IS FLYING");
        }*/


        if (gravityFall)
        {
            DoFall();
        }
        else if (gravityRise)
        {
            DoJump();
        }

        Vector3 currPos = transform.position;

        if (IsCursorLocked())
        {
            horizontalMove();
            //MovePlayer();
        }
        MovePlayer();
        /*
        if (checkFloorNormal() != Vector3.zero)
        {
            Debug.Log(checkFloorNormal());
            velocity += checkFloorNormal();
            velocity += Vector3.down / 2;
        }
        else
        {
            
        }*/

        // give velocity.y a little bit of downward force to make player stick to ground (Required by CharacterController)
        if (controller.isGrounded)
        {
            velocity.y -= 0.05f;
            //PlayerAnimator.SetBool("IsFalling", false);
            isFalling = false;
        } else
        {
            isSliding = false;
        }

        //if gravity enabled, drag player to platform
        if (gravityEnabled)//&& !controller.isGrounded)
        {
            velocity.y -= (gravity * Time.deltaTime) * .75f;
            if (velocity.y < 0 && !controller.isGrounded)
            {
                //PlayerAnimator.SetBool("IsFalling", true);
                isFalling = true;
            }
        }
        else
        {
            isFalling = false;
            //PlayerAnimator.SetBool("IsFalling", false);
            if (velocity.y > 0)
            {
                // Add negative acceleration to drop player
                velocity.y -= gravity * Time.deltaTime * .75f;
            }
            else if (velocity.y < 0)
            {
                // Add positive acceleration to rise player
                velocity.y += gravity * Time.deltaTime * .75f;
            }
            // if y velocity is very small, cancel it. Done to prevent velocity from bouncing positive and negative when player stationary. 
            if (velocity.y < .05f && velocity.y > -.05f)
            {
                velocity.y = 0;
            }
        }

        // in the case when the player accidentally clip through the floor, reset y to 0
        if (currPos.y < -20)
        {
            velocity.y = 0;
            transform.position = new Vector3(currPos.x, 0, currPos.z);
        }

        // Scaling player
        if (scalingUp)
        {
            ScalePlayerUp();
        }

        if (scalingDown)
        {
            ScalePlayerDown();
        }

        /*else if (isSliding)
        {
            PlayerAnimator.Play("Sliding");
        } else if (IsFlappingEnabled())
        {
            PlayerAnimator.Play("Flapping");
        }*/


    }

    void horizontalMove()
    {
        Vector2 moveValue = movement.ReadValue<Vector2>();
        //Vector3 move = transform.right * (moveValue.x * 1.25f) + transform.up * velocity.y + transform.forward * (moveValue.y * 1.25f);
        velocity += transform.right * moveValue.x * 1.5f + transform.forward * moveValue.y * 1.5f;
        if (moveValue != Vector2.zero)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
        /*if (isMoving)
        {
            isMoving = true;
            PlayerAnimator.SetBool("IsWalking", true);
        }
        else
        {
            isMoving = false;
            PlayerAnimator.SetBool("IsWalking", false);
        }*/
    }

    void MovePlayer()
    {
        //Vector2 moveValue = movement.ReadValue<Vector2>();
        //Vector3 move = transform.right * (moveValue.x * 1.25f) + transform.up * velocity.y + transform.forward * (moveValue.y * 1.25f);


        Vector3 finalMoveVector = velocity * speed * Time.deltaTime;

        //checkFloorNormal();

        //Debug.Log("Normal Vector : " + norm);
        if (isSliding)
        {

            finalMoveVector += SlidingVector * SlideSpeed;
        }

        controller.Move(finalMoveVector);

        if (controller.isGrounded)
        {
            velocity = Vector3.zero;
        }
        else
        {
            velocity = new Vector3(0, velocity.y, 0);
        }

    }

    // Flapping functions ============================================================================
    public bool IsFlappingEnabled()
    {
        //return !controller.isGrounded && !gravityEnabled;
        return !gravityEnabled;
    }

    public bool IsWalkingEnabled()
    {
        return !IsFlappingEnabled() && isMoving;
    }


    // Toggle cursor lock ========================================================================
    private void TogCursorLock(InputAction.CallbackContext obj)
    {
        ToggleCursorLock();
    }

    // disable gravity ===========================================================================
    private void TogGravity(InputAction.CallbackContext obj)
    {
        if (IsCursorLocked())
        {
            gravityEnabled = !gravityEnabled;
            velocity.y = 0;
        }
        if (!gravityEnabled)
        {
            isFlying = true;
            //PlayerAnimator.SetBool("IsFlapping", true);
        }
        else
        {
            isFlying = false;
            //PlayerAnimator.SetBool("IsFlapping", false);
        }
    }

    // Falling functions =============================================================================
    // drop flying player drown (leaves gravity turned off unless hits ground)
    private void DoFall()
    {
        if (IsCursorLocked())
        {

            if (!controller.isGrounded && !gravityEnabled)
            {
                velocity.y = -Mathf.Sqrt(jumpSpeed * (gravity)) * .4f;
            }
        }
    }

    private void StartFall(InputAction.CallbackContext obj)
    {
        if (IsCursorLocked())
        {
            gravityFall = true;
        }
    }
    private void EndFall(InputAction.CallbackContext obj)
    {
        if (IsCursorLocked())
        {
            gravityFall = false;
        }
    }

    // Rising functions ===============================================================================
    // jump. if player gravity = false, moves 
    private void StartJump(InputAction.CallbackContext obj)
    {
        if (IsCursorLocked())
        {
            Debug.Log("Jump!");
            // if not in air and gravity enabled, normal jump
            if (controller.isGrounded && gravityEnabled)
            {
                velocity.y = Mathf.Sqrt(jumpSpeed * (gravity)) * .75f;
                Debug.Log("After jump Velocity : " + velocity);
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
            velocity.y = Mathf.Sqrt(jumpSpeed * (gravity)) * .4f;
        }
    }

    private void EndJump(InputAction.CallbackContext obj)
    {
        gravityRise = false;
    }

    // Scaling funcitons ====================================================================
    private void BeginScalingUp(InputAction.CallbackContext obj)
    {
        scalingUp = true;
    }

    private void EndScalingUp(InputAction.CallbackContext obj)
    {
        scalingUp = false;
    }
    private void ScalePlayerUp()
    {
        if (IsCursorLocked())
        {
            Vector3 scalingVector = new Vector3(scalingFactor, scalingFactor, scalingFactor);
            transform.localScale += scalingVector;

            // Vector3 back = transform.TransformDirection(Vector3.back);
            transform.position += positionChangeFactor * -transform.forward;
        }
    }

    private void BeginScalingDown(InputAction.CallbackContext obj)
    {
        scalingDown = true;
    }

    private void EndScalingDown(InputAction.CallbackContext obj)
    {
        scalingDown = false;
    }

    private void ScalePlayerDown()
    {
        if (IsCursorLocked())
        {
            if (transform.localScale.y < 1.0f)
            {
                return; // Not allowed to be smaller
            }

            Vector3 scalingVector = new Vector3(-scalingFactor, -scalingFactor, -scalingFactor);
            transform.localScale += scalingVector;

            // Vector3 forward = transform.TransformDirection(Vector3.forward);
            transform.position += positionChangeFactor * transform.forward;
        }
    }

    // When standing on a slope, calculate the sliding Vector3 and store into the global variables
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //CollidePosition = hit.transform.position;
        //Debug.Log("Controller hit Normal: " + hit.normal);
        //Debug.Log("Controller hit position: " + hit.transform.position);
        if (controller.isGrounded)
        {
            Vector3 norm = hit.normal;

            //Debug.Log(slopeHit.normal);
            if (Vector3.Angle(norm, Vector3.up) > SlideLimit)   // if on a slope steeper than limit
            {
                isSliding = true;
                //PlayerAnimator.SetBool("IsSliding", true);
                SlidingVector = norm + Mathf.Sqrt(1 + (norm.x / norm.y) * (norm.x / norm.y)) * Vector3.down * Mathf.Abs(norm.x/norm.y);
            }
            else
            {
                isSliding = false;
                //PlayerAnimator.SetBool("IsSliding", false);
                SlidingVector = Vector3.zero;
            }
        } // if grounded but no RaycastHit, it means the slope is too steep for ray to detect
    }

    public void setAnimatorBool()
    {
        PlayerAnimator.SetBool("IsSliding", isSliding);
        PlayerAnimator.SetBool("IsWalking", isMoving);
        PlayerAnimator.SetBool("IsFalling", isFalling);
        PlayerAnimator.SetBool("IsFlapping", isFlying);
        PlayerAnimator.SetBool("IsThinking", isThinking);
    }



}


