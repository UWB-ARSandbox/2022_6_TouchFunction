using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using ASL;

public class GraphManipulation : MonoBehaviour
{
    ASLObject aslObj;

    public GameObject xAxis;
    public GameObject yAxis;

    public GameObject xAxisEnd;
    public GameObject yAxisEnd;

    public GameObject xScale;
    public GameObject yScale;

    public GameObject meshManager;

    MeshManager meshManagerScript;

    double xIntervalSize;
    double yIntervalSize;
    int xNumIntervals;
    int yNumIntervals;
    double xIntervalSpace;
    double yIntervalSpace;

    double xAxisLength;
    double yAxisLength;


    // Start is called before the first frame update
    void Start()
    {
        xIntervalSize = 1.0;
        yIntervalSize = 1.0;
        xNumIntervals = 10;
        yNumIntervals = 10;

        xAxisLength = (double)xAxis.transform.localScale.y;
        yAxisLength = (double)yAxis.transform.localScale.y;

        CalcIntervalSpace();
        SetupXScales();
        SetupYScales();

        meshManagerScript = meshManager.GetComponent<MeshManager>();

        aslObj = GetComponent<ASLObject>();
        aslObj._LocallySetFloatCallback(onFloatArrayReceived);
    }

    void CalcIntervalSpace()
    {
        xIntervalSpace = xAxisLength / xNumIntervals;
        yIntervalSpace = yAxisLength / yNumIntervals;
    }

    void CalcNumIntervals()
    {
        Debug.Log("Here");
        Debug.Log(xAxisLength);
        Debug.Log(yAxisLength);
        xNumIntervals = (int)(xAxisLength / xIntervalSpace);
        yNumIntervals = (int)(yAxisLength / yIntervalSpace);
        Debug.Log(xNumIntervals);
        Debug.Log(yNumIntervals);
        Debug.Log("Going back");
    }

    void SetupXScales()
    {
        double[] xAxisCoordinates = CalcAxisCoordinates(xNumIntervals, xIntervalSize);
        Vector3 origin = gameObject.transform.position;
        for (int i = 0; i < xAxisCoordinates.Length; i++)
        {
            GameObject scale = Instantiate(xScale);
            Vector3 scalePos = new Vector3((i + 1) * (float)xIntervalSpace + origin.x, origin.y, origin.z);
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
            Vector3 scalePos = new Vector3(origin.x, (i + 1) * (float)yIntervalSpace + origin.y, origin.z);
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
            SendGraphDetails();
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
            SendGraphDetails();
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
            SendGraphDetails();
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
            SendGraphDetails();
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
            SendGraphDetails();
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
            meshManagerScript.UpdateGraphs();
            SendGraphDetails();
        }
    }

    public void SetXAxisLength(double axisLength)
    {
        if (axisLength > 0)
        {
            xAxisLength = axisLength;
            SetupXAxis();
            ResetScales(xAxis);
            CalcNumIntervals();
            SetupXScales();
            meshManagerScript.UpdateGraphs();
            SendGraphDetails();
        }
    }

    public void SetupXAxis()
    {
        Vector3 axisScale = xAxis.transform.localScale;
        xAxis.transform.localScale = new Vector3(axisScale.x, (float)xAxisLength, axisScale.z);

        Vector3 axisPos = xAxis.transform.localPosition;
        double xPos = -(xAxisLength / 2);
        xAxis.transform.localPosition = new Vector3((float)xPos, axisPos.y, axisPos.z);

        Vector3 endPos = xAxisEnd.transform.localPosition;
        double endPosX = -(xAxisLength + 0.8);
        xAxisEnd.transform.localPosition = new Vector3((float)endPosX, 0, 0);
    }

    public void SetYAxisLength(double axisLength)
    {
        if (axisLength > 0)
        {
            yAxisLength = axisLength;
            SetupYAxis();
            ResetScales(yAxis);
            CalcNumIntervals();
            SetupYScales();
            meshManagerScript.UpdateGraphs();
            SendGraphDetails();
        }
    }

    public void SetupYAxis()
    {
        Vector3 axisScale = yAxis.transform.localScale;
        yAxis.transform.localScale = new Vector3(axisScale.x, (float)yAxisLength, axisScale.z);

        Vector3 axisPos = yAxis.transform.localPosition;
        double yPos = yAxisLength / 2;
        yAxis.transform.localPosition = new Vector3(axisPos.x, (float)yPos, axisPos.z);

        Vector3 endPos = yAxisEnd.transform.position;
        double endPosY = yAxisLength + 0.8;
        yAxisEnd.transform.localPosition = new Vector3(0, (float)endPosY, 0);
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

    public double GetXAxisLength()
    {
        return xAxisLength;
    }
    public double GetYAxisLength()
    {
        return yAxisLength;
    }

    public void SendGraphDetails()
    {
        float[] graphDetails = new float[9];
        graphDetails[0] = 0;
        graphDetails[1] = (float)xIntervalSize;
        graphDetails[2] = (float)yIntervalSize;
        graphDetails[3] = (float)xNumIntervals;
        graphDetails[4] = (float)yNumIntervals;
        graphDetails[5] = (float)xIntervalSpace;
        graphDetails[6] = (float)yIntervalSpace;
        graphDetails[7] = (float)xAxisLength;
        graphDetails[8] = (float)yAxisLength;

        aslObj.SendAndSetClaim(() =>
        {
            aslObj.SendFloatArray(graphDetails);
        });
    }

    public void onFloatArrayReceived(string _id, float[] value)
    {
        switch (value[0])
        {
            case 0f:
                xIntervalSize = (double)value[1];
                yIntervalSize = (double)value[2];
                xNumIntervals = (int) value[3];
                yNumIntervals = (int) value[4];
                xIntervalSpace = (double)value[5];
                yIntervalSpace = (double)value[6];
                xAxisLength = (double)value[7];
                yAxisLength = (double)value[8];
                
                ResetScales(xAxis);
                ResetScales(yAxis);
                SetupXAxis();
                SetupYAxis();
                SetupXScales();
                SetupYScales();

                meshManagerScript.UpdateGraphs();

                break;

        }
    }
}
