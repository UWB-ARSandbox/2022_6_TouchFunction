using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateAppId : MonoBehaviour
{

    InputField inputField;
    public GameObject textBox;
    void Awake()
    {
        inputField = GetComponent<InputField>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateWolframAppId(){
        string AppId = inputField.text;

        FindObjectOfType<WolframAlpha>().setAppId(AppId);
        textBox.SetActive(false);
    }
}
