using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphRes : MonoBehaviour
{
    public void onResChanged(System.Single val)
    {
        GetComponent<Text>().text = val.ToString();
    }
}
