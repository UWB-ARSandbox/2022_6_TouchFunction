using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ASL;

public class PlayerSpawn : MonoBehaviour
{
    private static GameObject _playerObject = null;
    private static ASLObject _playerAslObject = null;

    private static readonly float UPDATES_PER_SECOND = 30.0f;

    public Transform[] playerSpawn;
    public int playerID;

    // Instantiates a player for each client
    void Start()
    {
        playerID = ASL.GameLiftManager.GetInstance().m_PeerId;

        //ASLHelper.InstantiateASLObject("PlayerPre", playerSpawn[playerID -1].position, Quaternion.identity, null, null, OnPlayerCreated);
        ASLHelper.InstantiateASLObject("PlayerPre", Vector3.zero, Quaternion.identity, null, null, OnPlayerCreated);

        StartCoroutine(DelayedInit());
        StartCoroutine(NetworkedUpdate());
    }

    private static void OnPlayerCreated(GameObject obj)
    {
        _playerObject = obj;
        _playerAslObject = obj.GetComponent<ASLObject>();
        obj.layer = 6;
        obj.GetComponent<Player>().m_MeshManager = FindObjectOfType<MeshManager>();
        FindObjectOfType<ChangeColor>().playerASL = _playerObject.GetComponent<PlayerASL>();
    }

    // Ensures that the ASLObject is initialized
    // You can also do this in the callback if you prefer, but that has to be static.
    IEnumerator DelayedInit()
    {
        while (_playerObject == null)
        {
            yield return null;
        }

        _playerObject.GetComponent<Player>().enabled = true;
        _playerObject.GetComponentInChildren<Camera>().enabled = true;

        yield return new WaitForSeconds(1f);
        _playerAslObject.SendAndSetClaim(() =>
        {
            _playerAslObject.SendFloatArray(new float[] { 0, GameLiftManager.GetInstance().m_PeerId });
        });
    }

    // Putting your update in a coroutine allows you to run it at a rate of your choice
    IEnumerator NetworkedUpdate()
    {
        while (true)
        {
            if (_playerObject == null)
                yield return new WaitForSeconds(0.1f);

            _playerAslObject.SendAndSetClaim(() =>
            {
                // Debug.Log(transform.position);
                _playerAslObject.SendAndSetWorldPosition(_playerAslObject.transform.position);
                // _playerAslObject.SendAndSetLocalRotation(_playerAslObject.transform.localRotation);
            });

            yield return new WaitForSeconds(1 / UPDATES_PER_SECOND);
        }
    }
}