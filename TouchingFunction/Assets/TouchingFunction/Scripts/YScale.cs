using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class YScale : MonoBehaviour
{
    public GameObject number;
    
    // Start is called before the first frame update
    void Start()
    {
        Vector3 pos = number.transform.position;
        float width = number.GetComponent<TextMeshPro>().preferredWidth;
        float xPos = pos.x - (width/2 - 0.28f); // (curr width) / 2 - (single character width) / 2
        number.transform.position = new Vector3(xPos, pos.y, pos.z);
    }

    // Update is called once per frame
    void Update()
    {
        number.transform.rotation = Quaternion.LookRotation(number.transform.position - Camera.main.transform.position, Vector3.up);
    }
}
