using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ASL;
using UnityEngine.UI;

public class FuncitonPropertyManager : MonoBehaviour
{
    public FunctionPropertyDisplay[] FunctionPD;
    public GameObject[] DisplayButton;
    public GameObject[] fPDPanel;
    // Start is called before the first frame update
    void Start()
    {
        WolframAlpha.onObtainFunctionInfo += FindNextFPAndSend;
    }

    private int NextEmptyFunctionProperty()
    {
        for (int i = 0; i < FunctionPD.Length; i++)
        {
            if (FunctionPD[i].isEmpty())
            {
                Debug.Log("Found empty FPD");
                return i;
            }
        }
        return -1;
    }

    private void FindNextFPAndSend(Dictionary<string, string> dictionary)
    {
        int nextFPD = NextEmptyFunctionProperty();
        if(nextFPD >= 0)
        {
            Debug.Log("Sending FP to panel");
            FunctionPD[nextFPD].SendPropertiesToNetwork(dictionary);
        }
    }

    public GameObject GetLastButton()
    {
        for (int i = 0; i < FunctionPD.Length; i++)
        {
            // true if button needs claimed. false if button claimed.
            if (FunctionPD[i].isFPDButtonClaimed())
            {
                Debug.Log("Found Last Button");
                FunctionPD[i].ClaimFDPButton();
                // DisplayButton[i].SetActive(true);
                return DisplayButton[i];
            }
        }
        return null;
    }
    
    public void ReleaseButton(int i)
    {
        FunctionPD[i].ReleaseFDPButton();
        FunctionPD[i].ResetFunctionProperty();
    }

    public void ResetUI()
    {
        foreach(GameObject panel in fPDPanel)
        {
            panel.SetActive(false);
        }
    }
}
