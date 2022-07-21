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

    // Ensures that the ASLObject is initialized
    // You can also do this in the callback if you prefer, but that has to be static.
    IEnumerator DelayedInit()
    {
        while (_playerObject == null)
        {
            yield return null;
        }

        _playerObject.GetComponent<Player>().enabled = true;
        //_playerObject.GetComponentInChildren<Camera>().enabled = true;

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