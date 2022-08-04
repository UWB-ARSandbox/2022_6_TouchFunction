using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ASL;

public class PlayerSpawn : MonoBehaviour
{
    private static GameObject player;
    // Instantiates a player for each client
    void Start()
    {
        ASL.ASLHelper.InstantiateASLObject("PlayerPre", -GameLiftManager.GetInstance().m_PeerId * Vector3.left, Quaternion.identity, null, null, OnPlayerCreated);
    }

    private static void OnPlayerCreated(GameObject obj)
    {
        obj.layer = 6;
        obj.GetComponent<PlayerASL>().SetLocal();
        obj.GetComponent<PlayerASL>().InitLocalPlayer();
        obj.GetComponent<PlayerASL>().nameText.gameObject.SetActive(false);

        obj.GetComponent<Player>().enabled = true;
        obj.GetComponentInChildren<Camera>().enabled = true;

        obj.AddComponent<PlayerClickGraph>();
        obj.AddComponent<RCPlayerManager>();
        player = obj;
    }

    public void onQuit()
    {
        player.GetComponent<PlayerASL>().Quit();
    }
}