using System;
using System.Collections;
using System.Collections.Generic;
//using System.Windows.Forms;
using UnityEngine;
using Genbox.WolframAlpha;
using Genbox.WolframAlpha.Responses;
using Genbox.WolframAlpha.Objects;

public class WolframTest : MonoBehaviour
{

    WolframAlphaClient client;
    // Start is called before the first frame update
    async void Start()
    {
        //Create the client.
        client = new WolframAlphaClient("GHE9KT-EP5Y898AAL");

        Solve("x^3", -5f, 5f, .5f);
/*
        //We start a new query.
        FullResultResponse results = await client.FullResultAsync("solve x^3 at x = -5,-4.5,-4,-3.5,-3,-2.5,-2,-1.5,-1,-.5,0,.5,1,1.5,2,2.5,3,3.5,4,4.5,5");

        //Results are split into "pods" that contain information.
        foreach (Pod pod in results.Pods)
        {
            Debug.Log(pod.Title + ":");

            foreach (SubPod subPod in pod.SubPods)
            {
                if (string.IsNullOrEmpty(subPod.Plaintext))
                    Debug.Log("<Cannot output in console>");
                else
                    Debug.Log(subPod.Plaintext);
            }
        }
        */
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    async float[] Solve(string equation, float start, float end, float inc){
        string buildString = "Solve " + equation + " at x = ";
        for(float i = start; i <= end; i+= inc){
            buildString += i;
            if(i != end){
                buildString += ",";
            }
        }

        Debug.Log(buildString);

        FullResultResponse results = await client.FullResultAsync(buildString);

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
                    //Debug.Log(subPod.Plaintext);
                    resultString = subPod.Plaintext;
                }
            }
            }
        }
        //Debug.Log(resultString);
        float[] cleanedResults = new float [1];
        cleanedResults = cleanResults(resultString);

    }

    float[] cleanResults(string toClean){
        string trim = toClean.Replace(" ", "");
        char breakOn = ',';
        string[] broken = trim.Split(breakOn);
        broken[0] = broken[0].Substring(1,broken[0].Length - 1);
        broken[broken.Length -1] = broken[broken.Length-1].Substring(0, broken[broken.Length-1].Length -1);
        float[] cleaned = new float[broken.Length];
        for (int i = 0; i < broken.Length; i++){
            //Debug.Log(str);
            cleaned[i] = Convert.ToSingle(broken[i]);
            Debug.Log(cleaned[i]);
        }

        return cleaned;
    }
}
