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
    Transform graphAxis;

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
    }

    public void Deselect()
    {
        line.enabled = false;
        textMeshPro.gameObject.SetActive(false);
        GetComponentInChildren<MeshRenderer>().material.color = Color.white;
    }

    public void Select()
    {
        line.enabled = true;
        textMeshPro.gameObject.SetActive(true);
        GetComponentInChildren<MeshRenderer>().material.color = Color.yellow;
    }
}
