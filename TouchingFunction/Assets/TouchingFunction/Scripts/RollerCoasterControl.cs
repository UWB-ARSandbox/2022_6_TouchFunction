using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollerCoasterControl : MonoBehaviour
{

    public Player[] players;
    
    public int maxRider;
    //public MeshCreator mesh;

    public Vector3 DriverMoveVector;
    public Vector3 FinalMoveVector;

    public Vector3[] SeatPositions;
    public GameObject[] SafetyBars;


    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        maxRider = 4;
        players = new Player[maxRider];
        SeatPositions = new Vector3[4] { new Vector3(-1.2f, 0.7f, 0.95f), new Vector3(0.8f, 0.7f, 0.95f), 
                                        new Vector3(-1.2f, 0.7f, -3.25f), new Vector3(0.8f, 0.7f, -3.25f) };
    }



    // Update is called once per frame
    void Update()
    {
        //MoveVector = transform.forward * 0.01f;
        FinalMoveVector = DriverMoveVector;
        transform.position += FinalMoveVector;
        DriverMoveVector = Vector3.zero;
        //FinalMoveVector = Vector3.zero;
    }
/*
    private void FixedUpdate()
    {

        
    }*/

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

    void getMoveOnMesh()
    {

    }
}
