using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VKeyboard : MonoBehaviour
{

    public bool TrackPlayer;
    private Vector3 standby;
    private float lastPlayerScale = 1;
    private float lastKeyboardScale = 1;
    // Start is called before the first frame update
    void Start()
    {
        standby = new Vector3(0, -30, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if(TrackPlayer)
        {
        GameObject player = GameObject.Find("PlayerPre(Clone)");
        float playerScale = 1;
        float keyboardScale = 1;
        Debug.Log(player.transform.localScale.x);
        if(player.transform.localScale.x != lastPlayerScale)
        {
            lastPlayerScale = player.transform.localScale.x;
            if(player.transform.localScale.x == 1)
            {
                playerScale = 1f;
            }
            else if (player.transform.localScale.x <= 2.5f)
            {
                playerScale = 2.75f;
            }
            else if (player.transform.localScale.x <= 4.5)
            {
                playerScale = 5f;
                keyboardScale = 1.4f;
            }
            else if (player.transform.localScale.x <= 6.5f)
            {
                playerScale = 7f;
                keyboardScale = 1.5f;
            }
            else 
            {
                playerScale = 10f;
                keyboardScale = 1.75f;
            }
            if(lastKeyboardScale < keyboardScale)
            {
                lastKeyboardScale = keyboardScale;
                gameObject.transform.localScale = gameObject.transform.localScale * keyboardScale;
            }
            else if(lastKeyboardScale > keyboardScale)
            {
                lastKeyboardScale = keyboardScale;
                gameObject.transform.localScale = gameObject.transform.localScale / keyboardScale;
            }
        }
        gameObject.transform.position = Camera.main.transform.position + Camera.main.transform.forward * (1.25f * playerScale) + Camera.main.transform.up * -.45f;
        
        gameObject.transform.rotation = Quaternion.LookRotation(gameObject.transform.position - Camera.main.transform.position, Vector3.up);
        }
        else
        {
            gameObject.transform.position = standby;
        }
    }
}
