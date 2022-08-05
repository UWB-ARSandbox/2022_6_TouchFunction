using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ASL;

public class ListEntry : MonoBehaviour
{
    public int ListIndex;
    //public int MatIndex;
    public MeshCreator MeshC;
    public Button BTN;
    public TextMeshProUGUI TMP;
    public GameObject FunctionDisplayButton;

    public bool NeedButton;

    public void Awake()
    {
        transform.rotation = GameObject.Find("Canvas").transform.rotation;
        // FunctionDisplayButton = FindObjectOfType<FuncitonPropertyManager>().GetLastButton();
        // FunctionDisplayButton.transform.position = new Vector3(FunctionDisplayButton.transform.position.x, gameObject.transform.position.y, 0f);
        //NeedButton = true;
    }
    public void delayUpdate()
    {
        BTN.onClick.AddListener(delegate { onDeleteBtnClicked(); });
        TMP.text = "y = " + MeshC.functionText;
        MeshC.GetComponent<Renderer>().material.color = MeshC.c;
        if(FindObjectOfType<GraphListButtonControl>().needButton(ListIndex))
        {
            Debug.Log("Trying to find button");
            FunctionDisplayButton = FindObjectOfType<FuncitonPropertyManager>().GetLastButton();
            if(FunctionDisplayButton != null)
            {
                Debug.Log("Update button location");
                FunctionDisplayButton.SetActive(true);
                //FunctionDisplayButton.transform.position = new Vector3(FunctionDisplayButton.transform.position.x, gameObject.transform.position.y, GameObject.Find("Canvas").transform.position.z);
                FindObjectOfType<GraphListButtonControl>().claimButton(ListIndex, FunctionDisplayButton);
            }
        }
        else 
        {
            FunctionDisplayButton = FindObjectOfType<GraphListButtonControl>().GetButton(ListIndex);
        }
    }

    void Update()
    {
        
    }


    // upon clicking the delete button, send out a msg of {1, ListIndex};
    void onDeleteBtnClicked()
    {
 
        
        // FunctionDisplayButton.SetActive(false);
        // FindObjectOfType<FuncitonPropertyManager>().ReleaseButton(ListIndex);
        // FindObjectOfType<GraphListButtonControl>().releaseButton(ListIndex);
        FunctionDisplayButton.transform.parent.GetComponent<FunctionPropertyDisplay>().SendDelete();
        FindObjectOfType<MeshManager>().SendDeleteEntry(ListIndex);
    }

    public void SelfExplode()
    {
        Debug.Log("DELETE LIST INDEX " + ListIndex);
        BTN.onClick.RemoveAllListeners();
        Destroy(this.gameObject);
    }

}
