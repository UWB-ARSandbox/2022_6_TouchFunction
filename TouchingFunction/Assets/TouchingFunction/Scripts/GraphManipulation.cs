using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GraphManipulation : MonoBehaviour
{
    public GameObject xAxis;
    public GameObject yAxis;

    public GameObject xScale;
    public GameObject yScale;

    public GameObject meshManager;

    MeshManager meshManagerScript;

    double  xIntervalSize;
    double  yIntervalSize;
    int xNumIntervals;
    int yNumIntervals;
    double  xIntervalSpace;
    double  yIntervalSpace;



    // Start is called before the first frame update
    void Start()
    {
        xIntervalSize = 1.0;
        yIntervalSize = 1.0;
        xNumIntervals = 10;
        yNumIntervals = 10;

        CalcIntervalSpace();
        SetupXScales();
        SetupYScales();

        meshManagerScript = meshManager.GetComponent<MeshManager>();
    }

    void CalcIntervalSpace()
    {
        float xAxisLength = xAxis.transform.lossyScale.y;
        xIntervalSpace = (double) xAxisLength / xNumIntervals;

        float yAxisLength = yAxis.transform.localScale.y;
        yIntervalSpace = (double) yAxisLength / yNumIntervals;
    }

    void CalcNumIntervals()
    {
        float xAxisLength = xAxis.transform.lossyScale.y;
        xNumIntervals = (int) ((double) xAxisLength / xIntervalSpace);

        float yAxisLength = yAxis.transform.localScale.y;
        yNumIntervals = (int) ((double) yAxisLength / yIntervalSpace);
    }

    void SetupXScales()
    {
        double[] xAxisCoordinates = CalcAxisCoordinates(xNumIntervals, xIntervalSize);
        Vector3 origin = gameObject.transform.position;
        for (int i = 0; i < xAxisCoordinates.Length; i++)
        {
            GameObject scale = Instantiate(xScale);
            Vector3 scalePos = new Vector3((i + 1) * (float) xIntervalSpace + origin.x, origin.y, origin.z);
            scale.transform.position = scalePos;
            scale.transform.parent = xAxis.transform;
            TextMeshPro number = scale.transform.Find("number").gameObject.GetComponent<TextMeshPro>();
            number.text = xAxisCoordinates[i].ToString();
        }
    }

    void SetupYScales()
    {
        double[] yAxisCoordinates = CalcAxisCoordinates(yNumIntervals, yIntervalSize);
        Vector3 origin = gameObject.transform.position;
        for (int i = 0; i < yAxisCoordinates.Length; i++)
        {
            GameObject scale = Instantiate(yScale);
            Vector3 scalePos = new Vector3(origin.x, (i + 1) * (float) yIntervalSpace + origin.y, origin.z);
            scale.transform.position = scalePos;
            scale.transform.parent = yAxis.transform;
            TextMeshPro number = scale.transform.Find("number").gameObject.GetComponent<TextMeshPro>();
            number.text = yAxisCoordinates[i].ToString();
        }
    }

    double[] CalcAxisCoordinates(int numIntervals, double intervalSize)
    {
        double[] axisCoordinates = new double[numIntervals];
        for (int i = 0; i < axisCoordinates.Length; i++)
        {
            axisCoordinates[i] = (i + 1) * intervalSize;
            Debug.Log(axisCoordinates[i]);
        }

        return axisCoordinates;
    }

    void ResetScales(GameObject axis)
    {
        foreach (Transform scale in axis.transform)
        {
            GameObject.Destroy(scale.gameObject);
        }
    }

    public void SetXIntervalSize(double size)
    {
        if (size > 0)
        {
            ResetScales(xAxis);
            xIntervalSize = size;
            SetupXScales();
            meshManagerScript.UpdateGraphs();
        }
    }

    public void SetYIntervalSize(double size)
    {
        if (size > 0)
        {
            ResetScales(yAxis);
            yIntervalSize = size;
            SetupYScales();
            meshManagerScript.UpdateGraphs();
        }
    }

    public void SetXNumIntervals(int numIntervals)
    {
        if (numIntervals > 0)
        {
            ResetScales(xAxis);
            xNumIntervals = numIntervals;
            CalcIntervalSpace();
            SetupXScales();
            meshManagerScript.UpdateGraphs();
        }
    }

    public void SetYNumIntervals(int numIntervals)
    {
        if (numIntervals > 0)
        {
            ResetScales(yAxis);
            yNumIntervals = numIntervals;
            CalcIntervalSpace();
            SetupYScales();
            meshManagerScript.UpdateGraphs();
        }
    }

    public void SetXIntervalSpace(double intervalSpace)
    {
        if (intervalSpace > 0)
        {
            ResetScales(xAxis);
            xIntervalSpace = intervalSpace;
            CalcNumIntervals();
            SetupXScales();
            meshManagerScript.UpdateGraphs();
        }
    }

    public void SetYIntervalSpace(double intervalSpace)
    {
        if (intervalSpace > 0)
        {
            ResetScales(yAxis);
            yIntervalSpace = intervalSpace;
            CalcNumIntervals();
            SetupYScales();
            Debug.Log("Here");
            meshManagerScript.UpdateGraphs();
            Debug.Log("And back");
        }
    }

    public double GetXIntervalSize()
    {
        return xIntervalSize;
    }
    
    public double GetYIntervalSize()
    {
        return yIntervalSize;
    }

    public int GetXNumIntervals()
    {
        return xNumIntervals;
    }

    public int GetYNumIntervals()
    {
        return yNumIntervals;
    }

    public double GetXIntervalSpace()
    {
        return xIntervalSpace;
    }

    public double GetYIntervalSpace()
    {
        return yIntervalSpace;
    }

    public double GetXUnitSpace()
    {
        double xUnitSize = 1 / xIntervalSize;
        double xUnitSpace = xUnitSize * xIntervalSpace;
        return xUnitSpace;
    }

    public double GetYUnitSpace()
    {
        double yUnitSize = 1 / yIntervalSize;
        double yUnitSpace = yUnitSize * yIntervalSpace;
        return yUnitSpace;
    }
}
