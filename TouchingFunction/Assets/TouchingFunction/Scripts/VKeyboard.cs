using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VKeyboard : MonoBehaviour
{

    public bool TrackPlayer;
    private Vector3 standby;
    private float lastPlayerScale = 1;
    private float lastKeyboardScale = 1;

    public float playerScale;
    public float keyboardScale;

    public GameObject camera;
    // Start is called before the first frame update
    void Start()
    {
        standby = new Vector3(0, -30, 0);
    }

    public void PosiitonKeyboard()
    {
        transform.position = transform.parent.position + transform.parent.forward * 2f + transform.parent.up * -.4f;
        gameObject.transform.rotation = Quaternion.LookRotation(gameObject.transform.position - camera.transform.position, Vector3.up);
    }

    public void ViewKeyboard()
    {
        GetComponent<Canvas>().enabled = true;
    }

    public void HideKeyboard()
    {
        GetComponent<Canvas>().enabled = false;
    }

    public bool IsKeyboardVisable()
    {
        return GetComponent<Canvas>().enabled;
    }
}
