using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ASL;

public class Player : MonoBehaviour
{
    private static GameObject _playerObject = null;
    private static ASLObject _playerAslObject = null;

    private static readonly float UPDATES_PER_SECOND = 10.0f;
    
    // Instantiates a player for each client
    void Start()
    {
        ASLHelper.InstantiateASLObject("Player", Vector3.zero, Quaternion.identity, null, null, OnPlayerCreated);
        
        StartCoroutine(DelayedInit());
        StartCoroutine(NetworkedUpdate());
    }

    private static void OnPlayerCreated(GameObject obj)
    {
        _playerObject = obj;
        _playerAslObject = obj.GetComponent<ASLObject>();
    }

    // Ensures that the ASLObject is initialized
    // You can also do this in the callback if you prefer, but that has to be static.
    IEnumerator DelayedInit()
    {
        while (_playerObject == null)
        {
            yield return null;
        }

        _playerObject.GetComponent<PlayerMovement>().enabled = true;
        _playerObject.GetComponent<PlayerMovement>().playerCam.SetActive(true);
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
                // Debug.Log(transform.position);
                _playerAslObject.SendAndSetWorldPosition(_playerAslObject.transform.position);
            });
            
            yield return new WaitForSeconds(1 / UPDATES_PER_SECOND);
        }
    }
}