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
    // Called when Chip moves
    public void cableUpdate() {
        Debug.Log("new");
        foreach (var cable in MapUtility.Cables)
        {
            //aggiorno e salvo la posizione del cavo solo se questo � maneggiato da Cip
            if (cable.IsConnectedToCip)
            {
                var trail = cable.Instance.GetComponent<TrailRenderer>();
                var positions = new Vector3[trail.positionCount];
                trail.GetPositions(positions);
                var positionsList = positions.ToList();
                if (positionsList.Count == 0)
                {
                    positionsList.Add(target.transform.position);
                }
                else if(positionsList.Last() == target.transform.position)
                {
                    //positionsList.Remove(target.transform.position);
                }
                else if(positionsList.Count() >1)
                {
                    if(positionsList[positionsList.Count() - 2] == target.transform.position)
                    {
                        positionsList.RemoveAt(positionsList.Count() - 1);
                    }
                    else if (positionsList.Last().x != target.transform.position.x && positionsList.Last().z != target.transform.position.z)
                    {
                        addAllBetween(positionsList.Last(), new Vector3(positionsList.Last().x, positionsList.Last().y, target.transform.position.z),positionsList);
                        addAllBetween(positionsList.Last(), new Vector3(target.transform.position.x, positionsList.Last().y, positionsList.Last().z), positionsList);
                    }
                    else if (positionsList.Last().x != target.transform.position.x)
                    {
                        addAllBetween(positionsList.Last(), new Vector3(target.transform.position.x, positionsList.Last().y, positionsList.Last().z), positionsList);
                    }
                    else if (positionsList.Last().z != target.transform.position.z)
                    {
                        foreach (var vec in positionsList)
                               Debug.Log(vec);
                        Debug.Log("");
                        addAllBetween(positionsList.Last(), new Vector3(positionsList.Last().x, positionsList.Last().y, target.transform.position.z), positionsList);
                    }
                    else
                    {
                        Debug.LogError("Something wrong with movement");
                    }
                }
                else
                {
                    if (positionsList.Last().x != target.transform.position.x && positionsList.Last().z != target.transform.position.z)
                    {
                        addAllBetween(positionsList.Last(), new Vector3(positionsList.Last().x, positionsList.Last().y, target.transform.position.z), positionsList);
                        addAllBetween(positionsList.Last(), new Vector3(target.transform.position.x, positionsList.Last().y, positionsList.Last().z), positionsList);
                    }
                    else if (positionsList.Last().x != target.transform.position.x)
                    {
                        addAllBetween(positionsList.Last(), new Vector3(target.transform.position.x, positionsList.Last().y, positionsList.Last().z), positionsList);
                    }
                    else if (positionsList.Last().z != target.transform.position.z)
                    {
                        Debug.Log(target.transform.position);
                        addAllBetween(positionsList.Last(), new Vector3(positionsList.Last().x, positionsList.Last().y, target.transform.position.z), positionsList);
                    }
                    else
                    {
                        Debug.LogError("Something wrong with movement");
                    }
                }
                trail.Clear();
                trail.AddPositions(positionsList.ToArray());
                var positionas = new Vector3[trail.positionCount];
                trail.GetPositions(positionas);
                //foreach (var vec in positionas)
                //    Debug.Log(vec);
                ///Debug.Log("");
            }
        }
    }
    public bool isLastMove(Vector3 move)
    {
        foreach (var cable in MapUtility.Cables)
        {
            //aggiorno e salvo la posizione del cavo solo se questo � maneggiato da Cip
            if (cable.IsConnectedToCip)
            {
                var trail = cable.Instance.GetComponent<TrailRenderer>();
                var positions = new Vector3[trail.positionCount];
                trail.GetPositions(positions);
                var positionsList = positions.ToList();
                if (!(trail.positionCount < 2))
                { 
                    if (positionsList.ElementAt(trail.positionCount - 2).x == move.x && (positionsList.ElementAt(trail.positionCount - 2).z == move.z))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }
        return false;
    }

    public void UpdateCablePointsOnDetach()
    {
        var cable = MapUtility.Cables.First(cableA => cableA.IsConnectedToCip);
        var trail = cable.Instance.GetComponent<TrailRenderer>();
        var positions = new Vector3[trail.positionCount];
        trail.GetPositions(positions);
        var positionsList = positions.ToList();

        //rimuovo l'ultimo punto e porto Chip sul penultimo
        positionsList.Remove(positionsList.Last());
        target.transform.position = positionsList.Last();

        trail.Clear();
        trail.AddPositions(positionsList.ToArray());
        var positionas = new Vector3[trail.positionCount];
        trail.GetPositions(positionas);
    }

    public void addAllBetween(Vector3 last,Vector3 dest, List<Vector3> list)
    {
        var xCurrent = last.x;
        var yCurrent = last.z;
        if (last.x == dest.x)
        {
            if (yCurrent < dest.z)
            {
                yCurrent += 100;
            }
            else
            {
                yCurrent -= 100;
            }
            while(yCurrent != dest.z)
            {
                list.Add(new Vector3(xCurrent, 0, yCurrent));
                if (yCurrent < dest.z)
                {

                    yCurrent += 100;
                }
                else
                {
                    yCurrent -= 100;
                }
            }
        }
        else if(last.z == dest.z)
        {
            if (xCurrent < dest.z)
            {
                xCurrent += 100;
            }
            else
            {
                xCurrent -= 100;
            }
            while (xCurrent != dest.x)
            {
                list.Add(new Vector3(xCurrent, 0, yCurrent));
                if (xCurrent < dest.x)
                {
                    xCurrent += 100;
                }
                else
                {
                    xCurrent -= 100;
                }
            }
        }
        else
        {
            Debug.LogError("Diagonal Movemnet not allowed here");
        }
    }
}
