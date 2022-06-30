using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformCollision : MonoBehaviour
{
    public GameObject Player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == Player)
        {
            //Player.transform.parent = transform;
            Player.GetComponent<PlayerMovement>().onPlatform = true;
            Player.GetComponent<PlayerMovement>().verticalSpeed = 0;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == Player)
        {
            Player.GetComponent<PlayerMovement>().onPlatform = false;
            //Player.transform.parent = null;
        }
    }

}
