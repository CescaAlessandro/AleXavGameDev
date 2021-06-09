using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public static class MapUtility
{
    public static bool IsChipWiring { get; set; } = false;
    //flag per prevenire che un cavo scollegato venga riattaccato subito 
    //(se Chip è nelle vicinanze del LowerPin con cui ha appena interagito)
    public static bool PreventReattaching { get; set; } = false;

    public static List<Pin> UpperPins { get; set; }
    public static List<Pin> LowerPins { get; set; }
    public static List<Cable> Cables { get; set; }

    public static float range = 100;
    private static bool[,] collisionMap = new bool[11, 9];

    public static void SetWiring(bool flag)
    {
        IsChipWiring = flag;
    }
    /*
    public static bool LookForCollision(Tuple<float, float> start, Tuple<float, float> finish)
    {
        bool result = false;

        var cablesNotConnected = Cables.Where(cable => !cable.IsConnectedToCip).ToList();

        foreach (var cable in cablesNotConnected)
        {
            foreach (var position in cable.WirePositions)
            {
                var AB = Math.Sqrt((finish.Item1 - start.Item1) * (finish.Item1 - start.Item1) + (finish.Item2 - start.Item2) * (finish.Item2 - start.Item2));
                var AP = Math.Sqrt((position.Item1 - start.Item1) * (position.Item1 - start.Item1) + (position.Item2 - start.Item2) * (position.Item2 - start.Item2));
                var PB = Math.Sqrt((finish.Item1 - position.Item1) * (finish.Item1 - position.Item1) + (finish.Item2 - position.Item2) * (finish.Item2 - position.Item2));
                if (AB == AP + PB)
                {
                    result = true;
                    return result;
                }
            }
        }

        return result;
    }
    */
    public static bool LookForCollision(Tuple<float, float> start, Tuple<float, float> finish)
    {
        var startMapConvertionX = Math.Round(((-start.Item1) + 400) / 100,0);
        var startMapConvertionY = Math.Round(((start.Item2) + 500) / 100,0);
        var finishMapConvertionX = Math.Round(((-finish.Item1) + 400) / 100,0);
        var finishMapConvertionY = Math.Round(((finish.Item2) + 500) / 100,0);
        //Debug.Log(finishMapConvertionY);
        //Debug.Log(startMapConvertionY);
        //Debug.Log("");
        /*
        for(int i= 0; i< 11; i++)
        {
            for(int j = 0; j < 9; j++)
            {
                Debug.Log(collisionMap[i, j]);
            }
            Debug.Log("");

        }
        Debug.Log(""); Debug.Log("");
        */
        float currentX = (float)startMapConvertionX;
        float currentY = (float)startMapConvertionY;
        bool[,] mapClone = (bool[,])collisionMap.Clone();
        while (currentX != finishMapConvertionX || currentY != finishMapConvertionY )
        {
            if(currentY == finishMapConvertionY)
            {
                if (currentX < finishMapConvertionX)
                {
                    currentX++;
                }
                else
                {
                    currentX--;
                }
                if (collisionMap[(int)Math.Round(currentY, 0), (int)Math.Round(currentX, 0)])
                {
                    /*
                    string str = "";
                    for (int i = 0; i < 11; i++)
                    {

                        for (int j = 0; j < 9; j++)
                        {
                            str += collisionMap[i, j];
                            str += " ";
                        }
                        Debug.Log(str);
                        str = "";
                    }
                    Debug.Log("");
                    */

                    PrintCollisionMap();
                    if (TrailManager.Instance().isLastMove(new Vector3(((currentX*100)-400)*(-1f),0,(currentY*100)-500)))
                    {
                        mapClone[(int)Math.Round(currentY,0), (int)Math.Round(currentX,0)] = false;
                    }
                    else{
                        return true;
                    }
                }
                else
                {
                    mapClone[(int)Math.Round(currentY, 0), (int)Math.Round(currentX, 0)] = true;
                }
            }
            else
            {
                if (currentY < finishMapConvertionY)
                {
                    //Debug.Log(finishMapConvertionY);
                    currentY++;
                }
                else
                {
                    currentY--;
                }
                //Debug.Log((int)Math.Round(currentY, 0));
                //Debug.Log((int)Math.Round(finishMapConvertionX, 0));
                if (collisionMap[(int)Math.Round(currentY, 0), (int)Math.Round(currentX, 0)])
                {
                    /*
                    string str = "";
                    for (int i = 0; i < 11; i++)
                    {

                        for (int j = 0; j < 9; j++)
                        {
                            str += collisionMap[i, j];
                            str += " ";
                        }
                        Debug.Log(str);
                        str = "";
                    }
                    Debug.Log("");
                    */
                    PrintCollisionMap();
                    return true;
                }
                else
                {
                    mapClone[(int)Math.Round(currentY, 0), (int)Math.Round(currentX, 0)] = true;
                }
            }
        }
        collisionMap = mapClone;
        return false;
    }
        //controlla se Chip è vicino ad un pin
        //se vero => resituisce tupla con posizione pin e true
        //se false => resituisce tupla con posizione fake e falso
        public static Tuple<bool, Pin> IsPositionNearPin(Vector3 position)
    {
        var pins = UpperPins.Union(LowerPins).ToList();

        if (pins != null)
        {
            //N.B.: Pin child(0) = attachment
            var pinFound = pins.FirstOrDefault(pinPosition => Vector3.Distance(pinPosition.Instance.transform.GetChild(0).position, position) < range);

            if (pinFound != null)
                return new Tuple<bool, Pin>(true, pinFound);
            return new Tuple<bool, Pin>(false, null);
        }
        return new Tuple<bool, Pin>(false, null);
    }

    public static void setCollisionMap(float x,float y, bool value)
    {
        var MapConvertionX = ((-x) + 400) / 100;
        var MapConvertionY = (y + 500) / 100;
        collisionMap[(int)Math.Round(MapConvertionY, 0), (int)Math.Round(MapConvertionX, 0)] = value;
    }

    public static List<GameObject> GetAllObjectsOnlyInScene()
    {
        List<GameObject> objectsInScene = new List<GameObject>();

        foreach (GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
        {
            if (!EditorUtility.IsPersistent(go.transform.root.gameObject) && !(go.hideFlags == HideFlags.NotEditable || go.hideFlags == HideFlags.HideAndDontSave))
                objectsInScene.Add(go);
        }

        return objectsInScene;
    }

    //per debug...
    public static void PrintCollisionMap()
    {
        var matrix = "";
        for(int i = 0; i<11; i++)
        {
            var line = "";
            for (int j = 0; j < 9; j++)
            {
                if(collisionMap[i, j])
                    line = line + "[x]";
                else
                    line = line + "[-]";
            }
            matrix = matrix + line + '\n';
        }
        Debug.Log(matrix);
    }
}
