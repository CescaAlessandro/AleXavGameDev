using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    public new Camera camera;
    public GameObject block;
    public Grid grid;

    private GameObject pickedUpObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (pickedUpObject != null)
        {
            Vector3 MouseworldPoint = camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 65.0f));
            //Debug.Log(MouseworldPoint);
            pickedUpObject.transform.position = grid.GetCellCenterWorld(grid.WorldToCell(new Vector3(MouseworldPoint.x, pickedUpObject.transform.position.y, MouseworldPoint.z)));
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                Ray m_ray = camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit m_hit;
                bool result = Physics.Raycast(m_ray, out m_hit);
                if (result)
                {
                    pickedUpObject = m_hit.transform.gameObject;
                }
            }
        }
        /*
        Ray m_ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit m_hit;
        bool result = Physics.Raycast(m_ray, out m_hit);
        if (result)
        {
            Instantiate(block, grid.GetCellCenterWorld(grid.WorldToCell(m_hit.point)),Quaternion.identity,grid.transform);
        }
        */
    }
}
