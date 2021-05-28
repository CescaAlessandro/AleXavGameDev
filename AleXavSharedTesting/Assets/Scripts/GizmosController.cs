
using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(MeshCollider))]

public class GizmosController : MonoBehaviour
{
    private Vector3 screenPoint;
    private Vector3 offset;
    public float gridSize = 1f;

    void OnMouseDown()
    {
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);

        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }

    void OnMouseDrag()
    {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;

        curPosition.x = ((int)(curPosition.x / gridSize)) * gridSize;

        if (curPosition.x < -40)
            curPosition.x = -40;

        if (curPosition.x > 40)
            curPosition.x = 40;


        curPosition.z = ((int)(curPosition.z / gridSize)) * gridSize;


        if (curPosition.z < -75)
            curPosition.z = -75;

        if (curPosition.z > 75)
            curPosition.z = 75;

        transform.position = curPosition;      
    }
}
