using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointHandle : MonoBehaviour
{
    public void Hover()
    {
        GetComponent<MeshRenderer>().material.color = Color.yellow;
    }

    public void UnHover()
    {
        GetComponent<MeshRenderer>().material.color = Color.white;
    }
}
