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
    public int index { get; set; }
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

//classe root delle collisioni sulla board,
public class CollisionEntity
{
    //questi bool indicano se si puo' entrare in questa casella da una specifica direzione
    public bool collidesFromAbove;
    public bool collidesFromBelow;
    public bool collidesFromLeft;
    public bool collidesFromRight;
    //questi bool indicano se si puo' uscire dalla cella verso una specifica direzione
    public bool canBeExitedAbove;
    public bool canBeExitedBelow;
    public bool canBeExitedLeft;
    public bool canBeExitedRight;

    //Costruttori per tipi standard di collisioni
    public static CollisionEntity getNoCollisionEntity()
    {
        var ent = new CollisionEntity();
        ent.collidesFromAbove = false;
        ent.collidesFromBelow = false;
        ent.collidesFromLeft = false;
        ent.collidesFromRight = false;
        ent.canBeExitedAbove = true;
        ent.canBeExitedBelow = true;
        ent.canBeExitedLeft = true;
        ent.canBeExitedRight = true;
        return ent;
    }
    public static CollisionEntity getFullCollisionEntity()
    {
        var ent = new CollisionEntity();
        ent.collidesFromAbove = true;
        ent.collidesFromBelow = true;
        ent.collidesFromLeft = true;
        ent.collidesFromRight = true;
        ent.canBeExitedAbove = true;
        ent.canBeExitedBelow = true;
        ent.canBeExitedLeft = true;
        ent.canBeExitedRight = true;
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

    //modifica i bool delle collisioni quando Chip entra in questa casella da una specifica direzione
    public virtual void PassingThrough(directions dir)
    {
        //per le collsioni base la direzione e' indifferente
        collidesFromAbove = true;
        collidesFromBelow = true;
        collidesFromLeft = true;
        collidesFromRight = true;
    }
    
    //modifica i bool delle collisioni quando Chip esce da questa casella verso una direzione
    public virtual void Exiting(directions dir)
    {
        //per le collsioni base la direzione e' indifferente
        collidesFromAbove = false;
        collidesFromBelow = false;
        collidesFromLeft = false;
        collidesFromRight = false;
    }

    //restituisce un'eventuale modifica alla posizione di chip quando entra in questa casella
    public virtual Vector3 PassingThroughModifications(directions dir)
    {
        //per le collsioni base non vi e' modifica
        return new Vector3(0, 0, 0);
    }
}
public class Bridge : CollisionEntity
{
    public bool isTraversedVertical { get; set; }
    public bool isTraversedHoriz { get; set; }
    public Bridge()
    {
        isTraversedVertical = false;
        isTraversedHoriz = false;
        collidesFromAbove = false;
        collidesFromBelow = false;
        collidesFromLeft = false;
        collidesFromRight = false;
    }

    //
    public override void PassingThrough(directions dir)
    {
        switch (dir)
        {
            //Se Chip entra da sopra o sotto, puo' comunque entrare lateralmente ma non puo' uscire lateralmente ("scendere dal ponte non dalle scale")
            case directions.Top:
                isTraversedVertical = true;
                collidesFromAbove = true;
                collidesFromBelow = true;
                canBeExitedAbove = true;
                canBeExitedBelow = true;
                canBeExitedLeft = false;
                canBeExitedRight = false;
                break;
            case directions.Bottom:
                isTraversedVertical = true;
                collidesFromAbove = true;
                collidesFromBelow = true;
                canBeExitedAbove = true;
                canBeExitedBelow = true;
                canBeExitedLeft = false;
                canBeExitedRight = false;
                break;
            //Se Chip entra da destra o sinistra, puo' comunque entrare verticalmente ma non puo' uscire verticalmente ("attraversare le la parte delle scale da sotto")
            case directions.Left:
                isTraversedHoriz = true;
                collidesFromLeft = true;
                collidesFromRight = true;
                canBeExitedAbove = false;
                canBeExitedBelow = false;
                canBeExitedLeft = true;
                canBeExitedRight = true;
                break;
            case directions.Right:
                isTraversedHoriz = true;
                collidesFromLeft = true;
                collidesFromRight = true;
                canBeExitedAbove = false;
                canBeExitedBelow = false;
                canBeExitedLeft = true;
                canBeExitedRight = true;
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
                isTraversedVertical = false;
                collidesFromAbove = false;
                collidesFromBelow = false;
                break;
            case directions.Bottom:
                isTraversedVertical = false;
                collidesFromAbove = false;
                collidesFromBelow = false;
                break;
            case directions.Left:
                isTraversedHoriz = false;
                collidesFromLeft = false;
                collidesFromRight = false;
                break;
            case directions.Right:
                isTraversedHoriz = false;
                collidesFromLeft = false;
                collidesFromRight = false;
                break;
            default:
                break;
        }
    }

    //se Chip entra nel ponte verticalemente allora deve sollevarsi per passare sopra altrimenti non fare niente per passare sotto
    public override Vector3 PassingThroughModifications(directions dir)
    {
        if (dir == directions.Bottom || dir == directions.Top)
        {
            return new Vector3(0, 100, 0);
        }
        else
        {
            return new Vector3(0, 0, 0);
        }
    }
}

public class Hole : CollisionEntity
{
    public GameObject Instance { get; set; }
    public bool IsConnected { get; set; }
    public Cable CableConnected { get; set; }

    //Le collisioni dei buchi soni come quelle di un full colision base ma non è possibile uscirne solo verso la direzione da cui si entra 
    //(perche' quando si annulla la last move le collisioni sono ignorate)
    public Hole()
    {
        collidesFromAbove = false;
        collidesFromBelow = false;
        collidesFromLeft = false;
        collidesFromRight = false;
        canBeExitedAbove = true;
        canBeExitedBelow = true;
        canBeExitedLeft = true;
        canBeExitedRight = true;
    }

    public override void PassingThrough(directions dir)
    {
        collidesFromAbove = true;
        collidesFromBelow = true;
        collidesFromLeft = true;
        collidesFromRight = true;
        canBeExitedAbove = false;
        canBeExitedBelow = false;
        canBeExitedLeft = false;
        canBeExitedRight = false;
    }
    public override void Exiting(directions dir)
    {
        collidesFromAbove = false;
        collidesFromBelow = false;
        collidesFromLeft = false;
        collidesFromRight = false;
        canBeExitedAbove = true;
        canBeExitedBelow = true;
        canBeExitedLeft = true;
        canBeExitedRight = true;
    }
    public void cableCreatedOnHole()
    {
        collidesFromAbove = true;
        collidesFromBelow = true;
        collidesFromLeft = true;
        collidesFromRight = true;
        canBeExitedAbove = true;
        canBeExitedBelow = true;
        canBeExitedLeft = true;
        canBeExitedRight = true;
    }
}