using System;
using System.Collections;
using System.Collections.Generic;
//using System.Windows.Forms;
using UnityEngine;
using Genbox.WolframAlpha;
using Genbox.WolframAlpha.Responses;
using Genbox.WolframAlpha.Objects;
using System.Linq;
using System.Text.RegularExpressions;

public class WolframAlpha : MonoBehaviour
{
    public delegate void ObtainPointsEvent(List<float> points);
    public static event ObtainPointsEvent onObtainPoints;

    WolframAlphaClient client;
    // Start is called before the first frame update
    void Awake()
    {
        //Create the client.
        client = new WolframAlphaClient("GHE9KT-EP5Y898AAL");
    }


    public async void Solve(string equation, float start, float end, float inc){
        string input = "Table[" + equation + "], {x, " + start + ", " + end + ", " + inc + "}]";

        Debug.Log(input);

        FullResultResponse results = await client.FullResultAsync(input);

        string resultString = "";
        foreach (Pod pod in results.Pods)
        {
            if(pod.Title == "Result"){
                //Debug.Log(pod.Title + ":");

                foreach (SubPod subPod in pod.SubPods)
                {
                    
                    if (string.IsNullOrEmpty(subPod.Plaintext))
                        Debug.Log("<Cannot output in console>");
                    else{
                        // Debug.Log("Sub Pod: " + subPod.Plaintext);
                        resultString = subPod.Plaintext;
                    }
                }
            }
        }
        // Debug.Log("Result String: " + resultString);

        onObtainPoints?.Invoke(cleanResults(resultString));
    }

    static List<float> cleanResults(string toClean){
        // string trim = toClean.Replace(" ", "");
        // char breakOn = ',';
        // string[] broken = trim.Split(breakOn);
        // broken[0] = broken[0].Substring(1,broken[0].Length - 1);
        // broken[broken.Length -1] = broken[broken.Length-1].Substring(0, broken[broken.Length-1].Length -1);
        // float[] cleaned = new float[broken.Length];
        // for (int i = 0; i < broken.Length; i++){
        //     //Debug.Log(str);
        //     cleaned[i] = Convert.ToSingle(broken[i]);
        //     Debug.Log(cleaned[i]);
        // }
        
        return Regex.Replace(toClean, @"{|}|\s", "").Split(',').Select(float.Parse).ToList();;
    }
}
