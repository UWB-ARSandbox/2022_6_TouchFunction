using UnityEngine.InputSystem;
using UnityEngine;
using System;
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

    private PlayerInput playerInput;
    private InputAction rightClick;
    bool rightClicked;
    Vector3 originalMousePos;
    bool xSelected = false;
    bool ySelected = false;
    bool xTipSelected = false;
    bool yTipSelected = false;

    enum SelectedType
    {
        XAxisScale,
        YAxisScale,
        XAxisLine,
        YAxisLine,
        XAxisTip,
        YAxisTip,
        None
    }
    SelectedType selectedType;
    GameObject selected;
    Color selectedColor;
    Color defaultColor;

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

        rightClicked = false;
        selectedType = SelectedType.None;
        selected = null;
        selectedColor = new Color(1, 1, 0, 1);
        defaultColor = new Color(1, 1, 1, 1);
    }

    void Awake()
    {
        playerInput = new PlayerInput();
    }

    private void OnEnable()
    {
        rightClick = playerInput.PlayerControls.GraphManipulation;
        rightClick.started += StartRightClick;
        rightClick.canceled += EndRightClick;
        rightClick.Enable();
    }

    void Update()
    {        
        Debug.Log("This camera id: " + Camera.main.GetInstanceID());
        if (selectedType == SelectedType.XAxisScale)
        {
            Debug.Log("Right Click");
            Vector3 currPos = Input.mousePosition;
            Debug.Log("Curr Pos: " + currPos);
            float diff = currPos.x - originalMousePos.x;
            float delta = diff / 100.0f;
            Debug.Log("Diff: " + diff.ToString());
            SetXIntervalSpace(xIntervalSpace + Math.Round((double)delta, 1));
            originalMousePos = currPos;

            for (int i = 0; i < xAxis.transform.childCount; i++)
            {
                GameObject child = xAxis.transform.GetChild(i).gameObject;
                if (child.name.Equals("xScale"))
                {
                    GameObject scale = child.transform.Find("scale").gameObject;
                    scale.GetComponent<Renderer>().material.color = selectedColor;
                }
            }
        }

        else if (selectedType == SelectedType.YAxisScale)
        {
            Debug.Log("Right Click");
            Vector3 currPos = Input.mousePosition;
            Debug.Log("Curr Pos: " + currPos);
            float diff = currPos.y - originalMousePos.y;
            float delta = diff / 100.0f;
            Debug.Log("Diff: " + diff.ToString());
            SetYIntervalSpace(yIntervalSpace + Math.Round((double)delta, 1));
            originalMousePos = currPos;

            for (int i = 0; i < yAxis.transform.childCount; i++)
            {
                GameObject child = yAxis.transform.GetChild(i).gameObject;
                if (child.name.Equals("yScale"))
                {
                    GameObject scale = child.transform.Find("scale").gameObject;
                    scale.GetComponent<Renderer>().material.color = selectedColor;
                }
            }
        }

        else if (selectedType == SelectedType.XAxisTip)
        {
            Debug.Log("Right Click");
            Vector3 currPos = Input.mousePosition;
            Debug.Log("Curr Pos: " + currPos);
            float diff = currPos.x - originalMousePos.x;
            float delta = diff / 10.0f;
            Debug.Log("Diff: " + diff.ToString());
            SetXAxisLength(xAxisLength + Math.Round((double)delta, 1));
            originalMousePos = currPos;
        }

        else if (selectedType == SelectedType.YAxisTip)
        {
            Debug.Log("Right Click");
            Vector3 currPos = Input.mousePosition;
            Debug.Log("Curr Pos: " + currPos);
            float diff = currPos.y - originalMousePos.y;
            float delta = diff / 10.0f;
            Debug.Log("Diff: " + diff.ToString());
            SetYAxisLength(yAxisLength + Math.Round((double)delta, 1));
            originalMousePos = currPos;
        }
        else if (selectedType == SelectedType.XAxisLine)
        {
            Debug.Log("Right Click");
            Vector3 currPos = Input.mousePosition;
            Debug.Log("Curr Pos: " + currPos);
            float diff = currPos.x - originalMousePos.x;
            float delta = diff / 50.0f;
            Debug.Log("Diff: " + diff.ToString());
            SetXIntervalSize(xIntervalSize - Math.Round((double)delta, 1));
            originalMousePos = currPos;
        }
        else if (selectedType == SelectedType.YAxisLine)
        {
            Debug.Log("Right Click");
            Vector3 currPos = Input.mousePosition;
            Debug.Log("Curr Pos: " + currPos);
            float diff = currPos.y - originalMousePos.y;
            float delta = diff / 50.0f;
            Debug.Log("Diff: " + diff.ToString());
            SetYIntervalSize(yIntervalSize - Math.Round((double)delta, 1));
            originalMousePos = currPos;
        }
    }

    private void StartRightClick(InputAction.CallbackContext obj)
    {
        originalMousePos = Input.mousePosition;
        rightClicked = true;
        Debug.Log("Original Pos: " + originalMousePos);

        RaycastHit hitInfo = new RaycastHit();
        bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
        if (hit)
        {
            Debug.Log("HIT");
            selected = hitInfo.transform.gameObject;
            if (selected.name.Equals("xScale"))
            {
                Debug.Log("Selected x scale");
                selectedType = SelectedType.XAxisScale;

                for (int i = 0; i < xAxis.transform.childCount; i++)
                {
                    GameObject child = xAxis.transform.GetChild(i).gameObject;
                    if (child.name.Equals("xScale"))
                    {
                        GameObject scale = child.transform.Find("scale").gameObject;
                        scale.GetComponent<Renderer>().material.color = selectedColor;
                    }
                }

            }
            else if (selected.name.Equals("yScale"))
            {
                Debug.Log("Selected y scale");
                selectedType = SelectedType.YAxisScale;
                selected.GetComponent<Renderer>().material.color = selectedColor;

                for (int i = 0; i < yAxis.transform.childCount; i++)
                {
                    GameObject child = yAxis.transform.GetChild(i).gameObject;
                    if (child.name.Equals("yScale"))
                    {
                        GameObject scale = child.transform.Find("scale").gameObject;
                        scale.GetComponent<Renderer>().material.color = selectedColor;
                    }
                }
            }
            else if (selected.name.Equals("xAxisLine"))
            {
                Debug.Log("Selected x axis");
                selectedType = SelectedType.XAxisLine;
                selected.GetComponent<Renderer>().material.color = selectedColor;
            }
            else if (selected.name.Equals("yAxisLine"))
            {
                Debug.Log("Selected y axis");
                selectedType = SelectedType.YAxisLine;
                selected.GetComponent<Renderer>().material.color = selectedColor;
            }
            else if (selected.name.Equals("xTip"))
            {
                Debug.Log("Selected x tip");
                selectedType = SelectedType.XAxisTip;
                selected.GetComponent<Renderer>().material.color = selectedColor;
            }
            else if (selected.name.Equals("yTip"))
            {
                Debug.Log("Selected y tip");
                selectedType = SelectedType.YAxisTip;
                selected.GetComponent<Renderer>().material.color = selectedColor;
            }
            else
            {
                Debug.Log("No selection");
                selectedType = SelectedType.None;
                selected = null;
            }
        }
    }

    private void EndRightClick(InputAction.CallbackContext obj)
    {
        if (selectedType == SelectedType.XAxisScale)
        {
            for (int i = 0; i < xAxis.transform.childCount; i++)
            {
                GameObject child = xAxis.transform.GetChild(i).gameObject;
                if (child.name.Equals("xScale"))
                {
                    Debug.Log("Clearing x color");
                    GameObject scale = child.transform.Find("scale").gameObject;
                    scale.GetComponent<Renderer>().material.color = defaultColor;
                }
            }
        }
        else if (selectedType == SelectedType.YAxisScale)
        {
            for (int i = 0; i < yAxis.transform.childCount; i++)
            {
                Debug.Log("Clearing y color");
                GameObject child = yAxis.transform.GetChild(i).gameObject;
                if (child.name.Equals("yScale"))
                {
                    GameObject scale = child.transform.Find("scale").gameObject;
                    scale.GetComponent<Renderer>().material.color = defaultColor;
                }
            }
        }

        rightClicked = false;
        selectedType = SelectedType.None;
        selected.GetComponent<Renderer>().material.color = defaultColor;
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
            scale.name = "xScale";
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
            scale.name = "yScale";
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
        if (size > 0 && xIntervalSize != size)
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
        if (size > 0 && yIntervalSize != size)
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

    public void SetXIntervalSpace(double intervalSpace)
    {
        if (intervalSpace > 0 && xIntervalSpace != intervalSpace)
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
        if (intervalSpace > 0 && yIntervalSpace != intervalSpace)
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
        if (axisLength > 0 && xAxisLength != axisLength)
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
        if (axisLength > 0 && yAxisLength != axisLength)
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
