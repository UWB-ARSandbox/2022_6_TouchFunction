using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class PlayerClickGraph : MonoBehaviour
{
    Transform graphPlane;
    private PlayerInput playerInput;
    private InputAction click;
    private InputAction rightClick;
    private InputAction clickVR;
    private InputAction manipulatePoint;
    private InputAction gripClickVR;

    // public GameObject rightCon;
    // public GameObject leftCon;

    Camera playerCam;
    GameObject pointPrefab;
    GameObject hoverPoint;

    [SerializeField] GameObject currHoveringPoint;
    [SerializeField] GameObject currDraggingPoint;
    [SerializeField] GameObject currHoveringHandle;
    [SerializeField] bool enableDragPoint = false;
    Transform RightCon;
    Transform LeftCon;



    LayerMask layerMask;
    int graphLayer;
    int pointLayer;
    int handleLayer;
    LayerMask UILayer;

    RaycastHit hit;
    Ray currentRay;

    Vector3 p;

    PlayerActivateVRHands vRHands;

    // Start is called before the first frame update
    void Start()
    {
        graphPlane = GameObject.Find("GraphAxes").transform;
        hoverPoint = GameObject.Find("hoverPoint");
        hoverPoint.SetActive(false);

        pointLayer = LayerMask.NameToLayer("Point");
        graphLayer = LayerMask.NameToLayer("GraphMesh");
        handleLayer = LayerMask.NameToLayer("PointHandle");
        layerMask = LayerMask.GetMask("GraphMesh", "Point", "PointHandle");
        UILayer = LayerMask.GetMask("UI");

        LeftCon = vRHands.LeftHand.transform;
        RightCon = vRHands.RightHand.transform;
    }

    void Awake()
    {
        vRHands = GetComponent<PlayerActivateVRHands>();
        playerCam = GetComponentInChildren<Camera>();

        playerInput = new PlayerInput();

        pointPrefab = Resources.Load<GameObject>("MyPrefabs/point");
        // Unity's new input action system doesn't work for left click. Known bug in the code. Future updates might fix
        // creating action and binding in code is best solution right now. 
        click = new InputAction(binding: "<Mouse>/leftButton");
        click.started += Click;
        click.canceled += Release;
        click.Enable();

        rightClick = new InputAction(binding: "<Mouse>/rightButton");
        rightClick.performed += RightClick;
        rightClick.Enable();

        clickVR = playerInput.PlayerControls.Trigger;
        clickVR.started += Click;
        clickVR.canceled += Release;
        clickVR.Enable();

        gripClickVR = playerInput.PlayerControls.Grip;
        gripClickVR.performed += RightClick;
        gripClickVR.Enable();
    }

    Vector3 IntersectPlane(Ray ray)
    {
        //A plane can be defined as:
        //a point representing how far the plane is from the world origin
        Vector3 p_0 = graphPlane.position;
        //a normal (defining the orientation of the plane), should be negative if we are firing the ray from above
        Vector3 n = -graphPlane.forward;
        //We are intrerested in calculating a point in this plane called p
        //The vector between p and p0 and the normal is always perpendicular: (p - p_0) . n = 0

        //A ray to point p can be defined as: l_0 + l * t = p, where:
        //the origin of the ray
        Vector3 l_0 = ray.origin;
        //l is the direction of the ray
        Vector3 l = ray.direction;
        //t is the length of the ray, which we can get by combining the above equations:
        //t = ((p_0 - l_0) . n) / (l . n)

        //But there's a chance that the line doesn't intersect with the plane, and we can check this by first
        //calculating the denominator and see if it's not small. 
        //We are also checking that the denominator is positive or we are looking in the opposite direction
        float denominator = Vector3.Dot(l, n);

        if (denominator > 0.00001f)
        {
            //The distance to the plane
            float t = Vector3.Dot(p_0 - l_0, n) / denominator;

            //Where the ray intersects with a plane
            Vector3 p = l_0 + l * t;

            return p;
        }

        return Vector3.negativeInfinity;
    }

    void Update()
    {
        hit = new RaycastHit();
        currentRay = new Ray();

        if (vRHands.VRActive)
        {
            Ray rightRay = new Ray(RightCon.position, RightCon.forward);
            if (Physics.Raycast(rightRay, 100f, layerMask) || Physics.Raycast(ReverseRay(rightRay, 100f), layerMask))
                currentRay = rightRay;
                
            Ray leftRay = new Ray(LeftCon.position, LeftCon.forward);
            if (Physics.Raycast(leftRay, 100f, layerMask) || Physics.Raycast(ReverseRay(leftRay, 100f), layerMask))
                currentRay = leftRay;
        }
        else
        {
            currentRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        }

        if (enableDragPoint)
        {
            p = IntersectPlane(currentRay);
            currDraggingPoint.GetComponent<Point>().DragPosition(p);
        }

        if (vRHands.VRActive)
        {
            if (vRHands.IsLeftHandOverUI() || vRHands.IsRightHandOverUI())
            {
                hoverPoint.SetActive(false);
                return;
            }
        }
        else if (EventSystem.current.IsPointerOverGameObject())
        {
            hoverPoint.SetActive(false);
            return;
        }

        ResetHoveringPoint();

        if (Physics.Raycast(currentRay, out hit, 100f, layerMask) ||
            Physics.Raycast(ReverseRay(currentRay, 100f), out hit, 100f, layerMask))
        {
            // Hovering on the graph
            if (hit.transform.gameObject.layer == graphLayer && !enableDragPoint)
            {
                hoverPoint.transform.position = new Vector3(hit.point.x, hit.point.y, hit.transform.position.z);
                hoverPoint.transform.localScale = new Vector3(1f, 1f, hit.transform.GetComponent<MeshCreator>().width / 2f);
                hoverPoint.SetActive(true);
            }
            else if (hit.transform.gameObject.layer == pointLayer)
            {
                hoverPoint.SetActive(false);
                hit.transform.GetComponentInParent<Point>().Hover();
                currHoveringPoint = hit.transform.gameObject;
            }
            else if (hit.transform.gameObject.layer == handleLayer)
            {
                hit.transform.GetComponentInParent<Point>().Hover();
                currHoveringHandle = hit.transform.gameObject;
            }
        }
        else
        {
            hoverPoint.SetActive(false);
        }

    }

    void ResetHoveringPoint()
    {
        if (currHoveringPoint != null)
        {
            currHoveringPoint.GetComponentInParent<Point>().UnHover();
            currHoveringPoint = null;
        }

        if (currHoveringHandle != null)
        {
            currHoveringHandle.GetComponentInParent<Point>().UnHover();
            currHoveringHandle = null;
        }
    }

    void Click(InputAction.CallbackContext obj)
    {
        if (hoverPoint.activeInHierarchy)
        {
            ASL.ASLHelper.InstantiateASLObject("point", hit.transform.InverseTransformPoint(hoverPoint.transform.position), Quaternion.identity, hit.transform.name);
        }
        else if (currHoveringPoint != null)
        {
            currHoveringPoint.GetComponentInParent<Point>().Select();
        }
        else if (currHoveringHandle != null)
        {
            currDraggingPoint = currHoveringHandle.transform.parent.gameObject;
            enableDragPoint = true;
        }
    }

    void RightClick(InputAction.CallbackContext obj)
    {
        if (currHoveringPoint != null)
        {
            currHoveringPoint.GetComponentInParent<ASL.ASLObject>().SendAndSetClaim(() =>
            {
                currHoveringPoint.GetComponentInParent<ASL.ASLObject>().DeleteObject();
            });
        }
    }

    void Release(InputAction.CallbackContext obj)
    {
        currDraggingPoint = null;
        enableDragPoint = false;
    }

    Ray ReverseRay(Ray ray, float length)
    {
        return new Ray(ray.origin + ray.direction * length, -ray.direction);
    }
}