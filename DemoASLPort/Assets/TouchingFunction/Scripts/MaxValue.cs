using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaxValue : MonoBehaviour
{
    public void onValChanged(System.Single newVal)
    {
        GetComponent<Text>().text = newVal.ToString();
    }
}
