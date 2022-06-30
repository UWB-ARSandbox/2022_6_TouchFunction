using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeFxText : MonoBehaviour
{
    // Start is called before the first frame update
    public void onChanged(int newFx)
    {
        string newString;
        switch (newFx)
        {
            case 0:
                newString = "Y = 5 Sin(x/3) + 5";
                break;
            case 1:
                newString = "Y = -0.5 x + 10";
                break;
            case 2:
                newString = "x^2 / 2";
                break;
            default:
                newString = "Y = 5 Sin(x/3) + 5";
                break;
        }
        GetComponent<TextMesh>().text = newString;
    }
}
