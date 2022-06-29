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
    //Used in Solve
    public delegate void ObtainPointsEvent(List<float> points);
    public static event ObtainPointsEvent onObtainPoints;
/* //Used in GetTangentFunction
    public delegate void ObtainTangentEvent(string tangentLine);
    public static event ObtainTangentEvent onObtainTangent;
*/ /* //Used in GetFuncitonFromPoints
    public delegate void ObtainFunctionFromPoints(bool success, string function);
    public static event ObtainFunctionFromPoints onObtainFunction;
*/
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
        
        
        return Regex.Replace(toClean, @"{|}|\s", "").Split(',').Select(float.Parse).ToList();;
    }
/*
    // GetTangentFunction: Gets the tangent function at x
    // @param: originalFunction -- string of original function
    // @param: x                -- x value to get tangent for
    // @returns: invokes set render function and passes the tangent function to is as a string
    public async void GetTangentFunction(string originalFunciton, float x){
        string input = "Tangent line [" + originalFunciton + "] x = " + x;
        FullResultResponse results = await client.FullResultAsync(input);
        string resultString = "";

        foreach (Pod pod in results.Pods){
            if(pod.Title == "Interpolating polynomial"){
                foreach (SubPod subPod in pod.SubPods){
                    if(string.IsNullOrEmpty(subPod.Plaintext)){
                        Debug.Log("<Cannot output in console>");
                    }else{
                        resultString = subPod.Plaintext;
                    }
                }
            }
        }
        onObtainTangent?.Invoke(resultString);

    }
*/ /*
    // GetFunctionFromPoints: Gets a standard y=x function from a series of points
    // @param: points   -- string of points to derive function for
    // @returns: success -- invokes pointed c# success function and passes derived function as string
    // @returns: failure -- invokes pointed c# failure function. equation could not be derived. 
    public async void GetFunctionFromPoints(string points){
        string input = "Curve fit {" + points + "}";

        FullResultResponse results = await client.FullResultAsync(input);
        string resultString = "";

        foreach(Pod pod in results.Pods){
            if(pod.Title == "Result"){
                foreach (SubPod subPod in pod.SubPods){
                    if(string.IsNullOrEmpty(subPod.Plaintext)){
                        Debug.Log("<Cannot output in console>");
                    }else{
                        resultString = subPod.Plaintext;
                    }
                }
            }
        }
        if(resultString != ""){
            onObtainFunction?.Invoke(resultString);
        }else{
            notObtainFunction?.Invoke();
        }
    }
*/ /*
    public async void GetDerivativeFunction(string function){
        string input = "Derivative [" + function + "]"
    }
    */
}
