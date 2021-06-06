using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class TrailManager : MonoBehaviour
{
    public GameObject target;
    public static TrailManager tm;
    // Start is called before the first frame update
    void Start()
    {
        tm = this;
        MapUtility.Cables = new List<Cable>();
    }
    public static TrailManager Instance()
    {
        return tm;
    }
    // Update is called once per frame
    public void updateCable() {

        foreach (var cable in MapUtility.Cables)
        {
            //aggiorno e salvo la posizione del cavo solo se questo è maneggiato da Cip
            if (cable.IsConnectedToCip)
            {
                cable.Instance.transform.position = new Vector3(target.transform.position.x, cable.Instance.transform.position.y, target.transform.position.z);
                var newPosition = new Tuple<float, float>(cable.Instance.transform.position.x, cable.Instance.transform.position.z);
                cable.AddPosition(newPosition);
            }
        }
    }
}
