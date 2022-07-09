using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
//using UnityEngine.InputSystem.XR;

public class QuitScript : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction quit;
    public GameObject quitWindow;
    public GameObject player;

   
    //public static void Stay() { DoStay(); }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void Awake()
    {
        playerInput = new PlayerInput();
    }

    void OnEnable()
    {
        quit = playerInput.PlayerControls.Quit;
        quit.performed += TryQuit;
        quit.Enable();
    }

    void TryQuit(InputAction.CallbackContext obj)
    {
        if(player.GetComponent<Player>().IsCursorLocked())
        {
        Debug.Log("Quit hit.");
        quitWindow.SetActive(true);
        player.GetComponent<Player>().UnlockCursor();
        
        }
    }

    public void DoStay()
    {
        player.GetComponent<Player>().LockCursor();
        quitWindow.SetActive(false);
    }

    public void Quit() 
    {
        player.SetActive(false);
        Application.Quit(); 
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
