using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    // Instantiates a player for each client
    void Start()
    {
        ASL.ASLHelper.InstantiateASLObject("PlayerPre", Vector3.zero, Quaternion.identity, null, null, OnPlayerCreated);
    }

    private static void OnPlayerCreated(GameObject obj)
    {
        obj.layer = 6;
        obj.GetComponent<PlayerASL>().SetLocal();
        obj.GetComponent<PlayerASL>().InitLocalPlayer();
        obj.GetComponent<PlayerASL>().nameText.gameObject.SetActive(false);

        obj.GetComponent<Player>().enabled = true;
        obj.GetComponentInChildren<Camera>().enabled = true;
    }
}