using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ASL;

public class RollerCoasterControl : MonoBehaviour
{

    public GameObject rc;
    public Player[] players;
    public int[] playerPIDs;
    public bool isActivated;
    public int maxRider = 4;
    public MeshCreator mesh;

    public Vector3 speed;
    public Vector3 driverSpeed;
    public Vector3 SlideSpeed;
    public Vector3 InheritedSpeed;




    public Vector3 DriverSpeedVector;
    public Vector3 FinalMoveVector;

    public Vector3[] SeatPositions;
    public GameObject[] SafetyBars;

    public Vector3 SlidingVector;
    public Vector3 inheritedVec;

    public Vector3 MeshOffsetAdjustment;
    LayerMask meshMask;
    public Vector3 norm;
    RaycastHit hit;

    public float ConversionRate = 0.05f;
    public float InheritRate = 1f;

    public float speedLimit = 2f;

    public float InitHeight;

    public bool isEnd;
    public bool isDerail = false;
    public float derailLastTime = 5f;

    Vector3 initPosCopy;
    Vector3 initRotCopy;

    public RollerCoasterASL RCASL;
    //public int addCount = 0;
    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        isActivated = false;
        players = new Player[4];
        SeatPositions = new Vector3[4] { new Vector3(-1.2f, 0.7f, .75f), new Vector3(0.8f, 0.7f, .75f), 
                                        new Vector3(-1.2f, 1.5f, -3f), new Vector3(0.8f, 1.5f, -3f) };
        meshMask = LayerMask.GetMask("GraphMesh");
        initPosCopy = transform.position;
        initRotCopy = transform.eulerAngles;

        RCASL = GetComponent<RollerCoasterASL>();
    }



    // Update is called once per frame
    void Update()
    {

        alignPlayers();
        if (mesh == null) // if it's a ground car
        {
            FinalMoveVector = 10 * DriverSpeedVector;
            limitSpeed();
            transform.position += FinalMoveVector;
            //alignPlayers();
            DriverSpeedVector = Vector3.zero;
            return;
        }


        if (!isActivated)
        {
            isActivated = (DriverSpeedVector != Vector3.zero);
        }

        if (isActivated)
        {

            setRotation();

            if (isDerail)
            {
                transform.eulerAngles += new Vector3(30, 10, 60) * Time.deltaTime;
                
                derailLastTime -= Time.deltaTime;
                if (derailLastTime <= 0)
                {
                    resetRC();
                    return;
                }
                //alignPlayers();
            } else
            {
                getSlidingVec();
            }


            FinalMoveVector = DriverSpeedVector;
            if (!isEnd)
            {
                FinalMoveVector += SlidingVector;
                FinalMoveVector += MeshOffsetAdjustment;
            }

            limitSpeed();
            checkForGoingOver();
            //Debug.DrawRay(transform.position, FinalMoveVector, Color.green, 0.01f);
            transform.position += FinalMoveVector;
            //alignPlayers();
            DriverSpeedVector = Vector3.zero;
        }
        
        //Debug.DrawRay(transform.position, -transform.up, Color.green, 10f);
    }


    void alignPlayers()
    {
        for (int i = 0; i < maxRider; i++)
        {
            if (players[i] != null)
            {
                players[i].transform.localPosition = 
                                Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y,transform.eulerAngles.z) * SeatPositions[i] + transform.position;
                players[i].transform.eulerAngles = transform.eulerAngles;               
            }
        }
    }

    void setRotation()
    {
        if (Physics.Raycast(transform.position + 0.5f * transform.up, -transform.up, out hit, 5, meshMask))
        {
            if (hit.transform.GetComponent<MeshCreator>() == mesh)
            {
                isEnd = false;
                norm = hit.normal;
                MeshOffsetAdjustment = hit.point + 0.3f * norm - transform.position;
                float angle = Vector3.Angle(norm, Vector3.up);
                //Debug.LogError(angle);
                if (norm.x < 0)
                {
                    angle *= -1;
                }
                transform.eulerAngles = new Vector3(angle, 90, 0);
                //transform.position(transform.position.x, mesh.yVals);
            }
        } else
        {
            Vector3 cp = mesh.FindClosestPoint(transform.position);
            if (cp.x <= mesh.minVal+2f || cp.x >= mesh.maxVal-2f)
            {
                isEnd = true;
            } else
            {
                isDerail = true;
            }
            
        }
    }

    void getSlidingVec()
    {
        
        Vector3 newSlidingVec = (norm + Mathf.Sqrt(1 + (norm.x / norm.y) * (norm.x / norm.y)) * Vector3.down) * ConversionRate;
        if (newSlidingVec.magnitude > 0.000001f)
        {
            inheritedVec = InheritRate * newSlidingVec.normalized * FinalMoveVector.magnitude * Vector3.Dot(newSlidingVec.normalized, FinalMoveVector.normalized);
        } else
        {
            inheritedVec = FinalMoveVector * 0.95f;
        }
        SlidingVector = newSlidingVec + inheritedVec;

        //if (transform.position + SlidingVector)
    }

    void checkForGoingOver()
    {
        int c = 0;
        
        while (Vector3.Dot(hit.normal, transform.position + FinalMoveVector - hit.point) <= 0 // if the end point is below hit plane
            //!mesh.IsAboveGraph(transform.position + FinalMoveVector)  // if the end point is below graph
            && c<360)
        {
            //Debug.LogError("Going Over");
            //FinalMoveVector += OffSetIncrement * Vector3.up;
            if (FinalMoveVector.x > 0)
            {
                FinalMoveVector = Quaternion.Euler(0, 0, -1f) * FinalMoveVector;
            } else
            {
                FinalMoveVector = Quaternion.Euler(0, 0, 1f) * FinalMoveVector;
            }
            c++;
        }

    }

    void limitSpeed()
    {
        if (FinalMoveVector.magnitude > speedLimit)
        {
            FinalMoveVector = FinalMoveVector.normalized * speedLimit;
        }
    }

    private void OnTriggerStay(Collider other)
    {

        // if collide with a player, 
        if (other.gameObject.layer == 6)
        {
            Player p = other.GetComponent<Player>();
            if (!p.isRiding)
            {
                p.SetEnterVehicleGUI(true);
                p.RCControlToRide = this;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            Player p = other.GetComponent<Player>();
            p.SetEnterVehicleGUI(false);
            p.RCControlToRide = null;
        }
    }

    public bool IsDriver(Player p)
    {
        if (players[0] != null)
        {
            return players[0] == p;
        }
        return false;
    }

    public int AddRider(Player p)
    {
/*        if (!isActivated)
        {
            isActivated = true;
        }*/
        for (int i = 0; i < maxRider; i++)
        {
            if (players[i] == null)
            {
                players[i] = p;               
                RCASL.SendSeatsData();
                SafetyBars[i].transform.localEulerAngles = new Vector3(165, 0, 0);
                //Debug.LogError(i);
                alignPlayers();
                return i;
            }
        }
        return -1;
    }

    public void KickPlayer(Player p)
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (p == players[i])
            {
                SafetyBars[i].transform.localEulerAngles = new Vector3(80, 0, 0);
                p.isRiding = false;
                players[i] = null;
                RCASL.SendSeatsData();
            }
        }
    }

    void resetRC()
    {
        SlidingVector = Vector3.zero;
        isDerail = false;

        transform.position = initPosCopy;
        transform.eulerAngles = initRotCopy;
        derailLastTime = 5;
        isActivated = false;

        alignPlayers();


    }



}
