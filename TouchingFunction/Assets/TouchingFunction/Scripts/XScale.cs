using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XScale : MonoBehaviour
{
    public GameObject number;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        number.transform.rotation = Quaternion.LookRotation(number.transform.position - Camera.main.transform.position, Vector3.up);
    }
}
