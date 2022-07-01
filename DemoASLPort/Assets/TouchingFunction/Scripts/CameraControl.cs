using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{


    void Start()
    {
        // Vector3 rot = transform.localRotation.eulerAngles;
        // rotY = rot.y;
        // rotX = rot.z;
    }

    void Update()
    {
        if (isCursorLocked()) {

        }
    }

    bool isCursorLocked() {
        return Cursor.lockState != CursorLockMode.None;
    }
}
