using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridHighlighting : MonoBehaviour
{
    public GameObject cubePrefab;
    // Start is called before the first frame update
    void Start()
    {
        //Instantiate(cubePrefab, GetComponent<Grid>().GetCellCenterWorld(new Vector3Int(0, 0, 0)), Quaternion.identity,this.transform);
        //Instantiate(cubePrefab, GetComponent<Grid>().GetCellCenterWorld(new Vector3Int(1, 0, 0)), Quaternion.identity, this.transform);
        //Instantiate(cubePrefab, GetComponent<Grid>().GetCellCenterWorld(new Vector3Int(0, 0, 1)), Quaternion.identity, this.transform);
        //Instantiate(cubePrefab, GetComponent<Grid>().GetCellCenterWorld(new Vector3Int(2, 0, 0)), Quaternion.identity, this.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
