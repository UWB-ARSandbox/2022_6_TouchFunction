using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControls : MonoBehaviour
{
    #region Funciton Input
    public GameObject FunctionInputPanel;
    public GameObject FunctionInputButton;
    #endregion

    #region Graph Control
    public GameObject GraphControlPanel;
    public GameObject GraphControlButton;
    #endregion

    #region Flexible Color Picker
    public GameObject FlexibleColorPickerPanel;
    public GameObject FlexibleColorPickerButton;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        // Delay needed to ensure all MeshGen have been instantiated before UI is reset
        Invoke("ResetUI", .1f);
    }



    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisableBaseUI()
    {
        FunctionInputButton.SetActive(false);
        GraphControlButton.SetActive(false);
        FlexibleColorPickerButton.SetActive(false);
    }

    public void ResetUI()
    {
        // Function Input
        FunctionInputPanel.SetActive(false);
        FunctionInputButton.SetActive(true);

        // GraphControl
        GraphControlPanel.SetActive(false);
        GraphControlButton.SetActive(true);

        // Flexible Color Picker
        FlexibleColorPickerPanel.SetActive(false);
        FlexibleColorPickerButton.SetActive(true);
    }

    public void SetFunctionInputActive()
    {
        DisableBaseUI();
        FunctionInputPanel.SetActive(true);
        
    }

    public void SetGraphControlActive()
    {
        DisableBaseUI();
        GraphControlPanel.SetActive(true);
    }

    public void SetFlexibleColorPickerActive()
    {
        DisableBaseUI();
        FlexibleColorPickerPanel.SetActive(true);
    }

    
}