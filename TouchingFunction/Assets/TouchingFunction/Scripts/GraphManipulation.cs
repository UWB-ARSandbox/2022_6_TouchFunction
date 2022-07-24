using UnityEngine;
using System;
using TMPro;
using ASL;

public partial class GraphManipulation : MonoBehaviour
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

        enabledGraphManip = false;
        selectedType = SelectedType.None;
        selected = null;
        selectedColor = new Color(1, 1, 0, 1);  // yellow
        defaultColor = new Color(1, 1, 1, 1);   // white
    }

    // Calculates the space between each interval (for both x and y axis)
    void CalcIntervalSpace()
    {
        xIntervalSpace = Math.Round(xAxisLength / xNumIntervals, 2);
        yIntervalSpace = Math.Round(yAxisLength / yNumIntervals, 2);
    }

    // Calculates the number of intervals that can fit on each axis
    void CalcNumIntervals()
    {
        xNumIntervals = (int)(xAxisLength / xIntervalSpace);
        yNumIntervals = (int)(yAxisLength / yIntervalSpace);
    }

    // Renders the x axis scales
    void SetupXScales()
    {
        double[] xAxisCoordinates = CalcAxisCoordinates(xNumIntervals, xIntervalSize);
        Vector3 origin = gameObject.transform.position;
        for (int i = 0; i < xAxisCoordinates.Length; i++)
        {
            GameObject scale = Instantiate(xScale);
            scale.name = "xScale";
            Vector3 scalePos = new Vector3((i + 1) * (float)xIntervalSpace + origin.x, origin.y, origin.z);
            scale.transform.position = scalePos;
            scale.transform.parent = xAxis.transform;
            TextMeshPro number = scale.transform.Find("number").gameObject.GetComponent<TextMeshPro>();
            number.text = xAxisCoordinates[i].ToString();
        }
    }

    // Renders the y axis scales
    void SetupYScales()
    {
        double[] yAxisCoordinates = CalcAxisCoordinates(yNumIntervals, yIntervalSize);
        Vector3 origin = gameObject.transform.position;
        for (int i = 0; i < yAxisCoordinates.Length; i++)
        {
            GameObject scale = Instantiate(yScale);
            scale.name = "yScale";
            Vector3 scalePos = new Vector3(origin.x, (i + 1) * (float)yIntervalSpace + origin.y, origin.z);
            scale.transform.position = scalePos;
            scale.transform.parent = yAxis.transform;
            TextMeshPro number = scale.transform.Find("number").gameObject.GetComponent<TextMeshPro>();
            number.text = yAxisCoordinates[i].ToString();
        }
    }

    // Calculates the numbers at each interval of the specified axis 
    double[] CalcAxisCoordinates(int numIntervals, double intervalSize)
    {
        double[] axisCoordinates = new double[numIntervals];
        for (int i = 0; i < axisCoordinates.Length; i++)
        {
            axisCoordinates[i] = Math.Round((i + 1) * intervalSize, 2);
        }

        return axisCoordinates;
    }

    // Clears the intervals from the specified axis
    void ResetScales(GameObject axis)
    {
        foreach (Transform scale in axis.transform)
        {
            GameObject.Destroy(scale.gameObject);
        }
    }

    // Setter for interval increment (x axis)
    public void SetXIntervalSize(double size)
    {
        if (size > 0 && xIntervalSize != size)
        {
            ResetScales(xAxis);
            xIntervalSize = Math.Round(size, 2);
            SetupXScales();
            meshManagerScript.UpdateGraphs();
            SendGraphDetails();
        }
    }

    // Setter for interval increment (y axis)
    public void SetYIntervalSize(double size)
    {
        if (size > 0 && yIntervalSize != size)
        {
            ResetScales(yAxis);
            yIntervalSize = Math.Round(size, 2);
            SetupYScales();
            meshManagerScript.UpdateGraphs();
            SendGraphDetails();
        }
    }

    // Setter for number of intervals (x axis)
    public void SetXNumIntervals(int numIntervals)
    {
        if (numIntervals > 0 && xNumIntervals != numIntervals)
        {
            ResetScales(xAxis);
            xNumIntervals = numIntervals;
            CalcIntervalSpace();
            SetupXScales();
            meshManagerScript.UpdateGraphs();
            SendGraphDetails();
        }
    }

    // Setter for number of intervals (y axis)
    public void SetYNumIntervals(int numIntervals)
    {
        if (numIntervals > 0 && yNumIntervals != numIntervals)
        {
            ResetScales(yAxis);
            yNumIntervals = numIntervals;
            CalcIntervalSpace();
            SetupYScales();
            meshManagerScript.UpdateGraphs();
            SendGraphDetails();
        }
    }

    // Setter for amount of space between each interval (x axis)
    public void SetXIntervalSpace(double intervalSpace)
    {
        if (intervalSpace > 0 && xIntervalSpace != intervalSpace)
        {
            ResetScales(xAxis);
            xIntervalSpace = Math.Round(intervalSpace, 2);
            CalcNumIntervals();
            SetupXScales();
            meshManagerScript.UpdateGraphs();
            SendGraphDetails();
        }
    }

    // Setter for amount of space between each interval (y axis)
    public void SetYIntervalSpace(double intervalSpace)
    {
        if (intervalSpace > 0 && yIntervalSpace != intervalSpace)
        {
            ResetScales(yAxis);
            yIntervalSpace = Math.Round(intervalSpace, 2);
            CalcNumIntervals();
            SetupYScales();
            meshManagerScript.UpdateGraphs();
            SendGraphDetails();
        }
    }

    // Setter for length of x axis
    public void SetXAxisLength(double axisLength)
    {
        if (axisLength > 0 && xAxisLength != axisLength)
        {
            xAxisLength = Math.Round(axisLength, 2);
            SetupXAxis();
            ResetScales(xAxis);
            CalcNumIntervals();
            SetupXScales();
            meshManagerScript.UpdateGraphs();
            SendGraphDetails();
        }
    }

    // Modifies the x axis position and scale based on axis length
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

    // Setter for length of y axis
    public void SetYAxisLength(double axisLength)
    {
        if (axisLength > 0 && yAxisLength != axisLength)
        {
            yAxisLength = Math.Round(axisLength, 2);
            SetupYAxis();
            ResetScales(yAxis);
            CalcNumIntervals();
            SetupYScales();
            meshManagerScript.UpdateGraphs();
            SendGraphDetails();
        }
    }

    // Modifies the y axis position and scale based on axis length
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

    // Getter for interval increment (x axis)
    public double GetXIntervalSize()
    {
        return xIntervalSize;
    }

    // Getter for interval increment (y axis)
    public double GetYIntervalSize()
    {
        return yIntervalSize;
    }

    // Getter for number of intervals (x axis)
    public int GetXNumIntervals()
    {
        return xNumIntervals;
    }

    // Getter for number of intervals (y axis)
    public int GetYNumIntervals()
    {
        return yNumIntervals;
    }

    // Getter for amount of space between intervals (x axis)
    public double GetXIntervalSpace()
    {
        return xIntervalSpace;
    }

    // Getter for amount of space between intervals (y axis)
    public double GetYIntervalSpace()
    {
        return yIntervalSpace;
    }

    // Getter for amount of space between a coordinate difference of 1 (x axis)
    public double GetXUnitSpace()
    {
        double xUnitSize = 1 / xIntervalSize;
        double xUnitSpace = xUnitSize * xIntervalSpace;
        return xUnitSpace;
    }

    // Getter for amount of space between a coordinate difference of 1 (y axis)
    public double GetYUnitSpace()
    {
        double yUnitSize = 1 / yIntervalSize;
        double yUnitSpace = yUnitSize * yIntervalSpace;
        return yUnitSpace;
    }

    // Getter for axis length (x axis)
    public double GetXAxisLength()
    {
        return xAxisLength;
    }
    
    // Getter for axis length (x axis)
    public double GetYAxisLength()
    {
        return yAxisLength;
    }

    // Sends graph parameter details to other players in game
    public void SendGraphDetails()
    {
        float[] graphDetails = new float[10];
        graphDetails[0] = 0;
        graphDetails[1] = (float)xIntervalSize;
        graphDetails[2] = (float)yIntervalSize;
        graphDetails[3] = (float)xNumIntervals;
        graphDetails[4] = (float)yNumIntervals;
        graphDetails[5] = (float)xIntervalSpace;
        graphDetails[6] = (float)yIntervalSpace;
        graphDetails[7] = (float)xAxisLength;
        graphDetails[8] = (float)yAxisLength;
        graphDetails[9] = Camera.main.GetInstanceID();

        aslObj.SendAndSetClaim(() =>
        {
            aslObj.SendFloatArray(graphDetails);
        });
    }

    /*  Determine what we do with the array based on value[0]
        *  0: Use the float array to set graph parameters 
        */
    public void onFloatArrayReceived(string _id, float[] value)
    {
        switch (value[0])
        {
            case 0f:
                if (Camera.main.GetInstanceID() == value[9])
                {
                    break;
                }
                xIntervalSize = Math.Round((double)value[1], 2);
                yIntervalSize = Math.Round((double)value[2], 2);
                xNumIntervals = (int)value[3];
                yNumIntervals = (int)value[4];
                xIntervalSpace = Math.Round((double)value[5], 2);
                yIntervalSpace = Math.Round((double)value[6], 2);
                xAxisLength = Math.Round((double)value[7], 2);
                yAxisLength = Math.Round((double)value[8], 2);

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
