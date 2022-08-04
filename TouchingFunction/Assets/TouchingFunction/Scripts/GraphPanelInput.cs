using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GraphPanelInput : MonoBehaviour
{
    public Button submitButton;
    public TMP_InputField xIntSizeInput;
    public TMP_InputField xIntSpaceInput;
    public TMP_InputField xAxisLenPosInput;
    public TMP_InputField xAxisLenNegInput;
    public TMP_InputField yIntSizeInput;
    public TMP_InputField yIntSpaceInput;
    public TMP_InputField yAxisLenPosInput;
    public TMP_InputField yAxisLenNegInput;
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
        xIntSizeInput.placeholder.GetComponent<TextMeshProUGUI>().text = xIntervalSize;

        // Get x axis interval spacing
        string xIntervalSpace = graphManScript.GetXIntervalSpace().ToString();
        xIntSpaceInput.placeholder.GetComponent<TextMeshProUGUI>().text = xIntervalSpace;

        // Get x axis length (positive)
        string xAxisLenPos = graphManScript.GetXAxisLengthPos().ToString();
        xAxisLenPosInput.placeholder.GetComponent<TextMeshProUGUI>().text = xAxisLenPos;

        // Get x axis length (negative)
        string xAxisLenNeg = graphManScript.GetXAxisLengthNeg().ToString();
        xAxisLenNegInput.placeholder.GetComponent<TextMeshProUGUI>().text = xAxisLenNeg;

        // Get y axis interval size
        string yIntervalSize = graphManScript.GetYIntervalSize().ToString();
        yIntSizeInput.placeholder.GetComponent<TextMeshProUGUI>().text = yIntervalSize;

        // Get y axis interval spacing
        string yIntervalSpace = graphManScript.GetYIntervalSpace().ToString();
        yIntSpaceInput.placeholder.GetComponent<TextMeshProUGUI>().text = yIntervalSpace;

        // Get y axis length (positve)
        string yAxisLenPos = graphManScript.GetYAxisLengthPos().ToString();
        yAxisLenPosInput.placeholder.GetComponent<TextMeshProUGUI>().text = yAxisLenPos;

        // Get y axis length (negative)
        string yAxisLenNeg = graphManScript.GetYAxisLengthNeg().ToString();
        yAxisLenNegInput.placeholder.GetComponent<TextMeshProUGUI>().text = yAxisLenNeg;
    }

    private void GetInputOnClickHandler() 
    {
        double numDouble;
        bool isDouble;

        isDouble = double.TryParse(xIntSizeInput.text, out numDouble);
        if (isDouble) {
            graphManScript.SetXIntervalSize(numDouble);
        }
        xIntSizeInput.text = "";

        isDouble = double.TryParse(xIntSpaceInput.text, out numDouble);
        if (isDouble) {
            graphManScript.SetXIntervalSpace(numDouble);
        }
        xIntSpaceInput.text = "";

        isDouble = double.TryParse(xAxisLenPosInput.text, out numDouble);
        if (isDouble) {
            graphManScript.SetXAxisLengthPos(numDouble);
        }
        xAxisLenPosInput.text = "";

        isDouble = double.TryParse(xAxisLenNegInput.text, out numDouble);
        if (isDouble) {
            graphManScript.SetXAxisLengthNeg(numDouble);
        }
        xAxisLenNegInput.text = "";

        isDouble = double.TryParse(yIntSizeInput.text, out numDouble);
        if (isDouble) {
            graphManScript.SetYIntervalSize(numDouble);
        }
        yIntSizeInput.text = "";

        isDouble = double.TryParse(yIntSpaceInput.text, out numDouble);
        if (isDouble) {
            graphManScript.SetYIntervalSpace(numDouble);
        }
        yIntSpaceInput.text = "";

        isDouble = double.TryParse(yAxisLenPosInput.text, out numDouble);
        if (isDouble) {
            graphManScript.SetYAxisLengthPos(numDouble);
        }
        yAxisLenPosInput.text = "";

        isDouble = double.TryParse(yAxisLenNegInput.text, out numDouble);
        if (isDouble) {
            graphManScript.SetYAxisLengthNeg(numDouble);
        }
        yAxisLenNegInput.text = "";
    }
}