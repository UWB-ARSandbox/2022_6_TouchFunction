using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollerCoasterControl : MonoBehaviour
{

    public Player[] players;
    public bool isActivated;
    public int maxRider;
    public MeshCreator mesh;

    public Vector3 speed;

    public Vector3 DriverSpeedVector;
    public Vector3 FinalMoveVector;

    public Vector3[] SeatPositions;
    public GameObject[] SafetyBars;

    public Vector3 SlidingVector;
    public Vector3 inheritedVec;
    public Vector3 newSlidingVec;

    LayerMask meshMask;
    public Vector3 norm;
    RaycastHit hit;

    Vector3 lastPos;

    float frictionLoss = 0.005f;


   
    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        isActivated = false;
        maxRider = 4;
        players = new Player[maxRider];
        SeatPositions = new Vector3[4] { new Vector3(-1.2f, 0.7f, 0.95f), new Vector3(0.8f, 0.7f, 0.95f), 
                                        new Vector3(-1.2f, 0.7f, -3.25f), new Vector3(0.8f, 0.7f, -3.25f) };
        meshMask = LayerMask.GetMask("GraphMesh");
    }



    // Update is called once per frame
    void Update()
    {
        if (isActivated)
        {
            if (mesh != null)
            {
                setRotation();
                getSlidingVec();
            }
            //MoveVector = transform.forward * 0.01f;
            
            FinalMoveVector = DriverSpeedVector;
            FinalMoveVector += SlidingVector;
            transform.position += FinalMoveVector;
            DriverSpeedVector = Vector3.zero;
        }
        //Debug.DrawRay(transform.position, -transform.up, Color.green, 10f);
        
        
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
        if (!isActivated)
        {
            isActivated = true;
        }
        for (int i = 0; i < maxRider; i++)
        {
            if (players[i] == null)
            {
                players[i] = p;
                SafetyBars[i].transform.localEulerAngles = new Vector3(165, 0, 0);
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
                players[i] = null;
            }
        }
    }


    void setRotation()
    {
        if (Physics.Raycast(transform.position + 0.5f * transform.up, -transform.up, out hit, 5, meshMask))
        {
            if (hit.transform.GetComponent<MeshCreator>() == mesh)
            {
                
                norm = hit.normal;
                transform.position = hit.point + 0.3f * norm;
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
            FinalMoveVector = Vector3.zero;
            
        }
    }

    void getSlidingVec()
    {
        
        newSlidingVec = (norm + Mathf.Sqrt(1 + (norm.x / norm.y) * (norm.x / norm.y)) * Vector3.down) * frictionLoss;
        inheritedVec = newSlidingVec.normalized * FinalMoveVector.magnitude * Vector3.Dot(newSlidingVec.normalized, FinalMoveVector.normalized);
        SlidingVector = newSlidingVec + inheritedVec;

        //if (transform.position)

    }



}
