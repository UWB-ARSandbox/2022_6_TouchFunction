using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ASL;

public class SpawnCar : MonoBehaviour
{
    public GameObject GroundCarPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
        if (GameLiftManager.GetInstance().m_PeerId == 1)
        {
            ASLHelper.InstantiateASLObject("GroundCar", Vector3.zero - 2*Vector3.up, Quaternion.identity, null, null, onGroundCarCreated);
        }
        //ASL_AutonomousObjectHandler.Instance.InstantiateAutonomousObject(GroundCarPrefab, onGroundCarCreated);
        
    }

    private static void onGroundCarCreated(GameObject _obj)
    {
        _obj.GetComponent<RollerCoasterASL>().InitVehicle();
    }

}