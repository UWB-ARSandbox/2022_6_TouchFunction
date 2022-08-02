using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    GameObject currHoveringPoint;
    GameObject currDraggingPoint;

    Transform RightCon;
    Transform LeftCon;

    bool enableDragPoint = false;

    LayerMask layerMask;
    int graphLayer;
    int pointLayer;

    RaycastHit hit;

    Vector3 p;
    Vector3 pLeft;
    Vector3 pRight;

    // Start is called before the first frame update
    void Start()
    {
        graphPlane = GameObject.Find("GraphAxes").transform;

        

        pointLayer = LayerMask.NameToLayer("Point");
        graphLayer = LayerMask.NameToLayer("GraphMesh");
        layerMask = LayerMask.GetMask("GraphMesh", "Point");

        playerCam = GetComponentInChildren<Camera>();
        
        hoverPoint = GameObject.Find("hoverPoint");
        hoverPoint.SetActive(false);

        LeftCon = GetComponent<PlayerActivateVRHands>().LeftHand.transform;
        RightCon = GetComponent<PlayerActivateVRHands>().RightHand.transform;
    }

    void Awake()
    {
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

        //var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
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
        // if VR enabled
        if( GetComponent<PlayerActivateVRHands>().VRActive)
        {

            pRight = IntersectPlane(new Ray(RightCon.position, RightCon.forward));
            pLeft = IntersectPlane(new Ray(LeftCon.position, LeftCon.forward));

            if(pLeft != Vector3.negativeInfinity)
            {
                if(Physics.Raycast(pLeft + new Vector3(0f, 1f, 0f), Vector3.down, out hit, 2f, layerMask))
                {
                    p = pLeft;
                }
            }
            if(pRight != Vector3.negativeInfinity)
            {
                if(Physics.Raycast(pRight + new Vector3(0f, 1f, 0f), Vector3.down, out hit, 2f, layerMask))
                {
                    p = pRight;
                }
            }
        }
        else
        {
            p = IntersectPlane(Camera.main.ScreenPointToRay(Input.mousePosition));
        }
        

        if(enableDragPoint)
        {
            if(p != Vector3.negativeInfinity)
                currDraggingPoint.GetComponent<Point>().UpdatePosition(p.x);
        }

        // Hovering on the graph
        if (Physics.Raycast(p + new Vector3(0f, 1f, 0f), Vector3.down, out hit, 2f, layerMask))
        {
            if(hit.transform.gameObject.layer == graphLayer && !enableDragPoint)
            {
                if(currHoveringPoint != null)
                {
                    currHoveringPoint.GetComponentInParent<Point>().UnHover();
                    currHoveringPoint = null;
                }

                hoverPoint.transform.position = new Vector3(hit.point.x, hit.point.y, hit.transform.position.z);
                hoverPoint.SetActive(true);
            }
            else if(hit.transform.gameObject.layer == pointLayer)
            {
                hoverPoint.SetActive(false);

                if(currHoveringPoint != null)
                {
                    currHoveringPoint.GetComponentInParent<Point>().UnHover();
                }

                hit.transform.GetComponentInParent<Point>().Hover();
                currHoveringPoint = hit.transform.gameObject;
            }
        }
        else
        {
            if(currHoveringPoint != null)
            {
                currHoveringPoint.GetComponentInParent<Point>().UnHover();
                currHoveringPoint = null;
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
        else if (currHoveringPoint != null)
        {
            currHoveringPoint.GetComponentInParent<Point>().Select();
            currDraggingPoint = currHoveringPoint.transform.parent.gameObject;
            enableDragPoint = true;
        }
    }

    void RightClick(InputAction.CallbackContext obj)
    {
        
        if(currHoveringPoint != null)
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
}