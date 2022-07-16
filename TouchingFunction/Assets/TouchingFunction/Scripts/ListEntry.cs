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


    public void delayUpdate()
    {
        BTN.onClick.AddListener(delegate { onDeleteBtnClicked(); });
        TMP.text = "y = " + MeshC.functionText;
        MeshC.GetComponent<Renderer>().material.color = MeshC.c;
    }

    // upon clicking the delete button, send out a msg of {1, ListIndex};
    void onDeleteBtnClicked()
    {
 
        FindObjectOfType<MeshManager>().SendDeleteEntry(ListIndex);

    }

    public void SelfExplode()
    {
        Debug.Log("DELETE LIST INDEX " + ListIndex);
        BTN.onClick.RemoveAllListeners();
        Destroy(this.gameObject);
    }

}
