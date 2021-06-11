using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pin
{
    public GameObject Instance { get; set; }
    public PinType Type { get; set; }
    public bool IsConnected { get; set; }
    public Cable CableConnected { get; set; }
    public int Index { get; set; }
    public Tuple<Vector3,Quaternion> FluxSpawnPoint { get; set; }
    public Tuple<Vector3,Quaternion> AttachmentPoint { get; set; }
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

public class Dialog
{
    public Dialog(string text, bool flag)
    {
        this.Text = text;
        this.AlreadyShowed = flag;
    }
    public string Text { get; set; }

    public bool AlreadyShowed { get; set; } = false;
}

public class CollisionEntity
{
    public bool collidesFromAbove;
    public bool collidesFromBelow;
    public bool collidesFromLeft;
    public bool collidesFromRight;

    public static CollisionEntity getNoCollisionEntity()
    {
        var ent = new CollisionEntity();
        ent.collidesFromAbove = false;
        ent.collidesFromBelow = false;
        ent.collidesFromLeft = false;
        ent.collidesFromRight = false;
        return ent;
    }
    public static CollisionEntity getFullCollisionEntity()
    {
        var ent = new CollisionEntity();
        ent.collidesFromAbove = true;
        ent.collidesFromBelow = true;
        ent.collidesFromLeft = true;
        ent.collidesFromRight = true;
        return ent;
    }
    public static CollisionEntity getVerticalCollisionEntity()
    {
        var ent = new CollisionEntity();
        ent.collidesFromAbove = true;
        ent.collidesFromBelow = true;
        ent.collidesFromLeft = false;
        ent.collidesFromRight = false;
        return ent;
    }
    public static CollisionEntity getHorizontalCollisionEntity()
    {
        var ent = new CollisionEntity();
        ent.collidesFromAbove = false;
        ent.collidesFromBelow = false;
        ent.collidesFromLeft = true;
        ent.collidesFromRight = true;
        return ent;
    }
    public bool canBeAccessedFromDirection(directions dir)
    {
        switch (dir)
        {
            case directions.Top:
                return !collidesFromAbove;
            case directions.Bottom:
                return !collidesFromBelow;
            case directions.Left:
                return !collidesFromLeft;
            case directions.Right:
                return !collidesFromRight;
            default:
                return true;
        }

    }
    public virtual void PassingThrough(directions dir)
    {
        collidesFromAbove = true;
        collidesFromBelow = true;
        collidesFromLeft = true;
        collidesFromRight = true;
    }
    public virtual void Exiting(directions dir)
    {
        collidesFromAbove = false;
        collidesFromBelow = false;
        collidesFromLeft = false;
        collidesFromRight = false;
    }
}
public class Bridge : CollisionEntity
{
    public Bridge()
    {
        collidesFromAbove = false;
        collidesFromBelow = false;
        collidesFromLeft = false;
        collidesFromRight = false;
    }
    public override void PassingThrough(directions dir)
    {
        switch (dir)
        {
            case directions.Top:
                collidesFromAbove = true;
                collidesFromBelow = true;
                break;
            case directions.Bottom:
                collidesFromAbove = true;
                collidesFromBelow = true;
                break;
            case directions.Left:
                collidesFromLeft = true;
                collidesFromRight = true;
                break;
            case directions.Right:
                collidesFromLeft = true;
                collidesFromRight = true;
                break;
            default:
                break;
        }
    }
    public override void Exiting(directions dir)
    {
        switch (dir)
        {
            case directions.Top:
                collidesFromAbove = false;
                collidesFromBelow = false;
                break;
            case directions.Bottom:
                collidesFromAbove = false;
                collidesFromBelow = false;
                break;
            case directions.Left:
                collidesFromLeft = false;
                collidesFromRight = false;
                break;
            case directions.Right:
                collidesFromLeft = false;
                collidesFromRight = false;
                break;
            default:
                break;
        }
    }
    public Vector3 PassingThroughModifications(directions dir)
    {
        return new Vector3(0, 100, 0);
    }
}