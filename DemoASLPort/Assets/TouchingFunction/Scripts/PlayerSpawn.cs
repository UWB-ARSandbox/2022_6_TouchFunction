using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ASL;

public class PlayerSpawn : MonoBehaviour
{
    // The peer id of this player in ASL
    private static GameObject _playerObject = null;
    private static ASLObject _playerAslObject = null;

    private static readonly float UPDATES_PER_SECOND = 10.0f;
    
    // Instantiates a player for each client
    void Start()
    {
        ASLHelper.InstantiateASLObject("Player", Vector3.zero, Quaternion.identity, null, null, OnPlayerCreated);
        
        StartCoroutine(NetworkedUpdate());
    }

    private static void OnPlayerCreated(GameObject obj)
    {
        _playerObject = obj;
        _playerAslObject = obj.GetComponent<ASLObject>();

        _playerObject.GetComponent<Player>().enabled = true;
        _playerObject.GetComponent<Player>().playerCam.SetActive(true);

        _playerAslObject.SendAndSetClaim(() =>
        {
            _playerAslObject.SendAndSetLocalScale(new Vector3(0.3f, 0.3f, (float) GameLiftManager.GetInstance().m_PeerId / 100f));
        });
    }



    // Putting your update in a coroutine allows you to run it at a rate of your choice
    IEnumerator NetworkedUpdate()
    {
        while (true)
        {
            if(_playerObject == null)
                yield return new WaitForSeconds(0.1f);
            
            _playerAslObject.SendAndSetClaim(() =>
            {
                _playerAslObject.SendAndSetWorldPosition(_playerAslObject.transform.position);
            });
            
            yield return new WaitForSeconds(1 / UPDATES_PER_SECOND);
        }
    }
}