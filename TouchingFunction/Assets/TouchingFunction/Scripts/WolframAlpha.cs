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
    public delegate void ObtainPointsEvent(float[] points);
    public static event ObtainPointsEvent onObtainPoints;
    // TO FIX
    int index = 0;

/*  
    //Used in GetTangentFunction
    public delegate void ObtainTangentEvent(string tangentLine);
    public static event ObtainTangentEvent onObtainTangent;
*/ /* 
    //Used in GetFuncitonFromPoints
    public delegate void ObtainFunctionFromPointsEvent(string function);
    public static event ObtainFunctionFromPointsEvent onObtainFunction;
*/
    //Used in GetFunctionInfo
    public delegate void ObtainFunctionInfoEvent(Dictionary<string, string> dic);
    public static event ObtainFunctionInfoEvent onObtainFunctionInfo;


    //Original WA APPID:
    string originalWAAppId = "GHE9KT-EP5Y898AAL";
    string userWAAppId = "";
    WolframAlphaClient client;
    // Start is called before the first frame update
    void Awake()
    {
        //Create the client.
        client = new WolframAlphaClient("GHE9KT-EP5Y898AAL");
    }

    public void setAppId(string newAppId){
        userWAAppId = newAppId.ToUpper();
        client = new WolframAlphaClient(userWAAppId);
        Debug.Log("AppId updated. New AppId: " + userWAAppId);
    }

    // Solve: creates a list of y coordinate floats
    // @param: equation -- string containing the funciton to plot
    // @param: start    -- float starting point to plot
    // @param: end      -- float end point of plot
    // @param: inc      -- float increment amount 
    // @returns: list of y coordinate floats
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
        Debug.Log("Result String: " + resultString);

        onObtainPoints?.Invoke(cleanResults(resultString));
    }

    // cleanResults: 
    // @param: toClean -- string containing a list of floats from WA
    // @returns: list of floats
    static float[] cleanResults(string toClean){
        return Regex.Replace(toClean, @"{|}|\s", "").Split(',').Select(float.Parse).ToArray<float>();
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

        // foreach pod, look for the one titled "Interpolating polynomial"
        foreach (Pod pod in results.Pods){
            if(pod.Title == "Interpolating polynomial"){
                foreach (SubPod subPod in pod.SubPods){
                    // if subPod contains Plaintext, save it to return
                    if(!string.IsNullOrEmpty(subPod.Plaintext)){
                        resultString = subPod.Plaintext;
                    }
                }
            }
        }

        // pass tangent funciton into method.
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

        // foreach pod, look for the one that is titled "Results"
        foreach(Pod pod in results.Pods){
            if(pod.Title == "Result"){
                foreach (SubPod subPod in pod.SubPods){

                    // if that SubPod contains a Plaintext, save it to return
                    if(!string.IsNullOrEmpty(subPod.Plaintext)){
                        resultString = subPod.Plaintext;
                    }
                }
            }
        }
        
        // pass function into method.
        onObtainFunction?.Invoke(resultString);

    }
*/
    // GetFuncitonInfo: returns all Plaintext information from WolframAlpha for function.
    // @param: funciton -- string of function to get info for
    // @returns: Dictonary of all Plaintext information
    public async void GetFunctionInfo(string function){
        // query of just the function returns multiple strings of information on the function
        FullResultResponse results = await client.FullResultAsync(function);

        //Dictionary of strings from results
        Dictionary<string, string> dic = new Dictionary<string, string>();

        //for each pod in Pods
        foreach(Pod pod in results.Pods){

            // count for Pods/SubPods with multiple entries
            int SPCount = 1;

            //for each subPod in SubPods
            foreach(SubPod subPod in pod.SubPods){
                
                // if subPod contains Plaintext. if not, do nothing
                if (!string.IsNullOrEmpty(subPod.Plaintext)){
                    
                    // If subPod ! contain a title, use pod.Title
                    if(string.IsNullOrEmpty(subPod.Title)){
                        if(dic.ContainsKey(pod.Title)){
                            // Add the object with a key + an iterator
                            dic.Add(pod.Title + SPCount, subPod.Plaintext);
                            SPCount ++;
                        }
                        else
                        {
                            dic.Add(pod.Title, subPod.Plaintext);
                        }
                    //using subPod.Title
                    }else{
                        // TryAdd attempts to add the object to the dictionary. returns false if entry already exists for that key
                        if(dic.ContainsKey(subPod.Title)){
                            // Add the object with a key + an iterator
                            dic.Add(subPod.Title + SPCount, subPod.Plaintext);
                            SPCount ++;
                        }
                        else
                        {
                            dic.Add(subPod.Title, subPod.Plaintext);
                        }
                    }
                }
            }

            
        }
        //Send dictionary to other process
        onObtainFunctionInfo?.Invoke(dic);
    }
}