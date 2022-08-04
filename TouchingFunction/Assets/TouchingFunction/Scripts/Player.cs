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
    public float SlideLimit;
    public Vector3 SlidingVector;
    public Vector3 newSlidingVec;
    public Vector3 inheritedSliding;
    public float friction = 1f;
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
    
    private InputAction ride;
    private InputAction driveFwd;
    private InputAction driveBkwd;

    public Vector3 velocity;

    private bool gravityFall;
    private bool gravityRise;
    private bool mouseLook;
    private bool vrLookB;

    public PlayerASL playerASL;

    #region Animator Booleans
    public Animator PlayerAnimator;
    public bool isSliding = false;
    public bool isMoving = false;
    public bool isFalling = false;
    public bool isFlying = false;
    public bool isThinking = false;
    public bool isRiding = false;
    public bool isGrounded;
    #endregion
    

    #region Vehicle
    public GameObject EnterVehicleGUI;
    public RollerCoasterControl RCControlToRide;
    public RollerCoasterControl RCControlRiding;
    bool isForward;
    bool isBackward;
    public Canvas canvas;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        LockCursor();
        velocity = Vector3.zero;
        SlideLimit = 15f;
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

        // enter roller coaster
        ride = playerInput.PlayerControls.Ride;
        ride.performed += EnterExitVehicle;
        ride.Enable();

/*        driveFwd = playerInput.PlayerControls.DriveForward;
        driveFwd.started += carForward;
        driveFwd.canceled += carStop;
        driveFwd.Disable();

        driveBkwd = playerInput.PlayerControls.DriveBackward;
        driveBkwd.started += carBackward;
        driveBkwd.canceled += carStop;
        driveBkwd.Disable();*/

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

        /*// setting the transform of player to align with roller coaster
        if (isRiding)
        {
            controller.Move(RCControlRiding.FinalMoveVector);
            transform.forward = RCControlRiding.transform.forward;
        }*/

        setAnimatorBool();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = controller.isGrounded;

        if (isRiding)
        {
            /*if (RCControlRiding.IsDriver(this))
            {
                //Debug.LogError("!!!!!!!");
                if (Input.GetKey(KeyCode.W))
                {
                    
                    RCControlRiding.DriverMoveVector = RCControlRiding.transform.forward * 0.1f;
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    RCControlRiding.DriverMoveVector = RCControlRiding.transform.forward * -0.1f;
                }
            }*/
            if (RCControlRiding.IsDriver(this))
            {
                Vector2 driverMove = movement.ReadValue<Vector2>();
                RCControlRiding.DriverSpeedVector = RCControlRiding.transform.forward * driverMove.y * 0.1f;
                RCControlRiding.transform.RotateAround(RCControlRiding.transform.position, Vector3.up, 3*driverMove.x);

                /*if (isForward)
                {
                    RCControlRiding.DriverSpeedVector = RCControlRiding.transform.forward * 0.1f;
                }
                else if (isBackward)
                {
                    RCControlRiding.DriverSpeedVector = RCControlRiding.transform.forward * -0.1f;
                } */
            }


            return;
        }

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
            //clearSliding();
            //newSlidingVec = Vector3.zero;
        }

        //if gravity enabled, drag player to platform
        if (gravityEnabled)//&& !controller.isGrounded)
        {
            velocity.y -= (gravity * Time.deltaTime) * .75f;
            if (!isSliding)
            {
                if (velocity.y < 0 && !controller.isGrounded)
                {
                    //PlayerAnimator.SetBool("IsFalling", true);
                    isFalling = true;
                }
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
        //Debug.LogError("START JUMP");
        if (IsCursorLocked())
        {
            //Debug.Log("Jump!");
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
        //Debug.LogError("HIT!!");
        if (controller.isGrounded)
        {
            Vector3 norm = hit.normal;

            //Debug.Log(slopeHit.normal);
            if (hit.gameObject.layer == 11)//Vector3.Angle(norm, Vector3.up) > SlideLimit)   // if on a slope steeper than limit
            {
                isSliding = true;
                //PlayerAnimator.SetBool("IsSliding", true);
                newSlidingVec = (norm + Mathf.Sqrt(1 + (norm.x / norm.y) * (norm.x / norm.y)) * Vector3.down) * 0.005f;
                //Debug.Log("DOT PRODUCT : " + Vector3.Dot(newSlidingVec, SlidingVector));
                inheritedSliding = newSlidingVec.normalized * SlidingVector.magnitude * Vector3.Dot(newSlidingVec.normalized, SlidingVector.normalized);
                SlidingVector = newSlidingVec + inheritedSliding;
            }
            else
            {

                isSliding = false;
                clearSliding();
            }
        }
    }

    public void setAnimatorBool()
    {
        PlayerAnimator.SetBool("IsSliding", isSliding);
        PlayerAnimator.SetBool("IsWalking", isMoving);
        PlayerAnimator.SetBool("IsFalling", isFalling);
        PlayerAnimator.SetBool("IsFlapping", isFlying);
        PlayerAnimator.SetBool("IsThinking", isThinking);
        PlayerAnimator.SetBool("IsRiding", isRiding);
    }

    public void EnterExitVehicle(InputAction.CallbackContext obj)
    {
        if (!isRiding)
        {
            if (RCControlToRide != null)
            {
                //controller.enabled = false;
                //controller.height = 0;
                //controller.radius = 0;
                int seatNumber = RCControlToRide.AddRider(this);
                if (seatNumber >= 0)
                {
                    if (seatNumber == 0)    // if is driver, the transform of the car will be controlled by this player
                    {
                        /*                        RCControlToRide.GetComponent<RollerCoasterASL>().isLocal = true;
                                                RCControlToRide.GetComponent<RollerCoasterASL>().StartASL();*/
                        RCControlToRide.GetComponent<RollerCoasterASL>().StartASL();

                    } else
                    {
                        movement.Disable();
                    }

                    //controller.enabled = false;
                    //gameObject.transform.parent = RCControlToRide.transform;
                    RCControlRiding = RCControlToRide;

                    SetEnterVehicleGUI(false);

                    //controller.Move((RCControlRiding.transform.position - transform.position) + RCControlRiding.SeatPositions[seatNumber]);

                    //RCControlRiding.AddRider(this);
                    isRiding = true;

                    //movement.Disable();
                    jump.Disable();
                    disableGravity.Disable();
                    enableGravity.Disable();
                    scaleUp.Disable();
                    scaleDown.Disable();

                    canvas.gameObject.SetActive(false);
                }
            }
        }
        else
        {

            if (RCControlRiding.IsDriver(this))
            {
                RCControlRiding.GetComponent<RollerCoasterASL>().StopASL();
            }

            RCControlRiding.KickPlayer(this);
            //if (RCControlRiding)
            //isRiding = false;
            ////////////////
            //controller.enabled = true;
            //controller.height = 2.1f;
            //controller.radius = 0.5f;

            //gameObject.transform.parent = null;
            RCControlRiding = null;

            movement.Enable();
            jump.Enable();
            disableGravity.Enable();
            enableGravity.Enable();
            scaleUp.Enable();
            scaleDown.Enable();

            canvas.gameObject.SetActive(true);
        }
    }


    public void SetEnterVehicleGUI(bool op)
    {
        EnterVehicleGUI.SetActive(op);
    }

/*    void carForward(InputAction.CallbackContext obj)
    {
        if (RCControlRiding.IsDriver(this))
        {
            isForward = true;
        }
            
    }

    void carBackward(InputAction.CallbackContext obj)
    {
        if (RCControlRiding.IsDriver(this))
        {
            isBackward = true;
        }
    }

    void carStop(InputAction.CallbackContext obj)
    {
        if (RCControlRiding.IsDriver(this))
        {
            isForward = false;
            isBackward = false;
        }
    }*/

    void clearSliding()
    {
        SlidingVector = Vector3.zero;
        newSlidingVec = Vector3.zero;
        inheritedSliding = Vector3.zero;
    }
}


