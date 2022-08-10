using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ASL;
using System.Linq;

public class PlayerSpawn : MonoBehaviour
{
    private static GameObject player;

    public bool AllPlayerReady = false;

    bool[] playerReady;
    int playerCount;

    // Instantiates a player for each client
    void Start()
    {
        ASL.ASLHelper.InstantiateASLObject("PlayerPre", -GameLiftManager.GetInstance().m_PeerId * Vector3.left, Quaternion.identity, null, null, OnPlayerCreated);
        playerCount = GameLiftManager.GetInstance().GetHighestPeerId();
        playerReady = new bool[playerCount];
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

    public static GameObject GetPlayer()
    {
        return player;
    }

    private void Update()
    {
        if (AllPlayerReady)
        {
            return;
        }

        var p = FindObjectsOfType<PlayerASL>();
        foreach(PlayerASL pe in p)
        {
            if (pe.peerId == -1)
            {
                return;
            }
            playerReady[pe.peerId-1] = true;
        }

        AllPlayerReady = playerReady.All(x => x);
    }


}