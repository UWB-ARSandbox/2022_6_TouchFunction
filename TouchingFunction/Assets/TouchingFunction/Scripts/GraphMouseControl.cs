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
        XAxisTipPos,
        YAxisTipPos,
        XAxisTipNeg,
        YAxisTipNeg,
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
        Debug.Log("Origin (World): " + gameObject.transform.position);
        Debug.Log("Origin (Screen): " + Camera.main.WorldToScreenPoint(gameObject.transform.position));
        Debug.Log("Origin Mouse: " + Input.mousePosition);        
        if (enabledGraphManip && selectedType == SelectedType.XAxisScale)
        {
            Vector3 currPos = Input.mousePosition;
            Vector3 originPos = Camera.main.WorldToScreenPoint(gameObject.transform.position);
            float currDist = Math.Abs(currPos.x - originPos.x);
            float prevDist = Math.Abs(originalMousePos.x - originPos.x);
            float diff = currDist - prevDist;
            float delta = diff * Math.Abs(originPos.z) / 2000.0f; // the further the player is from the graph, the higher the sensitivity
            SetXIntervalSpace(xIntervalSpace + Math.Round((double)delta, 1));
            originalMousePos = currPos;
            SetSelectedColor(selectedColor);    // Scales keep changing so need to set color them every update
        }

        else if (enabledGraphManip && selectedType == SelectedType.YAxisScale)
        {
            Vector3 currPos = Input.mousePosition;
            Vector3 originPos = Camera.main.WorldToScreenPoint(gameObject.transform.position);
            float currDist = Math.Abs(currPos.y - originPos.y);
            float prevDist = Math.Abs(originalMousePos.y - originPos.y);
            float diff = currDist - prevDist;
            float delta = diff * Math.Abs(originPos.z) / 700.0f; // the further the player is from the graph, the higher the sensitivity
            SetYIntervalSpace(yIntervalSpace + Math.Round((double)delta, 1));
            originalMousePos = currPos;
            SetSelectedColor(selectedColor);    // Scales keep changing so need to set color them every update
        }

        else if (enabledGraphManip && selectedType == SelectedType.XAxisTipPos)
        {
            Vector3 currPos = Input.mousePosition;
            Vector3 originPos = Camera.main.WorldToScreenPoint(gameObject.transform.position);
            float currDist = Math.Abs(currPos.x - originPos.x);
            float prevDist = Math.Abs(originalMousePos.x - originPos.x);
            float diff = currDist - prevDist;
            float delta = diff * Math.Abs(originPos.z) / 1000.0f; // the further the player is from the graph, the higher the sensitivity
            SetXAxisLength(xAxisLengthPos + Math.Round((double)delta, 1));
            originalMousePos = currPos;
        }

        else if (enabledGraphManip && selectedType == SelectedType.YAxisTipPos)
        {
            Vector3 currPos = Input.mousePosition;
            Vector3 originPos = Camera.main.WorldToScreenPoint(gameObject.transform.position);
            float currDist = Math.Abs(currPos.y - originPos.y);
            float prevDist = Math.Abs(originalMousePos.y - originPos.y);
            float diff = currDist - prevDist;
            float delta = diff * Math.Abs(originPos.z) / 500.0f; // the further the player is from the graph, the higher the sensitivity
            SetYAxisLength(yAxisLengthPos + Math.Round((double)delta, 1));
            originalMousePos = currPos;
        }

        else if (enabledGraphManip && selectedType == SelectedType.XAxisTipNeg)
        {
            Vector3 currPos = Input.mousePosition;
            Vector3 originPos = Camera.main.WorldToScreenPoint(gameObject.transform.position);
            float currDist = Math.Abs(currPos.x - originPos.x);
            float prevDist = Math.Abs(originalMousePos.x - originPos.x);
            float diff = currDist - prevDist;
            float delta = diff * Math.Abs(originPos.z) / 1000.0f; // the further the player is from the graph, the higher the sensitivity
            SetXAxisLengthNeg(xAxisLengthNeg + Math.Round((double)delta, 1));
            originalMousePos = currPos;
        }
        
        else if (enabledGraphManip && selectedType == SelectedType.YAxisTipNeg)
        {
            Vector3 currPos = Input.mousePosition;
            Vector3 originPos = Camera.main.WorldToScreenPoint(gameObject.transform.position);
            float currDist = Math.Abs(currPos.y - originPos.y);
            float prevDist = Math.Abs(originalMousePos.y - originPos.y);
            float diff = currDist - prevDist;
            float delta = diff * Math.Abs(originPos.z) / 1000.0f; // the further the player is from the graph, the higher the sensitivity
            SetYAxisLengthNeg(yAxisLengthNeg + Math.Round((double)delta, 1));
            originalMousePos = currPos;
        }

        else if (enabledGraphManip && selectedType == SelectedType.XAxisLine)
        {
            Vector3 currPos = Input.mousePosition;
            Vector3 originPos = Camera.main.WorldToScreenPoint(gameObject.transform.position);
            float currDist = Math.Abs(currPos.x - originPos.x);
            float prevDist = Math.Abs(originalMousePos.x - originPos.x);
            float diff = currDist - prevDist;
            float delta = diff * Math.Abs(originPos.z) / 2000.0f; // the further the player is from the graph, the higher the sensitivity
            SetXIntervalSize(xIntervalSize - Math.Round((double)delta, 1));
            originalMousePos = currPos;
        }
        else if (enabledGraphManip && selectedType == SelectedType.YAxisLine)
        {
            Vector3 currPos = Input.mousePosition;
            Vector3 originPos = Camera.main.WorldToScreenPoint(gameObject.transform.position);
            float currDist = Math.Abs(currPos.y - originPos.y);
            float prevDist = Math.Abs(originalMousePos.y - originPos.y);
            float diff = currDist - prevDist;
            float delta = diff * Math.Abs(originPos.z) / 1000.0f; // the further the player is from the graph, the higher the sensitivity
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
            else if (selected.name.Equals("xAxisLinePos") || selected.name.Equals("xAxisLineNeg"))
            {
                selectedType = SelectedType.XAxisLine;
            }
            else if (selected.name.Equals("yAxisLinePos") || selected.name.Equals("yAxisLineNeg"))
            {
                selectedType = SelectedType.YAxisLine;
            }
            else if (selected.name.Equals("xTipPos"))
            {
                selectedType = SelectedType.XAxisTipPos;
            }
            else if (selected.name.Equals("yTipPos"))
            {
                selectedType = SelectedType.YAxisTipPos;
            }
            else if (selected.name.Equals("xTipNeg"))
            {
                selectedType = SelectedType.XAxisTipNeg;
            }
            else if (selected.name.Equals("yTipNeg"))
            {
                selectedType = SelectedType.YAxisTipNeg;
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
        if (selected != null)
        {
            SetSelectedColor(defaultColor);
        }
        enabledGraphManip = false;
        selectedType = SelectedType.None;
        selected = null;
    }

    // Set the color of the selected graph axes object based on specified color
    private void SetSelectedColor(Color color)
    {
        if (selectedType == SelectedType.XAxisScale)
        {
            for (int i = 0; i < transform.childCount; i++)
            {   
                // Set color for every x scale
                GameObject child = transform.GetChild(i).gameObject;
                if (child.name.Equals("xScale"))
                {
                    GameObject scale = child.transform.Find("scale").gameObject;
                    Debug.Assert(scale != null);
                    scale.GetComponent<Renderer>().material.color = color;
                }
            }
        }
        else if (selectedType == SelectedType.YAxisScale)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                // Set color for every y scale
                GameObject child = transform.GetChild(i).gameObject;
                if (child.name.Equals("yScale"))
                {
                    GameObject scale = child.transform.Find("scale").gameObject;
                    scale.GetComponent<Renderer>().material.color = color;
                }
            }
        }
        else if (selectedType == SelectedType.XAxisLine)
        {
            xAxisPos.GetComponent<Renderer>().material.color = color;
            xAxisNeg.GetComponent<Renderer>().material.color = color;
        }
        else if (selectedType == SelectedType.YAxisLine)
        {
            yAxisPos.GetComponent<Renderer>().material.color = color;
            yAxisNeg.GetComponent<Renderer>().material.color = color;
        }
        else
        {
            selected.GetComponent<Renderer>().material.color = color;
        }
    }
}