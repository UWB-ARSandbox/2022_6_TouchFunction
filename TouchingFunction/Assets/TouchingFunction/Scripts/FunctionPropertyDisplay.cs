using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ASL;
using System.Linq;

public class FunctionPropertyDisplay : MonoBehaviour
{
    ASLObject aslObj;
    Text text;
    private bool isFPDEmpty;
    public bool FPDButtonClaimed;
    public string FPDText;
    public GameObject FuncitonPropertyDisplay;
    public FunctionProperty FP;
    public GameObject FDPButton;

    void Awake()
    {
        //text = GetComponentInChildren<Text>();
        //FP = GetComponentInChildren<FunctionProperty>();
        isFPDEmpty = true;
        FPDButtonClaimed = true;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        aslObj = GetComponent<ASLObject>();
        aslObj._LocallySetFloatCallback(ReceivePropertiesFromNetwork);
        //WolframAlpha.onObtainFunctionInfo += SendPropertiesToNetwork;
    }

    // void Update()
    // {
    //     FDPButton.transform.position = new Vector3(FDPButton.transform.position.x, FDPButton.transform.position.y, GameObject.Find("Canvas").transform.position.z);
    // }

    public void SendPropertiesToNetwork(Dictionary<string, string> dictionary)
    {
        Debug.Log("in SendFLoat");
        string str = dictionary
            .Select((kvp) => kvp.Key + ": " + kvp.Value + '\n')
            .Aggregate((a, b) => a + b);

        float[] dic = StringToFloatArray.SToF(str);
        float[] toSend = new float[dic.Length + 1];
        Array.Copy(dic, 0, toSend, 1, dic.Length);
        toSend[0] = 1;
        aslObj.SendAndSetClaim(() =>
        {
            aslObj.SendFloatArray(toSend);
        });
    }

    public void SendDelete()
    {
        float[] toSend = new float[] {0};
        aslObj.SendAndSetClaim(() =>
        {
            aslObj.SendFloatArray(toSend);
        });
    }

    

    public void ReceivePropertiesFromNetwork(string _id, float[] _f)
    {
        switch(_f[0]){
            case 0:
                Debug.Log("recieved delete!");
                FDPButton.SetActive(false);
                int ListIndex = FindObjectOfType<GraphListButtonControl>().GetListID(FDPButton);
                Debug.Log("index: " + ListIndex);
                if(ListIndex != -1)
                {
                    FindObjectOfType<FuncitonPropertyManager>().ReleaseButton(ListIndex);
                    FindObjectOfType<GraphListButtonControl>().releaseButton(ListIndex);
                    ResetFunctionProperty();
                }
                break;
            case 1:
                float[] toProcess = new float[_f.Length -1];
                Array.Copy(_f, 1, toProcess, 0, _f.Length-1);
                FPDText = StringToFloatArray.FToS(toProcess);
                isFPDEmpty = false;
                break;
        }
        //text.text = StringToFloatArray.FToS(_f);
        
    }

    public bool isEmpty()
    {
        return isFPDEmpty;
    }

    // Returns true if button not claimed. false if button is claimed.
    public bool isFPDButtonClaimed()
    {
        return FPDButtonClaimed;
    }

    public void ClaimFDPButton()
    {
        FPDButtonClaimed = false;
    }

    public void ReleaseFDPButton()
    {
        FPDButtonClaimed = true;
        //ResetFunctionProperty();
    }

    public string GetFPDText()
    {
        if(FPDText != null)
        {
            return FPDText;
        }
        else 
        {
            return null;
        }
    }

    public void SetFunctionPropertyDisplayActive()
    {
        if(FuncitonPropertyDisplay.active)
        {
            FindObjectOfType<FuncitonPropertyManager>().ResetUI();
        }
        else
        {
            FindObjectOfType<FuncitonPropertyManager>().ResetUI();
            FuncitonPropertyDisplay.SetActive(!FuncitonPropertyDisplay.active);
        }
    }

    public void ResetFunctionProperty()
    {
        FPDText = "";
        isFPDEmpty = true;
        if(FuncitonPropertyDisplay.active)
        {
            FP.ClearText();
        }
        else
        {
            FuncitonPropertyDisplay.SetActive(true);
            FP.ClearText();
            FuncitonPropertyDisplay.SetActive(false);
        }
    }
}
