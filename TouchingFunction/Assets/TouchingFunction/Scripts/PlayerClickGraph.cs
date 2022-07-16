using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerClickGraph : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction click;
    private InputAction clickVRRight;
    private InputAction clickVRLeft;

    private LayerMask layerMask;

    public GameObject rightCon;
    public GameObject leftCon;

    public GameObject playerCam;
    // Start is called before the first frame update
    void Start()
    {
        playerInput = new PlayerInput();
        layerMask = LayerMask.GetMask("Point");
    }

    void Awake()
    {
        // Unity's new input action system doesn't work for left click. Known bug in the code. Future updates might fix
        // creating action and binding in code is best solution right now. 
        click = new InputAction(binding: "<Mouse>/leftButton");
        click.performed += TryFindPoint;
        click.Enable();

        clickVRRight = new InputAction(binding: "<XRController>{RightHand}/triggerPressed");
        clickVRRight.performed += TryFindPointVRRight;
        clickVRRight.Enable();

        clickVRLeft = new InputAction(binding: "<XRController>{LeftHand}/triggerPressed");
        clickVRLeft.performed += TryFindPointVRLeft;
        clickVRLeft.Enable();
    }

    // For PC users. Currently only targets objects in "Point" layer
    private void TryFindPoint(InputAction.CallbackContext obj)
    {
        RaycastHit hit;
        // if cursor locked, use center of camera for raycast
        if(GetComponent<Player>().IsCursorLocked())
        {
            if(Physics.Raycast(playerCam.transform.position, playerCam.transform.TransformDirection(Vector3.forward), out hit, 25, layerMask)) 
            {
                tryHit(hit);
            }
        }
        // if cursor not locked, use mouse position for raycast
        else 
        {
            Ray ray = playerCam.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray.origin, ray.direction, out hit, 25, layerMask))
            {
                tryHit(hit);
            }
        }
    }

    // Checks if gameObject is not null, then executes WasHit method. 
    private void tryHit(RaycastHit hit)
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

    // for VR users. uses right controller position and rotation for raycast. only checks for objects in "Point" layer.
   private void TryFindPointVRRight(InputAction.CallbackContext obj)
   {
        RaycastHit hit;
        if(Physics.Raycast(rightCon.transform.position, rightCon.transform.TransformDirection(Vector3.forward), out hit, 25, layerMask))
        {
            tryHit(hit);
        }
   }

   // for VR users. uses right controller position and rotation for raycast. only checks for objects in "Point" layer.
   private void TryFindPointVRLeft(InputAction.CallbackContext obj)
   {
        RaycastHit hit;
        if(Physics.Raycast(leftCon.transform.position, leftCon.transform.TransformDirection(Vector3.forward), out hit, 25, layerMask))
        {
            tryHit(hit);
        }
   }
}
