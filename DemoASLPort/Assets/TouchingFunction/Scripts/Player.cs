using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ASL;

public partial class Player : MonoBehaviour
{
    #region Player movement variables
    public float speed = 5.0f;
    public float jumpSpeed = 10.0f;
    public float gravity = 10.0f;
    public bool onPlatform = false;
    public float verticalSpeed = 0f;
    public bool inAir = false;
    #endregion
    
    #region Player animation
    public Transform rightArm;
    public Transform rightArmPivot;
    public Transform leftArm;
    public Transform leftArmPivot;

    bool gravityEnabled = true;
    bool armMovingForward = true;
    bool armFlappingUp = true;
    # endregion


    // Start is called before the first frame update
    void Start()
    {
        LockCursor();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            ToggleCursorLock();            
        } 

        Vector3 currPos = transform.position;

        if (currPos.y <= 0 || onPlatform)
        {
            inAir = false;
        } else
        {
            inAir = true;
        }

        // Vertical movement
        if(gravityEnabled)
        {
            if (inAir) 
            {
                verticalSpeed -= gravity * Time.deltaTime;
            } 

            
            float newY = currPos.y + Time.deltaTime * verticalSpeed;
            if (newY < 0)
            {
                newY = 0;
                verticalSpeed = 0;
                inAir = false;
            }
            transform.position = new Vector3(currPos.x, newY, currPos.z);
        }

        if (inAir && !gravityEnabled) {
            FlappingArmMovement(-0.3f, -0.9f);
        }
        else {
            if (leftArm.localRotation.z > -0.1f || leftArm.localRotation.z < -0.2f) {
                FlappingArmMovement(-0.1f, -0.2f);
            }
        }
        
        
        if (IsCursorLocked()) {
            MovePlayer();
            RotateCamera();

            if (Input.GetKeyDown(KeyCode.G)) {
                gravityEnabled = !gravityEnabled;
            }

            if (Input.GetKey(KeyCode.Space))
            {
                if (!inAir && gravityEnabled)
                {
                    verticalSpeed = jumpSpeed;
                }
                else if (!gravityEnabled) {
                    float jumpY = currPos.y + Time.deltaTime * jumpSpeed;
                    transform.localPosition = new Vector3(currPos.x, jumpY, currPos.z);
                }
            }

            if (Input.GetKey(KeyCode.F) && inAir && !gravityEnabled) {
                float fallY = currPos.y + Time.deltaTime * -jumpSpeed;
                transform.localPosition = new Vector3(currPos.x, fallY, currPos.z);
            }
        }
    }

    void MovePlayer()
    {
        Vector3 dir = new Vector3(0, 0, 0);
        dir.x = Input.GetAxis("Horizontal"); // Left/Right movement
        dir.z = Input.GetAxis("Vertical");  // Forward/Backward movement

        transform.Translate(dir * Time.deltaTime * speed);

        // Walking animation movement
        if (!inAir) {
            if (dir.z != 0 || dir.x != 0) {
                WalkingArmMovement(0.25f);
            }
            else {
                if(leftArm.localRotation.x > 0.01f || leftArm.localRotation.x < -0.01f) {
                    WalkingArmMovement(0.01f);
                }
            }
        }
    }

    void WalkingArmMovement(float maxAngle) {
        if (armMovingForward) {
            leftArm.RotateAround(leftArmPivot.position, leftArm.TransformDirection(Vector3.right), 200*Time.deltaTime);
            rightArm.RotateAround(rightArmPivot.position, rightArm.TransformDirection(Vector3.left), 200*Time.deltaTime);
        }
        else {
            leftArm.RotateAround(leftArmPivot.position, leftArm.TransformDirection(Vector3.left), 200*Time.deltaTime);
            rightArm.RotateAround(rightArmPivot.position, rightArm.TransformDirection(Vector3.right), 200*Time.deltaTime);
        }

        if (leftArm.localRotation.x > maxAngle) {
            armMovingForward = false;                  
        }
        else if (leftArm.localRotation.x < -maxAngle) {
            armMovingForward = true;
        }
    }

    void FlappingArmMovement(float maxAngle, float minAngle) {
        if (armFlappingUp) {
            leftArm.RotateAround(leftArmPivot.position, leftArm.TransformDirection(Vector3.forward), 400*Time.deltaTime);
            rightArm.RotateAround(rightArmPivot.position, rightArm.TransformDirection(Vector3.back), 400*Time.deltaTime);
        }
        else {
            leftArm.RotateAround(leftArmPivot.position, leftArm.TransformDirection(Vector3.back), 400*Time.deltaTime);
            rightArm.RotateAround(rightArmPivot.position, rightArm.TransformDirection(Vector3.forward), 400*Time.deltaTime);
        }

        if (leftArm.localRotation.z > maxAngle) {
            armFlappingUp = false;                  
        }
        else if (leftArm.localRotation.z < minAngle) {
            armFlappingUp = true;
        }
    }
}
