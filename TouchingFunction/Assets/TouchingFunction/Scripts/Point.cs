using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Point : MonoBehaviour
{
    [SerializeField] Vector2 coordinates;
    [SerializeField] TextMeshPro textMeshPro;
    [SerializeField] GameObject point;
    [SerializeField] GameObject handle;

    [SerializeField] MeshRenderer pointRenderer;
    [SerializeField] MeshRenderer handleRenderer;

    [SerializeField] GameObject lineToX;
    [SerializeField] GameObject lineToY;
    Transform graphAxis;

    public bool selected = false;

    void Start()
    {
        coordinates = GetComponentInParent<MeshCreator>().WorldToCartesian(transform.position);
        textMeshPro.text = String.Format("({0:0.00}, {1:0.00})", coordinates.x, coordinates.y);

        point.transform.localScale = new Vector3(0.1f, GetComponentInParent<MeshCreator>().width / 2f, 0.1f);

        graphAxis = FindObjectOfType<GraphManipulation>().transform;

        lineToX.transform.localRotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
        lineToY.transform.localRotation = Quaternion.LookRotation(Vector3.forward, Vector3.right);
    }

    void Update()
    {
        float axisX = graphAxis.position.x;
        float axisY = graphAxis.position.y;

        float pointX = transform.position.x;
        float pointY = transform.position.y;
        float pointZ = transform.position.z;

        if (lineToX.activeInHierarchy)
        {
            lineToX.transform.position = new Vector3(pointX, (axisY + pointY) / 2f, pointZ);
            lineToX.transform.localScale = new Vector3(0.05f, (pointY - axisY) / 2f, 0.05f);

            lineToY.transform.position = new Vector3((axisX + pointX) / 2f, pointY, pointZ);
            lineToY.transform.localScale = new Vector3(0.05f, (pointX - axisX) / 2f, 0.05f);
        }

        transform.localScale = new Vector3(1f / transform.parent.localScale.x,
                                            1f / transform.parent.localScale.y,
                                            1f / transform.parent.localScale.z);



        textMeshPro.transform.rotation = Quaternion.LookRotation(textMeshPro.transform.position - Camera.main.transform.position, Vector3.up);

        handle.transform.position = new Vector3(transform.position.x, axisY, transform.position.z);
    }

    public void Select()
    {
        if (!selected)
        {   // Select
            selected = true;
            lineToX.SetActive(true);
            lineToY.SetActive(true);
            handle.SetActive(true);
            pointRenderer.material.color = Color.blue;
            handleRenderer.material.color = Color.blue;
        }
        else
        {   // Deselect
            selected = false;
            lineToX.SetActive(false);
            lineToY.SetActive(false);
            handle.SetActive(false);
            textMeshPro.gameObject.SetActive(false);
            pointRenderer.material.color = Color.blue;
            handleRenderer.material.color = Color.white;
        }
    }

    public void Hover()
    {
        pointRenderer.material.color = Color.yellow;
        handleRenderer.material.color = Color.yellow;

        if (selected) return;

        lineToX.SetActive(true);
        lineToY.SetActive(true);
        textMeshPro.gameObject.SetActive(true);

    }

    public void UnHover()
    {
        pointRenderer.material.color = selected ? Color.blue : Color.white;
        handleRenderer.material.color = selected ? Color.blue : Color.white;

        if (selected) return;

        lineToX.SetActive(false);
        lineToY.SetActive(false);
        textMeshPro.gameObject.SetActive(false);
    }


    public void DragPosition(Vector3 newPos)
    {
        RaycastHit hit;
        Physics.Raycast(new Vector3(newPos.x, transform.position.y + 1f, transform.position.z), Vector3.down, out hit, 2f, LayerMask.GetMask("GraphMesh"));

        transform.position = hit.point;
        coordinates = GetComponentInParent<MeshCreator>().WorldToCartesian(transform.position);
        textMeshPro.text = String.Format("({0:0.00}, {1:0.00})", coordinates.x, coordinates.y);
    }

    public void UpdatePosition()
    {
        Vector3 newPos = GetComponentInParent<MeshCreator>().CartesianToWorld(coordinates);
        transform.position = newPos;
    }


}
