using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class PointsCreator : MonoBehaviour
{

    public GameObject pointSphere;
    public Vector3[] points;
    public List<GameObject> pointsObject;
    
    // Start is called before the first frame update
    void Start()
    {
        pointsObject = new List<GameObject>();
    }

    
    

   
    public void CreatePoints()
    {
        //DeletePoints();

        int minVal = GetComponent<MeshCreator>().minVal;
        int maxVal = GetComponent<MeshCreator>().maxVal;
        int meshPerX = GetComponent<MeshCreator>().MeshPerX;
        float[] yVal = GetComponent<MeshCreator>().yVals;
        Vector3 origin = GetComponent<MeshCreator>().origin;

        points = new Vector3[ Mathf.Abs(Mathf.Abs(maxVal) - Mathf.Abs(minVal)) * meshPerX ];

        float inc = 1f/meshPerX;
        for(int i = 0; i < maxVal * meshPerX; i ++ )
        {
            float x = origin.x + minVal + (i * inc);
            points[i] = new Vector3(x, origin.y + yVal[i], -.05f);
            GameObject newPoint = UnityEngine.Object.Instantiate(pointSphere, points[i], Quaternion.Euler(90f,0,0), gameObject.transform);
            newPoint.GetComponent<Point>().coordinates = new Vector3(minVal + (i * inc), yVal[i], 0f);
            newPoint.GetComponent<Point>().parentObject = gameObject;
            pointsObject.Add(newPoint);
        }
                
    }

    public void DeletePoints()
    {
        foreach(GameObject point in pointsObject)
        {
            Destroy(point);
        }
    }
}
