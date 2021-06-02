using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pin
{
    public GameObject Instance { get; set; }
    public PinType Type { get; set; }
    public bool IsConnected { get; set; }
}

public class Cable
{
    public GameObject Instance { get; set; }
    public bool IsConnectedToCip { get; set; }

    public List<Tuple<float, float>> WirePositions = new List<Tuple<float, float>>();

    public void AddPosition(Tuple<float, float> position)
    {
        if (!WirePositions.Contains(position))
        {
            WirePositions.Add(position);
        }
    }
}
