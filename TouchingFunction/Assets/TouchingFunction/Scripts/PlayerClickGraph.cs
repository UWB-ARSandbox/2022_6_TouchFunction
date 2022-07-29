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

    // public GameObject rightCon;
    // public GameObject leftCon;

    Camera playerCam;
    GameObject pointPrefab;
    GameObject hoverPoint;
    GameObject highlighted;

    LayerMask layerMask;
    int graphLayer;
    int pointLayer;

    RaycastHit hit;
    // Start is called before the first frame update
    void Start()
    {
        playerInput = new PlayerInput();

        pointLayer = LayerMask.NameToLayer("Point");
        graphLayer = LayerMask.NameToLayer("GraphMesh");
        layerMask = LayerMask.GetMask("GraphMesh", "Point");

        playerCam = GetComponentInChildren<Camera>();
        
        hoverPoint = GameObject.Find("hoverPoint");
        hoverPoint.SetActive(false);
    }

    void Awake()
    {
        pointPrefab = Resources.Load<GameObject>("MyPrefabs/point");
        // Unity's new input action system doesn't work for left click. Known bug in the code. Future updates might fix
        // creating action and binding in code is best solution right now. 
        click = new InputAction(binding: "<Mouse>/leftButton");
        click.performed += Click;
        click.Enable();

        clickVRRight = new InputAction(binding: "<XRController>{RightHand}/triggerPressed");
        clickVRRight.performed += Click;
        clickVRRight.Enable();

        clickVRLeft = new InputAction(binding: "<XRController>{LeftHand}/triggerPressed");
        clickVRLeft.performed += Click;
        clickVRLeft.Enable();
    }

    void Update()
    {
        if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, 25, layerMask) ||
            Physics.SphereCast(playerCam.transform.position, 1f, playerCam.transform.forward, out hit, 25, layerMask))
        {
            if(hit.transform.gameObject.layer == graphLayer)
            {
                if(highlighted != null)
                {
                    highlighted.GetComponentInParent<Point>().Deselect();
                    highlighted = null;
                }
                hoverPoint.transform.position = new Vector3(hit.point.x, hit.point.y, hit.transform.position.z);
                hoverPoint.SetActive(true);
            }
            else if(hit.transform.gameObject.layer == pointLayer)
            {
                hoverPoint.SetActive(false);

                if(highlighted != null && highlighted != hit.transform.gameObject)
                {
                    highlighted.GetComponentInParent<Point>().Deselect();
                }
                hit.transform.GetComponentInParent<Point>().Select();
                highlighted = hit.transform.gameObject;
            }

        }
        else
        {
            if(highlighted != null)
            {
                highlighted.GetComponentInParent<Point>().Deselect();
                highlighted = null;
            }
            hoverPoint.SetActive(false);
        }
    }

    void Click(InputAction.CallbackContext obj)
    {
        if (hoverPoint.activeInHierarchy)
        {
            ASL.ASLHelper.InstantiateASLObject("point", hit.transform.InverseTransformPoint(hoverPoint.transform.position), Quaternion.identity, hit.transform.name);
        }

        if (highlighted != null)
        {
            highlighted.GetComponentInParent<ASL.ASLObject>().SendAndSetClaim(() =>
            {
                highlighted.GetComponentInParent<ASL.ASLObject>().DeleteObject();
            });
        }
    }
}