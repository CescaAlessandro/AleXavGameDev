using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class TrailManager : MonoBehaviour
{
    public GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        MapUtility.Cables = new List<Cable>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var cable in MapUtility.Cables)
        {
            //aggiorno e salvo la posizione del cavo solo se questo è maneggiato da Cip
            if (cable.IsConnectedToCip)
            {
                var trail = cable.Instance.GetComponent<TrailRenderer>();
                var positions = new Vector3[trail.positionCount];
                trail.GetPositions(positions);
                var positionsList = positions.ToList();

                positionsList.Remove(target.transform.position);

                trail.Clear();
                trail.AddPositions(positionsList.ToArray());

                cable.Instance.transform.position = target.transform.position;
                var newPosition = new Tuple<float, float>(cable.Instance.transform.position.x, cable.Instance.transform.position.z);
                cable.AddPosition(newPosition);
            }
        }
    }
}
