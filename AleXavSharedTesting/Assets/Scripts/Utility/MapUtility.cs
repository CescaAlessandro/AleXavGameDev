using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class MapUtility 
{
    public static bool IsCipWiring { get; set; } = false;

    public static void SetWiring(bool flag)
    {
        IsCipWiring = flag;
    }

    //public static void CheckCollision(List<Tuple<float, float>> positionsFirstCable, List<Tuple<float, float>> positionsSecondCable)
    //{
    //    int counter = 0;

    //    foreach(var position in positionsFirstCable)
    //    {
    //        if (positionsSecondCable.Contains(position))
    //            counter++;
    //    }

    //    if (counter > 0)
    //        Debug.Log("Collisione rilevata.");
    //}

    public static bool SharesAnyValueWith<T>(this IEnumerable<T> a, IEnumerable<T> b)
    {
        return a.Intersect(b).Any();
    }
}
