using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphListButtonControl : MonoBehaviour
{
    public bool[] buttonClaimed = new bool[] {true, true, true, true};
    public GameObject[] ButtonList = new GameObject[4];
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool needButton(int i)
    {
        return buttonClaimed[i];
    }

    public void claimButton(int i, GameObject b)
    {
        buttonClaimed[i] = false;
        ButtonList[i] = b;
    }

    public void releaseButton(int i)
    {
        buttonClaimed[i] = true;
        ButtonList[i] = null;
    }

    public GameObject GetButton(int i)
    {
        return ButtonList[i];
    }

    public int GetListID(GameObject b)
    {
        for(int i = 0; i < ButtonList.Length; i++)
        {
            if(b == ButtonList[i])
            {
                return i;
            }
        }
        return -1;
    }

}
