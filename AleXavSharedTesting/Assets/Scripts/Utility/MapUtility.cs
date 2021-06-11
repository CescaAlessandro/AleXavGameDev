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

    public static Vector3 GetBestFinalLocationForMovement(Tuple<float, float> start, Tuple<float, float> finish)
    {
        var startMapConvertionX = (int)Math.Round(((-start.Item1) + 400) / 100,0);
        var startMapConvertionY = (int)Math.Round(((start.Item2) + 500) / 100,0);
        var finishMapConvertionX = (int)Math.Round(((-finish.Item1) + 400) / 100,0);
        var finishMapConvertionY = (int)Math.Round(((finish.Item2) + 500) / 100,0);

        float currentX = (float)startMapConvertionX;
        float currentY = (float)startMapConvertionY;
        bool[,] mapClone = (bool[,])collisionMap.Clone();
        List<Vector3> newPoints = new List<Vector3>();
        Vector3 bestLocation = new Vector3(start.Item1, 0, start.Item2);
        while(currentY != finishMapConvertionY)
        {
            var lastY = currentY;
            if (currentY < finishMapConvertionY)
            {
                //Debug.Log(finishMapConvertionY);
                currentY++;
            }
            else
            {
                currentY--;
            }
            if (collisionMap[(int)Math.Round(currentY, 0), (int)Math.Round(currentX, 0)])
            {
                PrintCollisionMap();
                if(TrailManager.Instance().isLastMove(new Vector3(((currentX * 100) - 400) * (-1f), 0, currentY * 100 - 500)))
                {
                    collisionMap[(int)Math.Round(lastY, 0), (int)Math.Round(currentX, 0)] = false;
                    TrailManager.Instance().removeLastPoint();
                    bestLocation = new Vector3(((currentX * 100) - 400) * (-1f), 0, currentY * 100 - 500);
                }
                {
                    return bestLocation;
                }
            }
            else
            {
                collisionMap[(int)Math.Round(currentY, 0), (int)Math.Round(currentX, 0)] = true;
                TrailManager.Instance().addPoint(new Vector3(((currentX * 100) - 400) * (-1f), 0, currentY * 100 - 500));
                bestLocation = new Vector3(((currentX * 100) - 400) * (-1f), 0, currentY * 100 - 500);
                Debug.Log(new Vector3(((currentX * 100) - 400) * (-1f), 0, currentY * 100 - 500));
            }
        }
        while (currentX != finishMapConvertionX)
        {
            var lastX = currentX;
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

                PrintCollisionMap();
                Debug.Log(TrailManager.Instance().getLastMove());
                if (TrailManager.Instance().isLastMove(new Vector3(((currentX * 100) - 400) * (-1f), 0, (currentY * 100) - 500)))
                {
                    collisionMap[(int)Math.Round(currentY, 0), (int)Math.Round(lastX, 0)] = false;
                    TrailManager.Instance().removeLastPoint();
                    bestLocation = new Vector3(((currentX * 100) - 400) * (-1f), 0, currentY * 100 - 500);
                }
                else
                {
                    return bestLocation;
                }
            }
            else
            {
                collisionMap[(int)Math.Round(currentY, 0), (int)Math.Round(currentX, 0)] = true;
                TrailManager.Instance().addPoint(new Vector3(((currentX * 100) - 400) * (-1f), 0, currentY * 100 - 500));
                bestLocation = new Vector3(((currentX * 100) - 400) * (-1f), 0, currentY * 100 - 500);
            }
        }
        PrintCollisionMap();
        Debug.Assert(bestLocation == new Vector3(finish.Item1, 0, finish.Item2),"Target Position not reached");
        return bestLocation;
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


    public class CollisionEntity
    {
        public bool collidesFromAbove;
        public bool collidesFromBelow;
        public bool collidesFromLeft;
        public bool collidesFromRight;

        static CollisionEntity getNoCollisionEntity()
        {
            var ent = new CollisionEntity();
            ent.collidesFromAbove = false;
            ent.collidesFromBelow = false;
            ent.collidesFromLeft = false;
            ent.collidesFromRight = false;
            return ent;
        }
        static CollisionEntity getFullCollisionEntity()
        {
            var ent = new CollisionEntity();
            ent.collidesFromAbove = true;
            ent.collidesFromBelow = true;
            ent.collidesFromLeft = true;
            ent.collidesFromRight = true;
            return ent;
        }
        static CollisionEntity getVerticalCollisionEntity()
        {
            var ent = new CollisionEntity();
            ent.collidesFromAbove = true;
            ent.collidesFromBelow = true;
            ent.collidesFromLeft = false;
            ent.collidesFromRight = false;
            return ent;
        }
        static CollisionEntity getHorizontalCollisionEntity()
        {
            var ent = new CollisionEntity();
            ent.collidesFromAbove = false;
            ent.collidesFromBelow = false;
            ent.collidesFromLeft = true;
            ent.collidesFromRight = true;
            return ent;
        }
    }
}
