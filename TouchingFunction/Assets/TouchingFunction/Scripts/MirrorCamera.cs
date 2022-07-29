using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorCamera : MonoBehaviour
{
    public Transform player;
    public float rotSpeed = 45;
    public bool isRunning = false;
    float currAngle;
    Vector3 anchorPoint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isRunning)
        {
            RotateCamera();
            anchorPoint = player.position + player.localScale.x * Vector3.up * 2.5f;
        } else
        {
            resetMirrorCamera();
        }
    }

    public void RotateCamera()
    {
        currAngle += Time.deltaTime * rotSpeed;
        gameObject.transform.position = anchorPoint + Quaternion.AngleAxis(currAngle, Vector3.up) 
                                                    * player.forward * player.localScale.x * 4;
        transform.LookAt(player);
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x-20, transform.localEulerAngles.y, transform.localEulerAngles.z);
    }

    void resetMirrorCamera()
    {
        gameObject.transform.position = Vector3.zero;
        gameObject.transform.localEulerAngles = Vector3.zero;
    }

    public void StartMirrorCamera()
    {
        anchorPoint = player.position + player.localScale.x * Vector3.up * 2.5f;
        isRunning = true;
    }

    public void StopMirrorCamera()
    {
        isRunning = false;
    }

}
