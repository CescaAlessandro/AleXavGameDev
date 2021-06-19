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
    //(se Chip � nelle vicinanze del LowerPin con cui ha appena interagito)
    public static bool PreventReattaching { get; set; } = false;
    public static bool GamePaused { get; set; } = false;

    public static List<Pin> UpperPins { get; set; }
    public static List<Pin> LowerPins { get; set; }
    public static List<Cable> Cables { get; set; }
    public static List<Hole> Holes { get; set; }
    public static List<Bridge> Bridges { get; set; }

    public static GameObject managersRef { get; set; }
    public static CanvasesBehaviour canvasesRef { get; set; }


    public static float range = 100;
    private static CollisionEntity[,] collisionMap = new CollisionEntity[11, 9];

    public static void SetWiring(bool flag)
    {
        IsChipWiring = flag;
    }
    public static void collisionMapBaseSetup()
    {
        for (int i = 0; i < 11; ++i)
        {
            for (int j = 0; j < 9; ++j)
            {
                collisionMap[i,j] = CollisionEntity.getNoCollisionEntity();
            }
        }
        //Debug.Log(collisionMap[1, 4].collidesFromAbove);
    }

    //Dato un punto di partenza e un punto di arrivo sulla board restituisce il punto della board piu' vicino al punto di arrivo raggiungibile dal punto di partenza rispettando i vincoli delle collisioni
    //Se il movimento richiesto e' in diagonale si muovera' verticalmente nella board fino a raggiungere la y desiderata e poi si muovera' orizzontalmente
    public static Vector3 GetBestFinalLocationForMovement(Vector3 start, Vector3 finish)
    {
        //converto i punti di partenza e arrivo in indici della mappa delle collisioni
        var startMapConvertionX = (int)Math.Round(((-start.x) + 400) / 100,0);
        var startMapConvertionY = (int)Math.Round(((start.z) + 500) / 100,0);
        var finishMapConvertionX = (int)Math.Round(((-finish.x) + 400) / 100,0);
        var finishMapConvertionY = (int)Math.Round(((finish.z) + 500) / 100,0);

        float currentX = (float)startMapConvertionX;
        float currentY = (float)startMapConvertionY;
        //bool[,] mapClone = (bool[,])collisionMap.Clone();
        List<Vector3> newPoints = new List<Vector3>();
        Vector3 bestLocation = start;
        while (currentY != finishMapConvertionY)
        {
            //muovo verticalmente fino a la y finale e' raggiunta o una collisione blocca il passaggio
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
            if (collisionMap[(int)Math.Round(currentY, 0), (int)Math.Round(currentX, 0)].collidesFromAbove)
            {
                //Se la casella che si sta tentando di entrare non consente l'entrata dalla direzione verticale
                //PrintCollisionMap();
                if(TrailManager.Instance().isLastMove(new Vector3(((currentX * 100) - 400) * (-1f), 0, currentY * 100 - 500)))
                {
                    //Se la casella che si sta tentando di entrare e' l'ultima mossa -> rimuovo l'ultimo punto e mi muovo sulla casella tenendo conto di eventuali modifiche dovute al tipo di casella
                    //rimuovo la collisione dalla casella corrente
                    //Debug.Log("a");
                    collisionMap[(int)Math.Round(lastY, 0), (int)Math.Round(currentX, 0)].Exiting(directions.Top);
                    TrailManager.Instance().removeLastPoint();
                    bestLocation = new Vector3(((currentX * 100) - 400) * (-1f), 0, currentY * 100 - 500)
                                                 + collisionMap[(int)Math.Round(currentY, 0), (int)Math.Round(currentX, 0)].PassingThroughModifications(directions.Top);
                }
                else
                {
                    //altrimenti il passaggio e' bloccato -> restituisco l'ultima casella come punto piu' vicino al punto di arrivo
                    //Debug.Log(bestLocation);
                    return bestLocation;
                }
            }
            else
            {
                //se non ci sono problemi di collisione
                if (collisionMap[(int)Math.Round(lastY, 0), (int)Math.Round(currentX, 0)].canBeExitedAbove)
                {
                    //controllo che la casella corrente permetta di uscirne verticalmente -> muovo sulla nuova casella e aggiungo questo punto al cavo tenendo conto delle modifiche alla posizione 
                    //dovute al tipo di casella
                    collisionMap[(int)Math.Round(currentY, 0), (int)Math.Round(currentX, 0)].PassingThrough(directions.Top);
                    TrailManager.Instance().addPoint(new Vector3(((currentX * 100) - 400) * (-1f), 0, currentY * 100 - 500)
                                                                    + collisionMap[(int)Math.Round(currentY, 0), (int)Math.Round(currentX, 0)].PassingThroughModifications(directions.Top));
                    bestLocation = new Vector3(((currentX * 100) - 400) * (-1f), 0, currentY * 100 - 500)
                                                                    + collisionMap[(int)Math.Round(currentY, 0), (int)Math.Round(currentX, 0)].PassingThroughModifications(directions.Top);
                    ////Debug.Log(new Vector3(((currentX * 100) - 400) * (-1f), 0, currentY * 100 - 500));
                }
                else
                {
                    //Se non posso uscire da questa casella verticalmente ho raggiunto un blocco -> restituisco l'ultima casella raggiunta
                    //Debug.Log(bestLocation);
                    return bestLocation;
                }
            }
        }
        while (currentX != finishMapConvertionX)
        {
            //muovo orizzontalmente fino a che raggiungo la x corretta o incontro una collisione che blocca il movimento
            var lastX = currentX;
            if (currentX < finishMapConvertionX)
            {
                currentX++;
            }
            else
            {
                currentX--;
            }
            if (collisionMap[(int)Math.Round(currentY, 0), (int)Math.Round(currentX, 0)].collidesFromLeft)
            {
                //Se riscontro una collisione
                //PrintCollisionMap();
                //Debug.Log(TrailManager.Instance().getLastMove());
                if (TrailManager.Instance().isLastMove(new Vector3(((currentX * 100) - 400) * (-1f), 0, (currentY * 100) - 500)))
                {
                    //Se sto cercando di muovermi nella casella dell'ultima mossa -> elimino l'ultimo punto e la collisione su di esso, mi muovo nella nuova casella

                    collisionMap[(int)Math.Round(currentY, 0), (int)Math.Round(lastX, 0)].Exiting(directions.Left);
                    TrailManager.Instance().removeLastPoint();
                    bestLocation = new Vector3(((currentX * 100) - 400) * (-1f), 0, currentY * 100 - 500);
                }
                else
                {
                    //Se non e' la casella dell'ultimo movimento allora non posso andarci -> collisione riscontrata, restituisco l'ultima posizione valida
                    
                    //Debug.Log(bestLocation);
                    return bestLocation;
                }
            }
            else
            {
                //Non c'è collisione

                if (collisionMap[(int)Math.Round(currentY, 0), (int)Math.Round(lastX, 0)].canBeExitedLeft)
                {
                    //Se posso uscire dalla casella corrente orizzontalmente -> mi muovo sulla nuova casella, aggiungo un punto al cavo, aggiungo la collisione del nuovo punto

                    collisionMap[(int)Math.Round(currentY, 0), (int)Math.Round(currentX, 0)].PassingThrough(directions.Left);
                    TrailManager.Instance().addPoint(new Vector3(((currentX * 100) - 400) * (-1f), 0, currentY * 100 - 500)
                                                                    + collisionMap[(int)Math.Round(currentY, 0), (int)Math.Round(currentX, 0)].PassingThroughModifications(directions.Left));
                    bestLocation = new Vector3(((currentX * 100) - 400) * (-1f), 0, currentY * 100 - 500)
                                                                    + collisionMap[(int)Math.Round(currentY, 0), (int)Math.Round(currentX, 0)].PassingThroughModifications(directions.Left);
                }
                else
                {
                    //non posso uscire dalla casella corrente orizzontalmente -> collisione, restituisco l'ultima posizione valida

                    //Debug.Log(bestLocation);
                    return bestLocation;
                }
            }
        }
        //PrintCollisionMap();
        //Debug.Assert(bestLocation == new Vector3(finish.Item1, 0, finish.Item2),"Target Position not reached");
        //Debug.Log(bestLocation);

        //ho raggiunto sia la x che la y desiderate-> restituisco l'ultima posizione valida che e' anche la posizione finale richiesta
        return bestLocation;
    }
        //controlla se Chip � vicino ad un pin
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

    public static void setCollisionMap(float x,float y, CollisionEntity ce)
    {
        var MapConvertionX = ((-x) + 400) / 100;
        var MapConvertionY = (y + 500) / 100;
        collisionMap[(int)Math.Round(MapConvertionY, 0), (int)Math.Round(MapConvertionX, 0)] = ce;
    }
    public static CollisionEntity getCollisionMap(float x, float y)
    {
        var MapConvertionX = ((-x) + 400) / 100;
        var MapConvertionY = (y + 500) / 100;
        return collisionMap[(int)Math.Round(MapConvertionY, 0), (int)Math.Round(MapConvertionX, 0)];
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
                if(collisionMap[i, j].collidesFromAbove)
                    line = line + "[A]";
                else if (collisionMap[i, j].collidesFromLeft)
                {
                    line = line + "[B]";
                }
                else
                    line = line + "[C]";
            }
            matrix = matrix + line + '\n';
        }
        Debug.Log(matrix);
    }

    public static int GetLevelNumber(string sceneName, string result)
    {
        char c = sceneName.Last();
        var sceneNameUpdated = sceneName.Remove(sceneName.Length-1);

        if (Char.IsDigit(c))
        {
            result = result + c;

            return GetLevelNumber(sceneNameUpdated, result);
        }
        else
        {
            return int.Parse(result);
        }
    }
    public static bool isCollisionOnPointHole(Vector3 point)
    {
        //Debug.Log("Hole Check");
        var MapConvertionX = (int)Math.Round(((-point.x) + 400) / 100, 0);
        var MapConvertionY = (int)Math.Round(((point.z) + 500) / 100, 0);

        foreach(var hole in Holes)
        {
            if (collisionMap[MapConvertionY, MapConvertionX] == hole)
                return true;
        }
        return false;
    }
}
