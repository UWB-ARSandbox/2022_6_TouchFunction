using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float mouseSensitivity = 100.0f;
    public float clampAngle = 80.0f;

    public Transform playerBody;
    public Transform playerHead;

    private float rotY = 0.0f; // rotation around the up/y axis
    private float rotX = 0.0f; // rotation around the right/x axis

    void Start()
    {
        // Vector3 rot = transform.localRotation.eulerAngles;
        // rotY = rot.y;
        // rotX = rot.z;
    }

    void Update()
    {
        if (isCursorLocked()) {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = -Input.GetAxis("Mouse Y");

            rotY += mouseX * mouseSensitivity * Time.deltaTime;
            rotX -= mouseY * mouseSensitivity * Time.deltaTime;

            rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);

            transform.rotation = Quaternion.Euler(rotX, rotY, 0);
            playerHead.rotation = Quaternion.Euler(0, rotY-90f, -rotX);
            playerBody.rotation = Quaternion.Euler(0, rotY, 0);
        }
    }

    bool isCursorLocked() {
        return Cursor.lockState != CursorLockMode.None;
    }
}
