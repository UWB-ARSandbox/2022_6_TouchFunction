using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerClickGraph : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction click;
    private LayerMask layerMask;

    public GameObject playerCam;
    // Start is called before the first frame update
    void Start()
    {
        playerInput = new PlayerInput();
        layerMask = LayerMask.GetMask("Point");
    }

    void Awake()
    {
        click = new InputAction(binding: "<Mouse>/leftButton");
        click.performed += TryFindPoint;
        click.Enable();
    }
    // void OnEnable()
    // {
    //     click = playerInput.PlayerControls.Click;
    //     click.performed += TryFindPoint;
    //     click.Enable();
    // }

    private void TryFindPoint(InputAction.CallbackContext obj)
    {
        //GameObject player = GetComponent<Player>().playerCam;
        bool cursorLocked = GetComponent<Player>().IsCursorLocked();
        Debug.Log("PlayerClicked");
        RaycastHit hit;
        if(cursorLocked)
        {
            if(Physics.Raycast(playerCam.transform.position, playerCam.transform.TransformDirection(Vector3.forward), out hit, 25, layerMask)) 
            {
                if(hit.transform.gameObject != null)
                {
                    Debug.Log("Found Object");
                    hit.transform.gameObject.GetComponent<Point>().WasHit();
                }
                else
                {
                    Debug.Log("GameObject not found.");
                }
            }
        }
        else 
        {
            Ray ray = playerCam.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray.origin, ray.direction, out hit, 25, layerMask))
            {
                if(hit.transform.gameObject != null)
                {
                    Debug.Log("Found Object");
                    hit.transform.gameObject.GetComponent<Point>().WasHit();
                }
                else
                {
                    Debug.Log("GameObject not found.");
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
