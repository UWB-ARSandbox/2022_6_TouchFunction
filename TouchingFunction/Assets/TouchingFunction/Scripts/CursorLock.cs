using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : MonoBehaviour
{
    public void LockCursor() {
        Cursor.lockState = CursorLockMode.Locked;
        GameObject.Find("CrossHair").transform.localScale = new Vector3(1, 1, 1);
    }

    public void UnlockCursor() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        GameObject.Find("CrossHair").transform.localScale = new Vector3(0, 0, 0);
    }

    public bool IsCursorLocked() {
        return Cursor.lockState != CursorLockMode.None;
    }

    public void ToggleCursorLock() {
        if (!IsCursorLocked()) {
            Cursor.lockState = CursorLockMode.Locked;
            GameObject.Find("CrossHair").transform.localScale = new Vector3(1, 1, 1);
        }
        else {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            GameObject.Find("CrossHair").transform.localScale = new Vector3(0, 0, 0);
        }
    }
}
