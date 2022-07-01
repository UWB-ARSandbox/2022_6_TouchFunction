using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ASL;

public class PlayerMovement : MonoBehaviour
{
    public GameObject playerCam;
    public float mouseSensitivity = 100.0f;
    public float clampAngle = 80.0f;

    private float rotY = 0.0f; // rotation around the up/y axis
    private float rotX = 0.0f; // rotation around the right/x axis

    public Transform head;
    public float speed = 5.0f;
    public float jumpSpeed = 10.0f;
    public float gravity = 10.0f;
    public bool onPlatform = false;
    public float verticalSpeed = 0f;

    public bool inAir = false;

    private static readonly float UPDATES_PER_SECOND = 2.0f;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; 
    }

    IEnumerator NetworkedUpdate()
    {
        while (true)
        {

            
            yield return new WaitForSeconds(1 / UPDATES_PER_SECOND);
        }
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
        // GetComponent<ASLObject>().SendAndSetClaim(() =>
        // {
        //     GetComponent<ASLObject>().SendAndSetWorldPosition(new Vector3(currPos.x, newY, currPos.z));
        // });
        
        if (isCursorLocked()) {
            MovePlayer();
            RotateCamera();
        }
    }

    void MovePlayer()
    {
        Vector3 dir = new Vector3(0, 0, 0);
        dir.x = Input.GetAxis("Horizontal"); // Left/Right movement
        dir.z = Input.GetAxis("Vertical");  // Forward/Backward movement

        transform.Translate(dir * Time.deltaTime * speed);
        // GetComponent<ASLObject>().SendAndSetClaim(() =>
        // {
        //     GetComponent<ASLObject>().SendAndSetWorldPosition(transform.position + dir * Time.deltaTime * speed);
        // });
        
        if (Input.GetKey(KeyCode.Space))
        {
            if (!inAir)
            {
                verticalSpeed = jumpSpeed;
            }
        }
    }

    void RotateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        rotY += mouseX * mouseSensitivity * Time.deltaTime;
        rotX += mouseY * mouseSensitivity * Time.deltaTime;

        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);

        head.transform.rotation = Quaternion.Euler(0, rotY, rotX);
        transform.rotation = Quaternion.Euler(0, rotY+90f, 0);
    }

    void ToggleCursorLock() {
        if (!isCursorLocked()) {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    bool isCursorLocked() {
        return Cursor.lockState != CursorLockMode.None;
    }
}
