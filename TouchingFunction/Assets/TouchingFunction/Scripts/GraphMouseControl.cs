using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public partial class GraphManipulation : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction manipulateGraph;
    bool enabledGraphManip;
    Vector3 originalMousePos;

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

    void Awake()
    {
        playerInput = new PlayerInput();
    }

    private void OnEnable()
    {
        manipulateGraph = playerInput.PlayerControls.GraphManipulation;
        manipulateGraph.started += StartGraphManipulation;
        manipulateGraph.canceled += EndGraphManipulation;
        manipulateGraph.Enable();
    }

    void Update()
    {
        if (enabledGraphManip && selectedType == SelectedType.XAxisScale)
        {
            Vector3 currPos = Input.mousePosition;
            float diff = currPos.x - originalMousePos.x;
            float delta = diff / 500.0f;
            SetXIntervalSpace(xIntervalSpace + Math.Round((double)delta, 1));
            originalMousePos = currPos;
            SetSelectedColor(selectedColor);    // Scales keep changing so need to set color them every update
        }

        else if (enabledGraphManip && selectedType == SelectedType.YAxisScale)
        {
            Vector3 currPos = Input.mousePosition;
            float diff = currPos.y - originalMousePos.y;
            float delta = diff / 200.0f;
            SetYIntervalSpace(yIntervalSpace + Math.Round((double)delta, 1));
            originalMousePos = currPos;
            SetSelectedColor(selectedColor);    // Scales keep changing so need to set color them every update
        }

        else if (enabledGraphManip && selectedType == SelectedType.XAxisTip)
        {
            Vector3 currPos = Input.mousePosition;
            float diff = currPos.x - originalMousePos.x;
            float delta = diff / 50.0f;
            SetXAxisLength(xAxisLength + Math.Round((double)delta, 1));
            originalMousePos = currPos;
        }

        else if (enabledGraphManip && selectedType == SelectedType.YAxisTip)
        {
            Vector3 currPos = Input.mousePosition;
            float diff = currPos.y - originalMousePos.y;
            float delta = diff / 50.0f;
            SetYAxisLength(yAxisLength + Math.Round((double)delta, 1));
            originalMousePos = currPos;
        }
        else if (enabledGraphManip && selectedType == SelectedType.XAxisLine)
        {
            Vector3 currPos = Input.mousePosition;
            float diff = currPos.x - originalMousePos.x;
            float delta = diff / 200.0f;
            SetXIntervalSize(xIntervalSize - Math.Round((double)delta, 1));
            originalMousePos = currPos;
        }
        else if (enabledGraphManip && selectedType == SelectedType.YAxisLine)
        {
            Vector3 currPos = Input.mousePosition;
            float diff = currPos.y - originalMousePos.y;
            float delta = diff / 200.0f;
            SetYIntervalSize(yIntervalSize - Math.Round((double)delta, 1));
            originalMousePos = currPos;
        }
        else
        {
            SelectObject();
        }
    }

    // Select an object on graph axes based on cursor position
    private void SelectObject()
    {
        if (selected != null)
        {
            SetSelectedColor(defaultColor);
        }

        RaycastHit hitInfo = new RaycastHit();
        bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
        if (hit)
        {
            selected = hitInfo.transform.gameObject;
            if (selected.name.Equals("xScale"))
            {
                selectedType = SelectedType.XAxisScale;
            }
            else if (selected.name.Equals("yScale"))
            {
                selectedType = SelectedType.YAxisScale;
            }
            else if (selected.name.Equals("xAxisLine"))
            {
                selectedType = SelectedType.XAxisLine;
            }
            else if (selected.name.Equals("yAxisLine"))
            {
                selectedType = SelectedType.YAxisLine;
            }
            else if (selected.name.Equals("xTip"))
            {
                selectedType = SelectedType.XAxisTip;
            }
            else if (selected.name.Equals("yTip"))
            {

                selectedType = SelectedType.YAxisTip;
            }
            else
            {
                selectedType = SelectedType.None;
                selected = null;
            }
        }

        if (selected != null)
        {
            SetSelectedColor(selectedColor);
        }
    }

    // When right click starts
    private void StartGraphManipulation(InputAction.CallbackContext obj)
    {
        originalMousePos = Input.mousePosition;
        enabledGraphManip = true;
    }

    // When right click ends
    private void EndGraphManipulation(InputAction.CallbackContext obj)
    {
        SetSelectedColor(defaultColor);
        enabledGraphManip = false;
        selectedType = SelectedType.None;
        selected = null;
    }

    // Set the color of the selected graph axes object based on specified color
    private void SetSelectedColor(Color color)
    {
        if (selected == null)
        {
            return;
        }

        if (selectedType == SelectedType.XAxisScale)
        {
            for (int i = 0; i < xAxis.transform.childCount; i++)
            {   // Set color for every x scale
                GameObject child = xAxis.transform.GetChild(i).gameObject;
                if (child.name.Equals("xScale"))
                {
                    GameObject scale = child.transform.Find("scale").gameObject;
                    scale.GetComponent<Renderer>().material.color = color;
                }
            }
        }
        else if (selectedType == SelectedType.YAxisScale)
        {
            for (int i = 0; i < yAxis.transform.childCount; i++)
            {
                // Set color for every y scale
                GameObject child = yAxis.transform.GetChild(i).gameObject;
                if (child.name.Equals("yScale"))
                {
                    GameObject scale = child.transform.Find("scale").gameObject;
                    scale.GetComponent<Renderer>().material.color = color;
                }
            }
        }
        else
        {
            selected.GetComponent<Renderer>().material.color = color;
        }
    }
}