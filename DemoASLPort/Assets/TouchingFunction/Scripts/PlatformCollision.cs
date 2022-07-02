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
            Player.GetComponent<Player>().onPlatform = true;
            Player.GetComponent<Player>().verticalSpeed = 0;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == Player)
        {
            Player.GetComponent<Player>().onPlatform = false;
            //Player.transform.parent = null;
        }
    }

}
