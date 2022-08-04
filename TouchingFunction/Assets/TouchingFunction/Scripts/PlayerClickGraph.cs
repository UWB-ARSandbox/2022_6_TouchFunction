using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using ASL;
using UnityEngine.UI;

public class PlayerClickGraph : MonoBehaviour
{
    public bool IsSettingRC;
    public bool IsDeletingRC = false;

    public Toggle SettingRCToggle;
    public Toggle DeleteRCToggle;
    public GameObject RCPrefab;
    public GameObject DMV;

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

    GameObject hoverRC;
    GameObject deleteHoverRC;
    public GameObject toDelete;
    
    LayerMask layerMask;
    LayerMask rcLayer;
    int graphLayer;
    int pointLayer;



    static RaycastHit hit;
    // Start is called before the first frame update
    void Start()
    {

        SettingRCToggle = GameObject.Find("SetRCToggle").GetComponent<Toggle>();
        DeleteRCToggle = GameObject.Find("DeleteRCToggle").GetComponent<Toggle>();
        playerInput = new PlayerInput();

        pointLayer = LayerMask.NameToLayer("Point");
        graphLayer = LayerMask.NameToLayer("GraphMesh");
        layerMask = LayerMask.GetMask("GraphMesh", "Point");
        rcLayer = LayerMask.GetMask("RollerCoaster");

        playerCam = GetComponentInChildren<Camera>();
        
        hoverPoint = GameObject.Find("hoverPoint");
        hoverPoint.SetActive(false);

        //IsSettingRC = true;
        hoverRC = GameObject.Find("hoverRC");
        hoverRC.SetActive(false);

        deleteHoverRC = GameObject.Find("deleteHoverRC");
        deleteHoverRC.SetActive(false);
    }

    void Awake()
    {
        pointPrefab = Resources.Load<GameObject>("MyPrefabs/point");
        RCPrefab = Resources.Load<GameObject>("MyPrefabs/RollerCoaster");
        DMV = GameObject.Find("DMV");
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

        if (IsSettingRC)    // creating roller coaster
        {
            if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, 25, layerMask))
            {
                if (hit.transform.gameObject.layer == graphLayer)
                {

                    hoverRC.transform.position = new Vector3(hit.point.x, hit.point.y, hit.transform.position.z) + 0.3f * hit.normal;
                    hoverRC.SetActive(true);
                    float angle = Vector3.Angle(hit.normal, Vector3.up);
                    if (hit.normal.x < 0)
                    {
                        angle *= -1;
                    }
                    hoverRC.transform.localEulerAngles = new Vector3(angle, 90, 0);
                }
            }
            else
            {
                hoverRC.SetActive(false);
            }
        } else if (IsDeletingRC)    // deleting roller coaster
        {
            if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, 25, rcLayer))
            {
                deleteHoverRC.transform.position = hit.transform.position;
                deleteHoverRC.transform.rotation = hit.transform.rotation;
                deleteHoverRC.SetActive(true);
                toDelete = hit.transform.gameObject;
            } else
            {
                toDelete = null; 
                deleteHoverRC.SetActive(false);
            }
        }
        else             // is creating points
        {
            if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, 25, layerMask) ||
            Physics.SphereCast(playerCam.transform.position, 1f, playerCam.transform.forward, out hit, 25, layerMask))
            {
                if (hit.transform.gameObject.layer == graphLayer)
                {


                     if (highlighted != null)
                     {
                         highlighted.GetComponentInParent<Point>().Deselect();
                         highlighted = null;
                     }
                     hoverPoint.transform.position = new Vector3(hit.point.x, hit.point.y, hit.transform.position.z);
                     hoverPoint.SetActive(true);
                    
                }
                else if (hit.transform.gameObject.layer == pointLayer)
                {
                    hoverPoint.SetActive(false);

                    if (highlighted != null && highlighted != hit.transform.gameObject)
                    {
                        highlighted.GetComponentInParent<Point>().Deselect();
                    }
                    hit.transform.GetComponentInParent<Point>().Select();
                    highlighted = hit.transform.gameObject;
                }
            }
            else
            {
                if (highlighted != null)
                {
                    highlighted.GetComponentInParent<Point>().Deselect();
                    highlighted = null;
                }
                hoverPoint.SetActive(false);               
            }
        }
    }

    void Click(InputAction.CallbackContext obj)
    {
        if(IsSettingRC)
        {
            if (hoverRC.activeInHierarchy)
            {
                //Debug.LogError("inSETTING");
                ASLHelper.InstantiateASLObject("RollerCoaster", hoverRC.transform.position, hoverRC.transform.rotation, null, null, OnRCCreated);
                //GameObject.Find("RollerCoaster").GetComponent<RollerCoasterControl>().mesh = hit.transform.GetComponent<MeshCreator>();
                //GameObject rc = Instantiate(RCPrefab, hoverRC.transform.position, hoverRC.transform.rotation);
                //rc.GetComponent<RollerCoasterControl>().mesh = hit.transform.GetComponent<MeshCreator>();
                IsSettingRC = false;
                SettingRCToggle.isOn = false;
                hoverRC.SetActive(false);
            }

        } else if (IsDeletingRC)
        {
            if (deleteHoverRC.activeInHierarchy)
            {
                toDelete.GetComponent<ASLObject>().SendAndSetClaim(() => {
                    toDelete.GetComponent<ASLObject>().DeleteObject();
                    toDelete = null;
                    IsDeletingRC = false;
                    DeleteRCToggle.isOn = false;
                    deleteHoverRC.SetActive(false);
                });
                
            }
        } else
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

    private static void OnRCCreated(GameObject _obj)
    {
        _obj.GetComponent<RollerCoasterControl>().mesh = hit.transform.GetComponent<MeshCreator>();
        _obj.GetComponent<RollerCoasterASL>().InitVehicle();
        _obj.GetComponent<RollerCoasterASL>().SendMeshInfo();
    }


}