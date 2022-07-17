using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformCollision : MonoBehaviour
{
   
    /*
     * // set player to layer 6
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            
            int index = GetComponentInParent<MeshCreator>().GraphIndex;
            //Debug.Log("COLLIDE!!!!!!!!!!!!    " + index);
            Player p = other.GetComponent<Player>();
            if (p.isFalling())
            {
                p.land();
                p.onPlatform = index;
                p.verticalSpeed = 0;
                p.mountMesh(index);
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            int index = GetComponentInParent<MeshCreator>().GraphIndex;
            //Debug.Log("EXIT!!!!!!!!!!!!    " + index);
            Player p = other.GetComponent<Player>();
            p.onPlatform = -1;
        }
    }*/

}
