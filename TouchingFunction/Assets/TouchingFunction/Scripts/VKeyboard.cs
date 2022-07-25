using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VKeyboard : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 1.25f + Camera.main.transform.up * -.45f;

        gameObject.transform.rotation = Quaternion.LookRotation(gameObject.transform.position - Camera.main.transform.position, Vector3.up);
    }
}
