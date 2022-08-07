using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ASL;

public class RollerCoasterASL : MonoBehaviour
{
    public RollerCoasterControl RCControl;
    public ASLObject aslObj;
    private static readonly float UPDATES_PER_SECOND = 60f;
    public bool isLocal = false;
    public bool isDriving = false;

    Coroutine networkCoroutine;

    //Start is called before the first frame update
    void Start()
    {
        /*aslObj = GetComponent<ASLObject>();
        aslObj._LocallySetFloatCallback(Receive);*/
    }

    public void InitVehicle()
    {
        aslObj = GetComponent<ASLObject>();
        aslObj._LocallySetFloatCallback(Receive);
    }

    // Update is called once per frame
    void Update()
    {
        if (aslObj == null)
        {
            if (GetComponent<ASLObject>() != null)
            {
                InitVehicle();
            }
        }
    }
    IEnumerator NetworkUpdate()
    {
        //Debug.LogError("ENTER NETWORK");
        while (aslObj == null)
        {
            //Debug.LogError("ASLObject is null");
            yield return null;
        }

        while (true)
        {
            
            //SendTransform();
            //Debug.LogError("sending RC transform");
            aslObj.SendAndSetClaim(() =>
            {
                aslObj.SendAndSetWorldPosition(transform.position);
                aslObj.SendAndSetWorldRotation(transform.rotation);
            });

            //UpdateRidersPosition();

            yield return new WaitForSeconds(1 / UPDATES_PER_SECOND);
        }
    }


    public void StartASL()
    {
        isLocal = true;
        networkCoroutine = StartCoroutine(NetworkUpdate());
    }

    public void StopASL()
    {
        isLocal = false;
        StopCoroutine(networkCoroutine);
    }

    // send the peerID of passenger/driver currently in this vehicle
    public void SendSeatsData()
    {
        float[] f = new float[5];
        f[0] = 0;
        for (int i = 1; i < 5; i++)
        {
            if (RCControl.players[i-1] == null)
            {
                f[i] = -1;
            } else
            {
                f[i] = RCControl.players[i-1].GetComponent<PlayerASL>().peerId;
            }
        }
        
        aslObj.SendAndSetClaim(() =>
        {
            aslObj.SendFloatArray(f);
        });
    }

    void Receive(string _id, float[] _f)
    {
        if (!string.Equals(aslObj.m_Id, _id)) return;

        switch (_f[0])
        {

            case 0: // setting rider information
                for (int i = 0; i < 4; i++)
                {
                    RCControl.players[i] = findPlayerWithPID((int)_f[i + 1]);
                    if (RCControl.players[i] != null)
                    {
                        RCControl.players[i].PlayerAnimator.SetBool("IsRiding", true);
                    }
                }
                break;

            case 1: // setting mesh data to roller coaster
                var g = FindObjectsOfType<MeshCreator>();
                foreach(MeshCreator m in g)
                {
                    if (m.GraphIndex == _f[1])
                    {
                        RCControl.mesh = m;
                    }
                }
                break;
            case 2:
                if (isLocal) break;
                transform.position = new Vector3(_f[1], _f[2], _f[3]);
                transform.eulerAngles = new Vector3(_f[4], _f[5], _f[6]);
                break;
        }
    }

    Player findPlayerWithPID(int pid)
    {
        if (pid < 0) return null;
        var pList = FindObjectsOfType<PlayerASL>();
        foreach(var p in pList)
        {
            if (p.peerId == pid)
            {
                return p.GetComponent<Player>();
            }
        }
        return null;
    }

    public void SendMeshInfo()
    {
        float[] f = { 1, RCControl.mesh.GraphIndex };
        aslObj.SendAndSetClaim(() => 
        {
            aslObj.SendFloatArray(f);
        });
    }

    void SendRotationArray ()
    {
        float[] f = { 2, transform.position.x, transform.position.y, transform.position.z, 
            transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z };
        aslObj.SendAndSetClaim(() => 
        {
            aslObj.SendFloatArray(f);
        });
    }

}
