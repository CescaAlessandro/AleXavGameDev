using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class MapUtility
{
    public static bool IsChipWiring { get; set; } = false;

    public static List<Pin> Pins { get; set; }
    public static List<Cable> Cables { get; set; }

    public static float range = 10;

    public static void SetWiring(bool flag)
    {
        IsChipWiring = flag;
    }

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

    //controlla se Chip è vicino ad un pin
    //se vero => resituisce tupla con posizione pin e true
    //se false => resituisce tupla con posizione fake e falso
    public static Tuple<bool, Pin> IsChipNearPin(Vector3 position)
    {
        if (Pins != null)
        {
            //N.B.: Pin child(0) = attachment
            var pinFound = Pins.FirstOrDefault(pinPosition => Vector3.Distance(pinPosition.Instance.transform.GetChild(0).position, position) < range);

            if (pinFound != null)
                return new Tuple<bool, Pin>(true, pinFound);
            return new Tuple<bool, Pin>(false, null);
        }
        return new Tuple<bool, Pin>(false, null);
    }
}
