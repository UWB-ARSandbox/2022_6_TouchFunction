using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Point : MonoBehaviour
{
    [SerializeField] Vector3 coordinates;
    public LineRenderer line;
    public TextMeshPro textMeshPro;
    public GameObject dragPoint;

    public GameObject lineToX;
    public GameObject lineToY;
    Transform graphAxis;

    public bool selected = false;

    void Start()
    {
        coordinates = GetComponentInParent<MeshCreator>().FindClosestPoint(transform.position);
        textMeshPro.text = String.Format("({0:0.00}, {1:0.00})", coordinates.x, coordinates.y);

        graphAxis = FindObjectOfType<GraphManipulation>().transform;
    }

    void Update()
    {
        float axisX = graphAxis.position.x;
        float axisY = graphAxis.position.y;
 
        if (line.enabled)
        {
            line.SetPositions(new Vector3[] { new Vector3(transform.position.x, axisY, transform.position.z),
                                            transform.position,
                                            new Vector3(axisX, transform.position.y, transform.position.z) });
        }

        transform.localScale = new Vector3( 1f / transform.parent.localScale.x,
                                            1f / transform.parent.localScale.y,
                                            1f / transform.parent.localScale.z );

        

        textMeshPro.transform.rotation = Quaternion.LookRotation(textMeshPro.transform.position - Camera.main.transform.position, Vector3.up);

        dragPoint.transform.position = new Vector3(transform.position.x, axisY, transform.position.z);
    }

    // public void Deselect()
    // {
    //     selected = false;
    //     line.enabled = false;
    //     dragPoint.SetActive(false);
    //     textMeshPro.gameObject.SetActive(false);
    //     GetComponentInChildren<MeshRenderer>().material.color = Color.white;
    // }

    public void Select()
    {
        if(selected) {
            selected = false;
            line.enabled = false;
            dragPoint.SetActive(false);
            textMeshPro.gameObject.SetActive(false);
            GetComponentInChildren<MeshRenderer>().material.color = Color.white;
            return;
        }

        selected = true;
        line.enabled = true;
        dragPoint.SetActive(true);
        textMeshPro.gameObject.SetActive(true);
        GetComponentInChildren<MeshRenderer>().material.color = Color.blue;
    }

    public void Hover()
    {
        GetComponentInChildren<MeshRenderer>().material.color = Color.yellow;
    }

    public void UnHover()
    {
        GetComponentInChildren<MeshRenderer>().material.color = selected ? Color.black : Color.white;
    }


    public void UpdatePosition(float newXPos)
    {
        RaycastHit hit;
        Physics.Raycast(new Vector3(newXPos, transform.position.y + 1f, transform.position.z), Vector3.down, out hit, 2f, LayerMask.GetMask("GraphMesh"));


        transform.position = hit.point;
        coordinates = GetComponentInParent<MeshCreator>().FindClosestPoint(transform.position);
        textMeshPro.text = String.Format("({0:0.00}, {1:0.00})", coordinates.x, coordinates.y);
    }


}
