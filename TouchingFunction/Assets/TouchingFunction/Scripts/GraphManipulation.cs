using UnityEngine;
using System;
using TMPro;
using ASL;

public partial class GraphManipulation : MonoBehaviour
{
    ASLObject aslObj;

    public GameObject xAxisPos;
    public GameObject xAxisNeg;
    public GameObject yAxisPos;
    public GameObject yAxisNeg;

    public GameObject xAxisEndPos;
    public GameObject xAxisEndNeg;
    public GameObject yAxisEndPos;
    public GameObject yAxisEndNeg;

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

    double xAxisLengthPos;
    double xAxisLengthNeg;
    double yAxisLengthPos;
    double yAxisLengthNeg;

    // Start is called before the first frame update
    void Start()
    {
        xIntervalSize = 1.0;
        yIntervalSize = 1.0;
        xNumIntervals = 10;
        yNumIntervals = 10;

        xAxisLengthPos = (double)xAxisPos.transform.localScale.y;
        yAxisLengthPos = (double)yAxisPos.transform.localScale.y;
        xAxisLengthNeg = (double)xAxisNeg.transform.localScale.y;
        yAxisLengthNeg = (double)yAxisNeg.transform.localScale.y;

        Vector3 origin = transform.localPosition;
        transform.localPosition = new Vector3(origin.x, (float) (yAxisLengthNeg + 2), origin.z);

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
        double totalXAxisLength = xAxisLengthPos + xAxisLengthNeg;
        xIntervalSpace = Math.Round(totalXAxisLength / xNumIntervals, 2);

        double totalYAxisLength = yAxisLengthPos + yAxisLengthNeg;
        yIntervalSpace = Math.Round(totalYAxisLength  / yNumIntervals, 2);
    }

    // Calculates the number of intervals that can fit on each axis
    void CalcNumIntervals()
    {
        double totalXAxisLength = xAxisLengthPos + xAxisLengthNeg;
        xNumIntervals = (int)(totalXAxisLength / xIntervalSpace);

        double totalYAxisLength = yAxisLengthPos + yAxisLengthNeg;
        yNumIntervals = (int)(totalYAxisLength / yIntervalSpace);
    }

    // Renders the x axis scales
    void SetupXScales()
    {
        Vector3 origin = gameObject.transform.position;
        
        // Setting up positive scales
        int xNumIntervalsPos = (int) (xAxisLengthPos / xIntervalSpace);
        double[] xAxisCoordinates = CalcAxisCoordinates(xNumIntervalsPos, xIntervalSize);
        for (int i = 0; i < xAxisCoordinates.Length; i++)
        {
            GameObject scale = Instantiate(xScale);
            scale.name = "xScale";
            Vector3 scalePos = new Vector3((i + 1) * (float)xIntervalSpace + origin.x, origin.y, origin.z);
            scale.transform.position = scalePos;
            scale.transform.parent = transform;
            TextMeshPro number = scale.transform.Find("number").gameObject.GetComponent<TextMeshPro>();
            number.text = xAxisCoordinates[i].ToString();
        }

        // Setting up negative scales
        int xNumIntervalsNeg = (int) (xAxisLengthNeg / xIntervalSpace);
        xAxisCoordinates = CalcAxisCoordinates(xNumIntervalsNeg, xIntervalSize);
        for (int i = 0; i < xAxisCoordinates.Length; i++)
        {
            GameObject scale = Instantiate(xScale);
            scale.name = "xScale";
            Vector3 scalePos = new Vector3(origin.x - (i + 1) * (float)xIntervalSpace, origin.y, origin.z);
            scale.transform.position = scalePos;
            scale.transform.parent = transform;
            TextMeshPro number = scale.transform.Find("number").gameObject.GetComponent<TextMeshPro>();
            number.text = (-xAxisCoordinates[i]).ToString();
        }
    }

    // Renders the y axis scales
    void SetupYScales()
    {
        Vector3 origin = gameObject.transform.position;
        
        // Setting up positive scales
        int yNumIntervalsPos = (int) (yAxisLengthPos / yIntervalSpace);
        double[] yAxisCoordinates = CalcAxisCoordinates(yNumIntervalsPos, yIntervalSize);
        for (int i = 0; i < yAxisCoordinates.Length; i++)
        {
            GameObject scale = Instantiate(yScale);
            scale.name = "yScale";
            Vector3 scalePos = new Vector3(origin.x, (i + 1) * (float)yIntervalSpace + origin.y, origin.z);
            scale.transform.position = scalePos;
            scale.transform.parent = transform;
            TextMeshPro number = scale.transform.Find("number").gameObject.GetComponent<TextMeshPro>();
            number.text = yAxisCoordinates[i].ToString();
        }

        int yNumIntervalsNeg = (int) (yAxisLengthNeg / yIntervalSpace);
        yAxisCoordinates = CalcAxisCoordinates(yNumIntervalsNeg, yIntervalSize);
        for (int i = 0; i < yAxisCoordinates.Length; i++)
        {
            GameObject scale = Instantiate(yScale);
            scale.name = "yScale";
            Vector3 scalePos = new Vector3(origin.x, origin.y - (i + 1) * (float)yIntervalSpace, origin.z);
            scale.transform.position = scalePos;
            scale.transform.parent = transform;
            TextMeshPro number = scale.transform.Find("number").gameObject.GetComponent<TextMeshPro>();
            number.text = (-yAxisCoordinates[i]).ToString();
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

    // Clears the intervals from the x axis
    void ResetXScales()
    {
        foreach (Transform child in transform)
        {
            if (child.name.Equals("xScale"))
            {
                GameObject.Destroy(child.gameObject);
            }
        }
    }

    // Clears the intervals from the y axis
    void ResetYScales()
    {
        foreach (Transform child in transform)
        {
            if (child.name.Equals("yScale"))
            {
                GameObject.Destroy(child.gameObject);
            }
        }
    }

    // Setter for interval increment (x axis)
    public void SetXIntervalSize(double size)
    {
        if (size > 0 && xIntervalSize != size)
        {
            ResetXScales();
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
            ResetYScales();
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
            ResetXScales();
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
            ResetYScales();
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
            ResetXScales();
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
            ResetYScales();
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
        if (axisLength > 0 && xAxisLengthPos != axisLength)
        {
            xAxisLengthPos = Math.Round(axisLength, 2);
            SetupXAxis();
            ResetXScales();
            CalcNumIntervals();
            SetupXScales();
            meshManagerScript.UpdateGraphs();
            SendGraphDetails();
        }
    }

    // Modifies the x axis position and scale based on axis length
    public void SetupXAxis()
    {
        Vector3 axisScale = xAxisPos.transform.localScale;
        xAxisPos.transform.localScale = new Vector3(axisScale.x, (float)xAxisLengthPos, axisScale.z);

        Vector3 axisPos = xAxisPos.transform.localPosition;
        double xPos = -(xAxisLengthPos / 2);
        xAxisPos.transform.localPosition = new Vector3((float)xPos, axisPos.y, axisPos.z);

        Vector3 endPos = xAxisEndPos.transform.localPosition;
        double endPosX = -(xAxisLengthPos + 0.8);
        xAxisEndPos.transform.localPosition = new Vector3((float)endPosX, 0, 0);
    }

    // Setter for length of x axis
    public void SetXAxisLengthNeg(double axisLength)
    {
        if (axisLength > 0 && xAxisLengthNeg != axisLength)
        {
            xAxisLengthNeg = Math.Round(axisLength, 2);
            SetupXAxisNeg();
            ResetXScales();
            CalcNumIntervals();
            SetupXScales();
            meshManagerScript.UpdateGraphs();
            SendGraphDetails();
        }
    }

    // Modifies the x axis position and scale based on axis length
    public void SetupXAxisNeg()
    {
        Vector3 axisScale = xAxisNeg.transform.localScale;
        xAxisNeg.transform.localScale = new Vector3(axisScale.x, (float)xAxisLengthNeg, axisScale.z);

        Vector3 axisPos = xAxisNeg.transform.localPosition;
        double xPos = xAxisLengthNeg / 2;
        xAxisNeg.transform.localPosition = new Vector3((float)xPos, axisPos.y, axisPos.z);

        Vector3 endPos = xAxisEndNeg.transform.localPosition;
        double endPosX = xAxisLengthNeg + 0.8;
        xAxisEndNeg.transform.localPosition = new Vector3((float)endPosX, 0, 0);
    }


    // Setter for length of y axis
    public void SetYAxisLength(double axisLength)
    {
        if (axisLength > 0 && yAxisLengthPos != axisLength)
        {
            yAxisLengthPos = Math.Round(axisLength, 2);
            SetupYAxis();
            ResetYScales();
            CalcNumIntervals();
            SetupYScales();
            meshManagerScript.UpdateGraphs();
            SendGraphDetails();
        }
    }

    // Modifies the y axis position and scale based on axis length
    public void SetupYAxis()
    {
        Vector3 axisScale = yAxisPos.transform.localScale;
        yAxisPos.transform.localScale = new Vector3(axisScale.x, (float)yAxisLengthPos, axisScale.z);

        Vector3 axisPos = yAxisPos.transform.localPosition;
        double yPos = yAxisLengthPos / 2;
        yAxisPos.transform.localPosition = new Vector3(axisPos.x, (float)yPos, axisPos.z);

        Vector3 endPos = yAxisEndPos.transform.position;
        double endPosY = yAxisLengthPos + 0.8;
        yAxisEndPos.transform.localPosition = new Vector3(0, (float)endPosY, 0);
    }

    // Setter for length of x axis
    public void SetYAxisLengthNeg(double axisLength)
    {
        if (axisLength > 0 && yAxisLengthNeg != axisLength)
        {
            yAxisLengthNeg = Math.Round(axisLength, 2);
            SetupYAxisNeg();
            ResetYScales();
            CalcNumIntervals();
            SetupYScales();
            meshManagerScript.UpdateGraphs();
            Vector3 origin = transform.localPosition;
            transform.localPosition = new Vector3(origin.x, (float) (yAxisLengthNeg + 2), origin.z);
            SendGraphDetails();
        }
    }

    // Modifies the x axis position and scale based on axis length
    public void SetupYAxisNeg()
    {
        Vector3 axisScale = yAxisNeg.transform.localScale;
        yAxisNeg.transform.localScale = new Vector3(axisScale.x, (float)yAxisLengthNeg, axisScale.z);

        Vector3 axisPos = yAxisNeg.transform.localPosition;
        double yPos = -(yAxisLengthNeg / 2);
        yAxisNeg.transform.localPosition = new Vector3(axisPos.x, (float)yPos, axisPos.z);

        Vector3 endPos = yAxisEndNeg.transform.position;
        double endPosY = -(yAxisLengthNeg + 0.8);
        yAxisEndNeg.transform.localPosition = new Vector3(0, (float)endPosY, 0);
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
        return xAxisLengthPos;
    }
    
    // Getter for axis length (x axis)
    public double GetYAxisLength()
    {
        return yAxisLengthPos;
    }

    // Sends graph parameter details to other players in game
    public void SendGraphDetails()
    {
        float[] graphDetails = new float[12];
        graphDetails[0] = 0;
        graphDetails[1] = (float)xIntervalSize;
        graphDetails[2] = (float)yIntervalSize;
        graphDetails[3] = (float)xNumIntervals;
        graphDetails[4] = (float)yNumIntervals;
        graphDetails[5] = (float)xIntervalSpace;
        graphDetails[6] = (float)yIntervalSpace;
        graphDetails[7] = (float)xAxisLengthPos;
        graphDetails[8] = (float)yAxisLengthPos;
        graphDetails[9] = (float)xAxisLengthNeg;
        graphDetails[10] = (float)yAxisLengthNeg;
        graphDetails[11] = Camera.main.GetInstanceID();

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
                if (Camera.main.GetInstanceID() == value[11])
                {
                    break;
                }
                xIntervalSize = Math.Round((double)value[1], 2);
                yIntervalSize = Math.Round((double)value[2], 2);
                xNumIntervals = (int)value[3];
                yNumIntervals = (int)value[4];
                xIntervalSpace = Math.Round((double)value[5], 2);
                yIntervalSpace = Math.Round((double)value[6], 2);
                xAxisLengthPos = Math.Round((double)value[7], 2);
                yAxisLengthPos = Math.Round((double)value[8], 2);
                xAxisLengthNeg = Math.Round((double)value[9], 2);
                yAxisLengthNeg = Math.Round((double)value[10], 2);


                ResetXScales();
                ResetYScales();
                SetupXAxis();
                SetupYAxis();
                SetupXScales();
                SetupYScales();

                meshManagerScript.UpdateGraphs();

                break;

        }
    }
}
