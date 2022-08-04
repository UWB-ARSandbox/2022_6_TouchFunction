using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.XR.Interaction.Toolkit;

public partial class GraphManipulation : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction manipulateGraph;
    private InputAction RightTrig;
    private InputAction LeftTrig;
    bool enabledGraphManip;
    Vector3 originalMousePos;

    bool RightTrigActive;
    bool LeftTrigActive;

    enum SelectedType
    {
        None,
        XAxisScale,
        YAxisScale,
        XAxisLine,
        YAxisLine,
        XAxisTipPos,
        YAxisTipPos,
        XAxisTipNeg,
        YAxisTipNeg
    }
    SelectedType selectedType;

    enum InputType
    {
        Mouse,
        VR
    }
    int inputType;

    GameObject selected;
    Color selectedColor;
    Color defaultColor;

    public GameObject RightCon;
    public GameObject LeftCon;

    private LayerMask mask;

    void Awake()
    {
        playerInput = new PlayerInput();
        mask = LayerMask.GetMask("Axes");

    }

    private void OnEnable()
    {
        manipulateGraph = playerInput.PlayerControls.GraphManipulation;
        manipulateGraph.started += StartGraphManipulation;
        manipulateGraph.canceled += EndGraphManipulation;
        manipulateGraph.Enable();

        RightTrig = playerInput.PlayerControls.ModRightTrigger;
        RightTrig.started += rightTriggerManipStart;
        RightTrig.canceled += rightTriggerManipEnd;
        RightTrig.Enable();

        LeftTrig = playerInput.PlayerControls.ModLeftTrigger;
        LeftTrig.started += leftTriggerManipStart;
        LeftTrig.canceled += leftTriggerManipEnd;
        LeftTrig.Enable();
    }

    void Update()
    {
        ClearGridlines();
        SetupGridlines();

        if(enabledGraphManip)      
        {
            Vector3 currPos;
            Vector3 originPos;
            

            if(RightTrigActive)
            {
                currPos = Camera.main.WorldToScreenPoint(RightCon.transform.position + RightCon.transform.forward * RightCon.GetComponent<XRInteractorLineVisual>().lineLength);
                originPos = Camera.main.WorldToScreenPoint(gameObject.transform.position);
                inputType = (int) InputType.VR;
            }
            else if(LeftTrigActive)
            {
                currPos = Camera.main.WorldToScreenPoint(LeftCon.transform.position + LeftCon.transform.forward * LeftCon.GetComponent<XRInteractorLineVisual>().lineLength);
                originPos = Camera.main.WorldToScreenPoint(gameObject.transform.position);
                inputType = (int) InputType.VR;
            }
            else
            {
                currPos = Input.mousePosition;
                originPos = Camera.main.WorldToScreenPoint(gameObject.transform.position);
                inputType = (int) InputType.Mouse;                
            }

            if (selectedType == SelectedType.XAxisScale)
            {
                float[] scalingFactor = {2000.0f, 2000.0f};
                
                float currDist = Math.Abs(currPos.x - originPos.x);
                float prevDist = Math.Abs(originalMousePos.x - originPos.x);
                float diff = currDist - prevDist;
                float delta = diff * Math.Abs(originPos.z) / scalingFactor[inputType]; // the further the player is from the graph, the higher the sensitivity
                SetXIntervalSpace(xIntervalSpace + Math.Round((double)delta, 1));
                originalMousePos = currPos;
                SetSelectedColor(selectedColor);    // Scales keep changing so need to set color them every update
            }

            else if (selectedType == SelectedType.YAxisScale)
            {
                float[] scalingFactor = {700.0f, 2000.0f};
                
                float currDist = Math.Abs(currPos.y - originPos.y);
                float prevDist = Math.Abs(originalMousePos.y - originPos.y);
                float diff = currDist - prevDist;
                float delta = diff * Math.Abs(originPos.z) / scalingFactor[inputType]; // the further the player is from the graph, the higher the sensitivity
                SetYIntervalSpace(yIntervalSpace + Math.Round((double)delta, 1));
                originalMousePos = currPos;
                SetSelectedColor(selectedColor);    // Scales keep changing so need to set color them every update
            }

            else if (selectedType == SelectedType.XAxisTipPos)
            {
                float[] scalingFactor = {1000.0f, 500.0f};

                float currDist = Math.Abs(currPos.x - originPos.x);
                float prevDist = Math.Abs(originalMousePos.x - originPos.x);
                float diff = currDist - prevDist;
                float delta = diff * Math.Abs(originPos.z) / scalingFactor[inputType]; // the further the player is from the graph, the higher the sensitivity
                SetXAxisLengthPos(xAxisLengthPos + Math.Round((double)delta, 1));
                originalMousePos = currPos;
            }

            else if (selectedType == SelectedType.YAxisTipPos)
            {
                float[] scalingFactor = {500.0f, 500.0f};
                
                float currDist = Math.Abs(currPos.y - originPos.y);
                float prevDist = Math.Abs(originalMousePos.y - originPos.y);
                float diff = currDist - prevDist;
                float delta = diff * Math.Abs(originPos.z) / scalingFactor[inputType]; // the further the player is from the graph, the higher the sensitivity
                SetYAxisLengthPos(yAxisLengthPos + Math.Round((double)delta, 1));
                originalMousePos = currPos;
            }

            else if (selectedType == SelectedType.XAxisTipNeg)
            {
                float[] scalingFactor = { 1000.0f, 500.0f };
                
                float currDist = Math.Abs(currPos.x - originPos.x);
                float prevDist = Math.Abs(originalMousePos.x - originPos.x);
                float diff = currDist - prevDist;
                float delta = diff * Math.Abs(originPos.z) / scalingFactor[inputType]; // the further the player is from the graph, the higher the sensitivity
                SetXAxisLengthNeg(xAxisLengthNeg + Math.Round((double)delta, 1));
                originalMousePos = currPos;
            }
            
            else if (selectedType == SelectedType.YAxisTipNeg)
            {
                float[] scalingFactor = { 1000.0f, 1000.0f };
                
                float currDist = Math.Abs(currPos.y - originPos.y);
                float prevDist = Math.Abs(originalMousePos.y - originPos.y);
                float diff = currDist - prevDist;
                float delta = diff * Math.Abs(originPos.z) / scalingFactor[inputType]; // the further the player is from the graph, the higher the sensitivity
                SetYAxisLengthNeg(yAxisLengthNeg + Math.Round((double)delta, 1));
                originalMousePos = currPos;
            }

            else if (selectedType == SelectedType.XAxisLine)
            {
                float[] scalingFactor = { 2000.0f, 2000.0f };

                float currDist = Math.Abs(currPos.x - originPos.x);
                float prevDist = Math.Abs(originalMousePos.x - originPos.x);
                float diff = currDist - prevDist;
                float delta = diff * Math.Abs(originPos.z) / scalingFactor[inputType]; // the further the player is from the graph, the higher the sensitivity
                SetXIntervalSize(xIntervalSize - Math.Round((double)delta, 1));
                originalMousePos = currPos;
            }
            else if (selectedType == SelectedType.YAxisLine)
            {
                float[] scalingFactor = { 1000.0f, 2000.0f };

                float currDist = Math.Abs(currPos.y - originPos.y);
                float prevDist = Math.Abs(originalMousePos.y - originPos.y);
                float diff = currDist - prevDist;
                float delta = diff * Math.Abs(originPos.z) / scalingFactor[inputType]; // the further the player is from the graph, the higher the sensitivity
                SetYIntervalSize(yIntervalSize - Math.Round((double)delta, 1));
                originalMousePos = currPos;
            }
        }
        else
        {
            SelectObject();
        }
    }

    // Select an object on graph axes based on cursor position
    private void SelectObject()
    {
        SetSelectedColor(defaultColor);

        RaycastHit hitInfo = new RaycastHit();
        // If controllers are connected, check both controllers. Operation holds a right dominance for selecting objects. 
        if(RightCon != null)
        {
            if(Physics.Raycast((RightCon.transform.position), RightCon.transform.forward, out hitInfo, Mathf.Infinity, mask))
            {
                selectedType = TypeSelected(hitInfo);
                if (selected != null)
                {
                    SetSelectedColor(selectedColor);
                }
            }
            else if(Physics.Raycast((LeftCon.transform.position), LeftCon.transform.forward, out hitInfo, Mathf.Infinity, mask))
            {
                selectedType = TypeSelected(hitInfo);
                if (selected != null)
                {
                    SetSelectedColor(selectedColor);
                }
            }
        }// Check for axis hit on mouse input. 
        else
        {
            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo))
            {
                selectedType = TypeSelected(hitInfo);
                if (selected != null)
                {
                    SetSelectedColor(selectedColor);
                }
            }
        }
        
    }

    private SelectedType TypeSelected(RaycastHit hitInfo)
    {
        selected = hitInfo.transform.gameObject;
        if (selected.name.Equals("xScale"))
        {
            return SelectedType.XAxisScale;
        }
        else if (selected.name.Equals("yScale"))
        {
            return SelectedType.YAxisScale;
        }
        else if (selected.name.Equals("xAxisLinePos") || selected.name.Equals("xAxisLineNeg"))
        {
            return SelectedType.XAxisLine;
        }
        else if (selected.name.Equals("yAxisLinePos") || selected.name.Equals("yAxisLineNeg"))
        {
            return SelectedType.YAxisLine;
        }
        else if (selected.name.Equals("xTipPos"))
        {
            return SelectedType.XAxisTipPos;
        }
        else if (selected.name.Equals("yTipPos"))
        {
            return SelectedType.YAxisTipPos;
        }
        else if (selected.name.Equals("xTipNeg"))
        {
            return SelectedType.XAxisTipNeg;
        }
        else if (selected.name.Equals("yTipNeg"))
        {
            return SelectedType.YAxisTipNeg;
        }
        else
        {
            selected = null;
            return SelectedType.None;
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

    private void rightTriggerManipStart(InputAction.CallbackContext obj)
    {
        originalMousePos = Camera.main.WorldToScreenPoint(RightCon.transform.position + RightCon.transform.forward * RightCon.GetComponent<XRInteractorLineVisual>().lineLength);
        enabledGraphManip = true;
        RightTrigActive = true;
    }

    private void rightTriggerManipEnd(InputAction.CallbackContext obj)
    {
        SetSelectedColor(defaultColor);
        enabledGraphManip = false;
        selectedType = SelectedType.None;
        selected = null;
        RightTrigActive = false;
    }

    private void leftTriggerManipStart(InputAction.CallbackContext obj)
    {
        originalMousePos = Camera.main.WorldToScreenPoint(LeftCon.transform.position + LeftCon.transform.forward * LeftCon.GetComponent<XRInteractorLineVisual>().lineLength);
        enabledGraphManip = true;
        LeftTrigActive = true;
    }
    private void leftTriggerManipEnd(InputAction.CallbackContext obj)
    {
        SetSelectedColor(defaultColor);
        enabledGraphManip = false;
        selectedType = SelectedType.None;
        selected = null;
        LeftTrigActive = false;
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
        else if (selected == null)
        {   
            // Catch case for any thing that null
            return;
        }
        else
        {
            selected.GetComponent<Renderer>().material.color = color;
        }
    }
}