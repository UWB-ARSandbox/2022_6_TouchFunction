using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ASL;

public class PlayerMovement : MonoBehaviour
{
    public Transform head;
    public float speed = 5.0f;
    public float jumpSpeed = 10.0f;
    public float gravity = 10.0f;
    public bool onPlatform = false;
    public float verticalSpeed = 0f;

    public bool inAir = false;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; 
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            ToggleCursorLock();            
        } 
        Vector3 currPos = transform.localPosition;

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
        transform.localPosition = new Vector3(currPos.x, newY, currPos.z);

        // if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
        // {
        //     Vector3 fwd = new Vector3(head.forward.x, 0, head.forward.z).normalized;
        //     if (Input.GetKey(KeyCode.W))
        //     {
                
        //         transform.Translate(fwd * Time.deltaTime * speed);
        //     }
        //     if (Input.GetKey(KeyCode.S))
        //     {
        //         transform.Translate(-1*fwd * Time.deltaTime * speed);
        //     }
        // }

        // if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) {
        //     Vector3 rgt = new Vector3(head.right.x, 0, head.right.z).normalized;
        //     if (Input.GetKey(KeyCode.A))
        //     {
        //         transform.Translate(-1 * rgt * Time.deltaTime * speed);
        //     }
        //     if (Input.GetKey(KeyCode.D))
        //     {
        //         transform.Translate(rgt * Time.deltaTime * speed);
        //     }
        // }

        if (isCursorLocked()) {
            Vector3 dir = new Vector3(0, 0, 0);
            dir.x = Input.GetAxis("Horizontal"); // Left/Right movement
            dir.z = Input.GetAxis("Vertical");  // Forward/Backward movement
            transform.Translate(dir * Time.deltaTime * speed);

            if (dir.z > 0) {
                // walking motion
            }
            else if (dir.x > 0) {
                //strafing motion
            }
            else {
                // standing still
            }
            
            if (Input.GetKey(KeyCode.Space))
            {
                if (!inAir)
                {
                    verticalSpeed = jumpSpeed;
                }
                
            }
        }

        // GetComponent<ASLObject>().SendAndSetClaim(() =>
        // {
        //     GetComponent<ASLObject>().SendAndSetWorldPosition(transform.position);  
        // });
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
