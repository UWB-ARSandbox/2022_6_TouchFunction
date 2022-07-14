using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISelection : MonoBehaviour
{
    [SerializeField] List<GameObject> UIElements;
    public void SelectUIElement(int n)
    {
        if(n >= UIElements.Count)
        {
            Debug.LogError("No gameobject is setup at index " + n + " for selection");
        }
        UIElements.ForEach((g) => g.SetActive(false));
        UIElements[n].SetActive(true);
    }
}
