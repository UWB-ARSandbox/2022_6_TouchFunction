using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GraphPanelInput : MonoBehaviour
{
    public Button submitButton;
    public TMP_InputField xIntSizeInput;
    public TMP_InputField xNumIntInput;
    public TMP_InputField xIntSpaceInput;
    public TMP_InputField xAxisLenInput;
    public TMP_InputField yIntSizeInput;
    public TMP_InputField yNumIntInput;
    public TMP_InputField yIntSpaceInput;
    public TMP_InputField yAxisLenInput;
    public GameObject graphAxes;

    GraphManipulation graphManScript;
    
    // Start is called before the first frame update
    void Start()
    {
        graphManScript = graphAxes.GetComponent<GraphManipulation>();
        submitButton.onClick.AddListener(GetInputOnClickHandler);
    }

    // Update is called once per frame
    void Update()
    {
        // Get x axis interval size
        string xIntervalSize = graphManScript.GetXIntervalSize().ToString(); 
        xIntSizeInput.placeholder.GetComponent<Text>().text = xIntervalSize;

        // Get x axis number of intervals
        string xNumIntervals = graphManScript.GetXNumIntervals().ToString();
        xNumIntInput.placeholder.GetComponent<Text>().text = xNumIntervals;

        // Get x axis interval spacing
        string xIntervalSpace = graphManScript.GetXIntervalSpace().ToString();
        xIntSpaceInput.placeholder.GetComponent<Text>().text = xIntervalSpace;

        // Get x axis length
        string xAxisLen = graphManScript.GetXAxisLength().ToString();
        xAxisLenInput.placeholder.GetComponent<Text>().text = xAxisLen;

        // Get y axis interval size
        string yIntervalSize = graphManScript.GetYIntervalSize().ToString();
        yIntSizeInput.placeholder.GetComponent<Text>().text = yIntervalSize;

        // Get y axis number of intervals
        string yNumIntervals = graphManScript.GetYNumIntervals().ToString();
        yNumIntInput.placeholder.GetComponent<Text>().text = yNumIntervals;

        // Get y axis interval spacing
        string yIntervalSpace = graphManScript.GetYIntervalSpace().ToString();
        yIntSpaceInput.placeholder.GetComponent<Text>().text = yIntervalSpace;

        // Get y axis length
        string yAxisLen = graphManScript.GetYAxisLength().ToString();
        yAxisLenInput.placeholder.GetComponent<Text>().text = yAxisLen;
    }

    private void GetInputOnClickHandler() 
    {
        double numDouble;
        bool isDouble;

        int numInt;
        bool isInt;

        isDouble = double.TryParse(xIntSizeInput.text, out numDouble);
        if (isDouble) {
            graphManScript.SetXIntervalSize(numDouble);
        }
        xIntSizeInput.text = "";

        isInt = int.TryParse(xNumIntInput.text, out numInt);
        if (isInt) {
            graphManScript.SetXNumIntervals(numInt);
        }
        xNumIntInput.text = "";

        isDouble = double.TryParse(xIntSpaceInput.text, out numDouble);
        if (isDouble) {
            graphManScript.SetXIntervalSpace(numDouble);
        }
        xIntSpaceInput.text = "";

        isDouble = double.TryParse(xAxisLenInput.text, out numDouble);
        if (isDouble) {
            graphManScript.SetXAxisLength(numDouble);
        }
        xAxisLenInput.text = "";

        isDouble = double.TryParse(yIntSizeInput.text, out numDouble);
        if (isDouble) {
            graphManScript.SetYIntervalSize(numDouble);
        }
        yIntSizeInput.text = "";

        isInt = int.TryParse(yNumIntInput.text, out numInt);
        if (isInt) {
            graphManScript.SetYNumIntervals(numInt);
        }
        yNumIntInput.text = "";

        isDouble = double.TryParse(yIntSpaceInput.text, out numDouble);
        if (isDouble) {
            graphManScript.SetYIntervalSpace(numDouble);
        }
        yIntSpaceInput.text = "";

        isDouble = double.TryParse(yAxisLenInput.text, out numDouble);
        if (isDouble) {
            graphManScript.SetYAxisLength(numDouble);
        }
        yAxisLenInput.text = "";
    }
}