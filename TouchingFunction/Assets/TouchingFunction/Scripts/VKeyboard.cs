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

    // Update is called once per frame
    void Update()
    {
        
        if(TrackPlayer)
        {
        GameObject player = GameObject.Find("PlayerPre(Clone)");
        playerScale = player.transform.localScale.x;
        keyboardScale = gameObject.transform.localScale.x;
        // float playerScale = 1;
        // float keyboardScale = 1;
        Debug.Log(player.transform.localScale.x);
        
            
            if (player.transform.localScale.x > 2f)
            {
                playerScale = player.transform.localScale.x;
                keyboardScale = player.transform.localScale.x /2f;
            }
        if(player.transform.localScale.x != lastPlayerScale)
        {    
            lastPlayerScale = player.transform.localScale.x;
            keyboardScale = player.transform.localScale.x /2f;
            lastPlayerScale = player.transform.localScale.x;
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
        gameObject.transform.position = camera.transform.position;
        gameObject.transform.position = gameObject.transform.position + (camera.transform.forward * (playerScale));
        gameObject.transform.position = gameObject.transform.position + (camera.transform.up * -.4f );
        
        gameObject.transform.rotation = Quaternion.LookRotation(gameObject.transform.position - camera.transform.position, Vector3.up);
        }
        else
        {
            gameObject.transform.position = standby;
        }
    }
}
