using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : MonoBehaviour
{
    public Transform head;
    public float mouseSensitivity = 10f;
    public float contSensitivity = 100f;
    public float clampAngle = 60.0f;

    private float rotY = 0.0f; // rotation around the up/y axis
    private float rotX = 0.0f; // rotation around the right/x axis


    void RotateCameraVR()
    {
        Quaternion moveValVR = vrLook.ReadValue<Quaternion>();
        Vector3 vecVR = moveValVR.eulerAngles;
        //head.localRotation = Quaternion.Euler(-vecVR.x, 0, 0);
        //head.localRotation = Quaternion.Euler(vecVR.x, 0, 0);
        if (!isRiding)
        {
            head.localRotation = Quaternion.Euler(vecVR.x, 0, 0);
            transform.rotation = Quaternion.Euler(0, vecVR.y, 0);
        }
        else
        {
            head.rotation = Quaternion.Euler(vecVR.x, vecVR.y, 0);
        }
    }

    void RotateCameraMouse()
    {
        //mouseLook = false;
        Vector2 moveValue = look.ReadValue<Vector2>();

        rotY += moveValue.x * (mouseSensitivity) * Time.deltaTime;
        rotX += moveValue.y * (mouseSensitivity) * Time.deltaTime;

        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);

        if (!isRiding)
        {
            head.localRotation = Quaternion.Euler(-rotX, 0, 0);
            transform.rotation = Quaternion.Euler(0, rotY, 0);
        }
        else
        {
            head.rotation = Quaternion.Euler(-rotX, rotY, 0);
        }
    }
}
