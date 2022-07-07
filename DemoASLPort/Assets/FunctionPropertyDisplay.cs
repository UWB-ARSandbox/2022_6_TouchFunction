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

    void Awake()
    {
        text = GetComponentInChildren<Text>();
    }
    // Start is called before the first frame update
    void Start()
    {
        aslObj = GetComponent<ASLObject>();
        aslObj._LocallySetFloatCallback(ReceivePropertiesFromNetwork);
        WolframAlpha.onObtainFunctionInfo += SendPropertiesToNetwork;
    }

    public void SendPropertiesToNetwork(Dictionary<string, string> dictionary)
    {
        string str = dictionary
            .Select((kvp) => kvp.Key + ": " + kvp.Value + '\n')
            .Aggregate((a, b) => a + b);

        aslObj.SendAndSetClaim(() =>
        {
            aslObj.SendFloatArray(StringToFloatArray.SToF(str));
        });
    }

    public void ReceivePropertiesFromNetwork(string _id, float[] _f)
    {
        text.text = StringToFloatArray.FToS(_f);
    }
}
