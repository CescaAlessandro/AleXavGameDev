using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DudeBehaviour : MonoBehaviour
{
    private string sceneName;
    private SpriteRenderer spriteRenderer;
    private TextMeshProUGUI textMesh;
    private float nextDialogTimer;
    private int dialogFlowState;
    private bool mistakeDone;
    
    //for 4th tutorial
    private bool usingBridge;
    private Flux f1;
    private Flux f2;
    private bool colorTipGiven = false;
    private bool bridgeTipGiven = false;
    
    //for 5th tutorial
    private bool usingHole;
    private bool colorCommented = false;
    private bool NearHoleCommented = false;
    private bool holeUsed = false;
    private Pin pinToConnect;
    
    //Dude sprites
    public Sprite dudeHappy;
    public Sprite dudeFine;
    public Sprite dudeWorried;
    public Sprite dudePissed;
    public Sprite dudeWow;
    public Sprite dudeWink;
    // Start is called before the first frame update
    void Start()
    {
        sceneName = SceneManager.GetActiveScene().name;
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        textMesh = this.transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        nextDialogTimer = 0;
        dialogFlowState = 0;
        mistakeDone = false;

        if (sceneName.Equals("Level 1"))
        {
            textMesh.text = DialogsUtility.dialogs[DialogInstance.Presentation];
            spriteRenderer.sprite = dudeHappy;
        }
        else if (sceneName.Equals("Level 2"))
        {
            textMesh.text = DialogsUtility.dialogs[DialogInstance.WelcomeAgain];
            spriteRenderer.sprite = dudeHappy;
        }
        else if (sceneName.Equals("Level 3"))
        {
            textMesh.text = DialogsUtility.dialogs[DialogInstance.WelcomeThird];
            spriteRenderer.sprite = dudeHappy;
        }
        else if (sceneName.Equals("Level 5"))
        {
            textMesh.text = DialogsUtility.dialogs[DialogInstance.ExplanationBridgePartOne];
            spriteRenderer.sprite = dudeHappy;
        }
        else if (sceneName.Equals("Level 7"))
        {
            textMesh.text = DialogsUtility.dialogs[DialogInstance.ExplanationHolePartOne];
            spriteRenderer.sprite = dudeHappy;
        }
        else
        {
            textMesh.text = DialogsUtility.dialogs[DialogInstance.BeautifulDay];
            spriteRenderer.sprite = dudeFine;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!MapUtility.GamePaused)
        {
            if (sceneName.Equals("Level 1"))
            {
                FirstTutorialBehavoiur();
            }
            else if (sceneName.Equals("Level 2"))
            {
                SecondTutorialBehavoiur();
            }
            else if (sceneName.Equals("Level 3"))
            {
                ThirdTutorialBehavoiur();
            }
            else if(sceneName.Equals("Level 5"))
            {
                FourthTutorialBehavoiur();
            }
            else if (sceneName.Equals("Level 7"))
            {
                FifthTutorialBehavoiur();
            }
            else
            {
                StandardBehavoiur();
            }
        } 
    }

    public void FirstTutorialBehavoiur()
    {
        nextDialogTimer += Time.deltaTime;

        var upperPin = MapUtility.UpperPins.First();
        var lowerPin = MapUtility.LowerPins.First();

        //se stacchi il cavo, fai arrabbiare Dude
        if (!upperPin.IsConnected && !lowerPin.IsConnected &&
            (dialogFlowState == 3 || dialogFlowState == 4))
        {
            nextDialogTimer = 0;
            dialogFlowState = 5;
            mistakeDone = true;

            spriteRenderer.sprite = dudePissed;
            textMesh.text = DialogsUtility.dialogs[DialogInstance.Pissed];
        }

        //Dude ti da la possibilit� di rimediare (l'hai gi� fatto arrabbiare almeno una volta)
        if (nextDialogTimer >= 8 && !upperPin.IsConnected && !lowerPin.IsConnected &&
            dialogFlowState == 5)
        {
            spriteRenderer.sprite = dudePissed;
            textMesh.text = DialogsUtility.dialogs[DialogInstance.RedeemYourself];
            dialogFlowState = 6;
        }

        //primo dialogo
        if (nextDialogTimer > 8 && !upperPin.IsConnected &&
            dialogFlowState == 0)
        {
            nextDialogTimer = 0;
            dialogFlowState = 1;
            spriteRenderer.sprite = dudeFine;
            textMesh.text = DialogsUtility.dialogs[DialogInstance.Welcome];
        }

        //secondo dialogo
        if (nextDialogTimer > 8 && !upperPin.IsConnected &&
            dialogFlowState == 1)
        {
            nextDialogTimer = 0;
            dialogFlowState = 2;
            spriteRenderer.sprite = dudeFine;
            textMesh.text = DialogsUtility.dialogs[DialogInstance.GoToUpperPin];
        }

        //se primi dialoghi skippati... parte 1
        if (nextDialogTimer <= 8 && upperPin.IsConnected && !lowerPin.IsConnected &&
            (dialogFlowState == 1 || dialogFlowState == 0))
        {
            nextDialogTimer = 0;
            dialogFlowState = 4;

            spriteRenderer.sprite = dudeWow;
            textMesh.text = DialogsUtility.dialogs[DialogInstance.GoToLowerPinSavagePartOne];
        }

        //se non hai gi� fatto arrabbiare Dude una volta
        if (!mistakeDone)
            //se primi dialoghi skippati... parte 2
            if (nextDialogTimer >= 8 && upperPin.IsConnected && !lowerPin.IsConnected &&
                dialogFlowState == 4)
            {
                dialogFlowState = 3;
                spriteRenderer.sprite = dudeFine;
                textMesh.text = DialogsUtility.dialogs[DialogInstance.GoToLowerPinSavagePartTwo];
            }

        //altrimenti, prosegui normale
        if (upperPin.IsConnected && !lowerPin.IsConnected &&
            (dialogFlowState == 2 || dialogFlowState == 6 || dialogFlowState == 5))
        {
            dialogFlowState = 3;
            spriteRenderer.sprite = dudeFine;
            textMesh.text = DialogsUtility.dialogs[DialogInstance.GoToLowerPin];
        }

        //goal
        if (upperPin.IsConnected && lowerPin.IsConnected &&
            dialogFlowState == 3)
        {
            spriteRenderer.sprite = dudeHappy;
            textMesh.text = DialogsUtility.dialogs[DialogInstance.GoalReached];
            dialogFlowState = -1;

            MapUtility.GamePaused = true;
            MenuManager.Instance().LoadLevelCompleteMenu();
        }

        //special goal
        if (upperPin.IsConnected && lowerPin.IsConnected &&
            dialogFlowState == 4)
        {
            spriteRenderer.sprite = dudeWink;
            textMesh.text = DialogsUtility.dialogs[DialogInstance.SkippedTutorial];
            dialogFlowState = -1;

            MapUtility.GamePaused = true;
            MenuManager.Instance().LoadLevelCompleteMenu();
        }
    }

    public void SecondTutorialBehavoiur()
    {
        nextDialogTimer += Time.deltaTime;

        var blueUpperPin = MapUtility.UpperPins.ElementAt(0);
        var pinkUpperPin = MapUtility.UpperPins.ElementAt(1);
        var lowerPin = MapUtility.LowerPins.First();

        //primo dialogo
        if (nextDialogTimer >= 8 && !blueUpperPin.IsConnected && !pinkUpperPin.IsConnected &&
            dialogFlowState == 0)
        {
            dialogFlowState = 1;
            nextDialogTimer = 0;
            spriteRenderer.sprite = dudeFine;
            textMesh.text = DialogsUtility.dialogs[DialogInstance.CanDetach];
        }

        //speciale: salta spiegazione
        if ((blueUpperPin.IsConnected || pinkUpperPin.IsConnected) &&
            (dialogFlowState == 0 || dialogFlowState == 1))
        {
            dialogFlowState = 11;
            
            spriteRenderer.sprite = dudeWow;
            textMesh.text = DialogsUtility.dialogs[DialogInstance.NoTimeToLose];
        }

        //secondo dialogo
        if (nextDialogTimer > 8 && !blueUpperPin.IsConnected && !pinkUpperPin.IsConnected &&
            dialogFlowState == 1)
        {
            dialogFlowState = 2;
            nextDialogTimer = 0;
            spriteRenderer.sprite = dudeFine;
            textMesh.text = DialogsUtility.dialogs[DialogInstance.CreateBlue];
        }

        //speciale: hai scelto di collegare il rosa anche se Dude ti ha suggerito blu
        if (!blueUpperPin.IsConnected && pinkUpperPin.IsConnected &&
            (dialogFlowState == 2 || dialogFlowState == 8))
        {
            dialogFlowState = 7;
            mistakeDone = true;
            spriteRenderer.sprite = dudePissed;
            textMesh.text = DialogsUtility.dialogs[DialogInstance.ISaidBlue];
        }

        //speciale: Dude prende in giro la tua lentezza
        if (!blueUpperPin.IsConnected && !pinkUpperPin.IsConnected &&
            (dialogFlowState == 7 || dialogFlowState == 3 || dialogFlowState == 11 || dialogFlowState == 9))
        {
            dialogFlowState = 8;
            //nextDialogTimer = 0;
            spriteRenderer.sprite = dudeWorried;
            textMesh.text = DialogsUtility.dialogs[DialogInstance.LongDay];
        }

        //speciale: se fai quello che dice Dude, ti perdona
        if (blueUpperPin.IsConnected && !pinkUpperPin.IsConnected &&
            dialogFlowState == 8)
        {
            dialogFlowState = 9;
            nextDialogTimer = 0;
            mistakeDone = false;
            spriteRenderer.sprite = dudeFine;
            textMesh.text = DialogsUtility.dialogs[DialogInstance.OkBlue];
        }

        //terzo dialogo
        if (blueUpperPin.IsConnected && !pinkUpperPin.IsConnected &&
            (dialogFlowState == 2 || (dialogFlowState == 9 && nextDialogTimer >=5)))
        {
            dialogFlowState = 3;

            spriteRenderer.sprite = dudeFine;
            textMesh.text = DialogsUtility.dialogs[DialogInstance.Attach];
        }

        //quarto dialogo
        if (lowerPin.IsConnected &&
            (dialogFlowState == 3 || dialogFlowState == 7 || dialogFlowState == 11 || dialogFlowState == 9))
        {
            dialogFlowState = 4;

            spriteRenderer.sprite = dudeHappy;
            textMesh.text = DialogsUtility.dialogs[DialogInstance.Detach];
        }

        //quinto dialogo
        if (!lowerPin.IsConnected &&
            (dialogFlowState == 4 || dialogFlowState == 10))
        {
            dialogFlowState = 5;

            spriteRenderer.sprite = dudeWink;
            textMesh.text = DialogsUtility.dialogs[DialogInstance.Remove];
        }

        //speciale: hai attaccato il cavo anche se Dude ti ha chiesto di rimuoverlo
        if (lowerPin.IsConnected &&
            dialogFlowState == 5)
        {
            dialogFlowState = 10;
            mistakeDone = true;

            spriteRenderer.sprite = dudePissed;
            textMesh.text = DialogsUtility.dialogs[DialogInstance.BadTime];
        }

        //speciale: Dude ha perso la pazienza
        if (!lowerPin.IsConnected && !blueUpperPin.IsConnected && !pinkUpperPin.IsConnected &&
            dialogFlowState == 5 && mistakeDone)
        {
            dialogFlowState = 10;

            spriteRenderer.sprite = dudePissed;
            textMesh.text = DialogsUtility.dialogs[DialogInstance.WasteTime];
        }

        //sesto dialogo
        if (!blueUpperPin.IsConnected && !pinkUpperPin.IsConnected && !mistakeDone &&
            dialogFlowState == 5)
        {
            dialogFlowState = 6;
            
            spriteRenderer.sprite = dudeFine;
            textMesh.text = DialogsUtility.dialogs[DialogInstance.RepeatForPink];
        }

        //speciale: finch� non fai quello che dice Dude, non si va avanti
        if (blueUpperPin.IsConnected &&
            dialogFlowState == 6)
        {
            dialogFlowState = 12;
            
            spriteRenderer.sprite = dudePissed;
            textMesh.text = DialogsUtility.dialogs[DialogInstance.IAmRuler];
        }

        //sesto dialogo
        if (!blueUpperPin.IsConnected && pinkUpperPin.IsConnected && lowerPin.IsConnected &&
            (dialogFlowState == 6 || dialogFlowState == 12))
        {
            spriteRenderer.sprite = dudeHappy;
            textMesh.text = DialogsUtility.dialogs[DialogInstance.GoalReached];
            dialogFlowState = -1;

            MapUtility.GamePaused = true;
            MenuManager.Instance().LoadLevelCompleteMenu();
        }
    }

    public void ThirdTutorialBehavoiur()
    {
        nextDialogTimer += Time.deltaTime;

        var blueUpperPin = MapUtility.UpperPins.ElementAt(0);
        var pinkUpperPin = MapUtility.UpperPins.ElementAt(1);
        var blueLowerPin = MapUtility.LowerPins.ElementAt(1);
        var pinkLowerPin = MapUtility.LowerPins.ElementAt(0);

        if (dialogFlowState == 0 && nextDialogTimer >= 8)
        {
            dialogFlowState = 1;
            nextDialogTimer = 0;
            spriteRenderer.sprite = dudeFine;
            textMesh.text = DialogsUtility.dialogs[DialogInstance.ExplanationPartOne];
        }
        if(dialogFlowState == 1 && nextDialogTimer >= 8)
        {
            dialogFlowState = 2;
            nextDialogTimer = 0;
            spriteRenderer.sprite = dudeFine;
            textMesh.text = DialogsUtility.dialogs[DialogInstance.ExplanationPartTwo];
        }
        if(dialogFlowState == 2 && nextDialogTimer >= 8)
        {
            dialogFlowState = 3;
            nextDialogTimer = 0;
            spriteRenderer.sprite = dudeFine;
            textMesh.text = DialogsUtility.dialogs[DialogInstance.ExplanationPartThree];
        }
        if(dialogFlowState == 3 && nextDialogTimer >= 8)
        {
            dialogFlowState = 4;
            nextDialogTimer = 0;
            spriteRenderer.sprite = dudeWorried;
            textMesh.text = DialogsUtility.dialogs[DialogInstance.ExplanationPartFour];
        }
        if(dialogFlowState == 4 && nextDialogTimer >= 8)
        {
            dialogFlowState = 5;
            nextDialogTimer = 0;
            spriteRenderer.sprite = dudeFine;
            textMesh.text = DialogsUtility.dialogs[DialogInstance.ExplanationPartFive];

            AudioManager.Instance().PlayLoopZap();
        }
        if(dialogFlowState == 5 && nextDialogTimer >= 8)
        {
            AudioManager.Instance().StopZap();

            dialogFlowState = 6;
            nextDialogTimer = 0;
            spriteRenderer.sprite = dudeWorried;
            textMesh.text = DialogsUtility.dialogs[DialogInstance.ExplanationPartSix];
        }
        if(dialogFlowState == 6 && nextDialogTimer >= 8)
        {
            dialogFlowState = 7;
            nextDialogTimer = 0;
            spriteRenderer.sprite = dudeFine;
            textMesh.text = DialogsUtility.dialogs[DialogInstance.ExplanationPartSeven];
        }
        if(dialogFlowState == 7 && nextDialogTimer >= 8)
        {
            dialogFlowState = 8;
            nextDialogTimer = 0;
            spriteRenderer.sprite = dudeWorried;
            textMesh.text = DialogsUtility.dialogs[DialogInstance.ExplanationPartEight];
        }
        if(dialogFlowState == 8 && nextDialogTimer >= 8)
        {
            GameManager.Instance().SpawnFluxIndex(0);

            dialogFlowState = 9;
            nextDialogTimer = 0;
 
            spriteRenderer.sprite = dudeWow;
            textMesh.text = DialogsUtility.dialogs[DialogInstance.HereItIs];
        }
        if(dialogFlowState == 9 && nextDialogTimer >= 6)
        {
            dialogFlowState = 10;
            nextDialogTimer = 0;

            spriteRenderer.sprite = dudeWink;
            textMesh.text = DialogsUtility.dialogs[DialogInstance.NotScared];
        }
        if((dialogFlowState == 10) && nextDialogTimer >= 6 && (!blueUpperPin.IsConnected || !blueLowerPin.IsConnected))
        {
            dialogFlowState = 11;

            spriteRenderer.sprite = dudeHappy;
            textMesh.text = DialogsUtility.dialogs[DialogInstance.WhatAreYouWaitingFor];
        }
        if((dialogFlowState == 11 || dialogFlowState == 10) && blueUpperPin.IsConnected && blueLowerPin.IsConnected)
        {
            dialogFlowState = 14;
        }
        if((dialogFlowState == 14 || dialogFlowState == 13) && GameManager.Instance().GetNumberFluxesDepleteded() == 1)
        {
            GameManager.Instance().SpawnFluxIndex(0);
            GameManager.Instance().SpawnFluxIndex(1);

            dialogFlowState = 12;
            nextDialogTimer = 0;

            spriteRenderer.sprite = dudeHappy;
            textMesh.text = DialogsUtility.dialogs[DialogInstance.DailyGoal];
        }
        if((dialogFlowState == 14) && (!blueUpperPin.IsConnected || !blueLowerPin.IsConnected))
        {
            dialogFlowState = 13;

            spriteRenderer.sprite = dudePissed;
            textMesh.text = DialogsUtility.dialogs[DialogInstance.KindaSus];
        }
        if(dialogFlowState == 12 && nextDialogTimer >= 8)
        {
            dialogFlowState = 15;
            nextDialogTimer = 0;

            spriteRenderer.sprite = dudeWink;
            textMesh.text = DialogsUtility.dialogs[DialogInstance.ByTheWay];
        }
        if(dialogFlowState == 15 && nextDialogTimer >= 50)
        {
            dialogFlowState = 13;
            nextDialogTimer = 0;

            spriteRenderer.sprite = dudePissed;
            textMesh.text = DialogsUtility.dialogs[DialogInstance.ALotTime];
        }

        if(GameManager.Instance().GetNumberFluxesDepleteded() == 3 && dialogFlowState != -1)
        {
            spriteRenderer.sprite = dudeHappy;
            textMesh.text = DialogsUtility.dialogs[DialogInstance.GoalReached];

            MapUtility.GamePaused = true;
            MenuManager.Instance().LoadLevelCompleteMenu();

            dialogFlowState = -1;
        }
    }

    public void FourthTutorialBehavoiur()
    {
        //Debug.Log(dialogFlowState);
        nextDialogTimer += Time.deltaTime;

        var yellowUpperPin = MapUtility.UpperPins.ElementAt(0);
        var pinkUpperPin = MapUtility.UpperPins.ElementAt(1);
        var redUpperPin = MapUtility.UpperPins.ElementAt(2);
        var yellowLowerPin = MapUtility.LowerPins.ElementAt(2);
        var pinkLowerPin = MapUtility.LowerPins.ElementAt(0);
        var redLowerPin = MapUtility.LowerPins.ElementAt(1);
        var bridge = MapUtility.Bridges.ElementAt(0);
        //primo dialogo
        if (nextDialogTimer >= 12 && dialogFlowState == 0)
        {
            dialogFlowState = 1;
            nextDialogTimer = 0;
            spriteRenderer.sprite = dudeFine;
            textMesh.text = DialogsUtility.dialogs[DialogInstance.ExplanationHolePartTwo];
        }
        if (dialogFlowState == 1 && nextDialogTimer >= 10)
        {
            f1 = GameManager.Instance().SpawnFluxIndex(0);
            f2 = GameManager.Instance().SpawnFluxIndex(1);
            dialogFlowState = 2;
            nextDialogTimer = 0;
            spriteRenderer.sprite = dudeFine;
            textMesh.text = DialogsUtility.dialogs[DialogInstance.TwoFluxesArriving];
        }
        if (dialogFlowState == 2 && nextDialogTimer >= 5 &&
            ((yellowLowerPin.IsConnected && yellowLowerPin.IsConnected)
            || (pinkLowerPin.IsConnected && pinkLowerPin.IsConnected)) && !bridge.isTraversed && !bridgeTipGiven)
        {
            bridgeTipGiven = true;
            dialogFlowState = 2;
            nextDialogTimer = 0;
            spriteRenderer.sprite = dudeWorried;
            textMesh.text = DialogsUtility.dialogs[DialogInstance.NotUsingBridge];
        }
        if (dialogFlowState == 2 && !colorTipGiven && nextDialogTimer >= 5 &&
            ((!cableColorsMatchOrNoConnection(yellowUpperPin, MapUtility.LowerPins))
               || (!cableColorsMatchOrNoConnection(pinkUpperPin, MapUtility.LowerPins))))
        {
            colorTipGiven = true;
            dialogFlowState = 3;
            nextDialogTimer = 0;
            textMesh.text = DialogsUtility.dialogs[DialogInstance.WrongColorsConnectedPartOne];
        }
        if ((dialogFlowState == 2 || dialogFlowState == 3) && f1.hasArrived && f2.hasArrived && nextDialogTimer >= 2
            && (!yellowUpperPin.IsConnected || !yellowUpperPin.IsConnected || !pinkUpperPin.IsConnected || !pinkLowerPin.IsConnected
                || (!cableColorsMatchOrNoConnection(yellowUpperPin, MapUtility.LowerPins)) || (!cableColorsMatchOrNoConnection(yellowUpperPin, MapUtility.LowerPins))))
        {
            GameManager.Instance().DeleteFlux(f1);
            GameManager.Instance().DeleteFlux(f2);
            Destroy(f2.gameObject);
            f1 = GameManager.Instance().SpawnFluxIndex(0);
            f2 = GameManager.Instance().SpawnFluxIndex(1);
            spriteRenderer.sprite = dudePissed;
            textMesh.text = DialogsUtility.dialogs[DialogInstance.FluxesMissed];
            nextDialogTimer = 0;
        }
        if ((dialogFlowState == 2 || dialogFlowState == 3) && GameManager.Instance().GetNumberFluxesDepleteded() == 2)
        {
            dialogFlowState = 5;
            //Go on
        }
        if (dialogFlowState == 3 && nextDialogTimer >= 8)
        {
            dialogFlowState = 2;
            textMesh.text = DialogsUtility.dialogs[DialogInstance.WrongColorsConnectedPartTwo];
            nextDialogTimer = 0;
        }
        if (dialogFlowState == 5)
        {
            dialogFlowState = 6;
            spriteRenderer.sprite = dudeHappy;
            textMesh.text = DialogsUtility.dialogs[DialogInstance.BridgeUsedWellDone];
            nextDialogTimer = 0;
        }
        if (dialogFlowState == 6 && nextDialogTimer >= 10)
        {
            f1 = GameManager.Instance().SpawnFluxIndex(2);
            f2 = GameManager.Instance().SpawnFluxIndex(1);
            nextDialogTimer = 0;
            textMesh.text = DialogsUtility.dialogs[DialogInstance.TutorialThreeFluxesArriving];
            dialogFlowState = 7;
        }
        if (dialogFlowState == 7 && nextDialogTimer >= 15)
        {
            nextDialogTimer = 0;
            f1 = GameManager.Instance().SpawnFluxIndex(0);
            dialogFlowState = 8;
        }
        if (dialogFlowState == 8 && nextDialogTimer >= 18)
        {
            nextDialogTimer = 0;
            f1 = GameManager.Instance().SpawnFluxIndex(0);
            f1 = GameManager.Instance().SpawnFluxIndex(2);
            dialogFlowState = 9;
        }
        if(GameManager.Instance().GetNumberFluxesDepleteded() == 7)
        {
            spriteRenderer.sprite = dudeHappy;
            textMesh.text = DialogsUtility.dialogs[DialogInstance.GoalReached];

            MenuManager.Instance().LoadLevelCompleteMenu();

            dialogFlowState = -1;
        }
    }

    public void FifthTutorialBehavoiur()
    {
        nextDialogTimer += Time.deltaTime;

        var yellowUpperPin = MapUtility.UpperPins.ElementAt(0);
        var pinkUpperPin = MapUtility.UpperPins.ElementAt(1);
        var redUpperPin = MapUtility.UpperPins.ElementAt(2);
        var yellowLowerPin = MapUtility.LowerPins.ElementAt(2);
        var pinkLowerPin = MapUtility.LowerPins.ElementAt(0);
        var redLowerPin = MapUtility.LowerPins.ElementAt(1);
        var hole1 = MapUtility.Holes.ElementAt(0);
        var hole2 = MapUtility.Holes.ElementAt(1);

        if (nextDialogTimer >= 7 && dialogFlowState == 0)
        {
            pinToConnect = redUpperPin;
            dialogFlowState = 1;
            nextDialogTimer = 0;
            spriteRenderer.sprite = dudeFine;
            textMesh.text = DialogsUtility.dialogs[DialogInstance.ExplanationHolePartTwo];
        }

        if (nextDialogTimer >= 8 && dialogFlowState == 1)
        {
            dialogFlowState = 2;
            nextDialogTimer = 0;
            spriteRenderer.sprite = dudeFine;
            textMesh.text = DialogsUtility.dialogs[DialogInstance.ExplanationHolePartThree];
        }

        //wait for the first hole to be connected
        
        //case wrong hole
        if (nextDialogTimer >= 8 && dialogFlowState == 2 &&  hole2.IsConnected && !hole1.IsConnected)
        {
            pinToConnect = MapUtility.UpperPins.ElementAt(hole2.CableConnected.index);
            dialogFlowState = 3;
            nextDialogTimer = 0;
            spriteRenderer.sprite = dudeFine;
            textMesh.text = DialogsUtility.dialogs[DialogInstance.ExplanationHoleWrongHole];
        }
        
        //case right hole
        if (nextDialogTimer >= 8 && dialogFlowState == 2 && hole1.IsConnected)
        {
            pinToConnect = MapUtility.UpperPins.ElementAt(hole1.CableConnected.index);
            dialogFlowState = 3;
        }
        
        //case hole not used
        if (nextDialogTimer >= 8 && dialogFlowState == 2 && pinToConnect.IsConnected  
            && hole1.CableConnected == null && hole2.CableConnected == null 
            && MapUtility.LowerPins.First(pin => pin.Instance.GetComponent<Renderer>().material.color == pinToConnect.Instance.GetComponent<Renderer>().material.color).IsConnected)
        {
            nextDialogTimer = 0;
            spriteRenderer.sprite = dudeWorried;
            textMesh.text = DialogsUtility.dialogs[DialogInstance.ExplanationHoleNotUsed];
        }
        
        //different pin used possibility
        if(nextDialogTimer >= 4 && dialogFlowState == 3)
        {
            if (!(pinToConnect == redUpperPin))
            {
                spriteRenderer.sprite = dudeHappy;
                textMesh.text = DialogsUtility.dialogs[DialogInstance.ExplanationHoleDifferentColor];
                nextDialogTimer = 0;
            }
            dialogFlowState = 4;
        }

        //both holes connected
        if (nextDialogTimer >= 8 && dialogFlowState == 4 && 
            (hole1.IsConnected || hole2.IsConnected))
        {
            dialogFlowState = 5;
            nextDialogTimer = 0;
            spriteRenderer.sprite = dudeFine;
            textMesh.text = DialogsUtility.dialogs[DialogInstance.ExplanationHolePartFour];
        }
        
        try
        {
            //Vedo se il pin dello stesso colore di quello scelto dall'utente è a sua volta collegato con il cavo
            Pin lowerPinToConnect = MapUtility.LowerPins.First(pin => pin.Instance.GetComponent<Renderer>().material.color == pinToConnect.Instance.GetComponent<Renderer>().material.color);

            if (hole1.IsConnected && hole2.IsConnected)
            {
                if (nextDialogTimer >= 3 && dialogFlowState == 5
                    && hole1.CableConnected.index == pinToConnect.CableConnected.index
                    && hole2.CableConnected.index == pinToConnect.CableConnected.index)
                {
                    dialogFlowState = 6;
                    nextDialogTimer = 0;
                    spriteRenderer.sprite = dudeFine;
                    textMesh.text = DialogsUtility.dialogs[DialogInstance.ExplanationHolePartFive];
                }
            }

            if (hole1.IsConnected && hole2.IsConnected )
            {
                //se i due pin e i due buchi sono collegati tutti con cavi aventi lo stesso indice
                if (nextDialogTimer >= 3 && dialogFlowState == 6
                    && lowerPinToConnect.CableConnected.index == pinToConnect.CableConnected.index
                    && hole1.CableConnected.index == pinToConnect.CableConnected.index
                    && hole2.CableConnected.index == pinToConnect.CableConnected.index)
                {
                    dialogFlowState = 7;
                    nextDialogTimer = 0;
                    spriteRenderer.sprite = dudeFine;
                    textMesh.text = DialogsUtility.dialogs[DialogInstance.ExplanationHolePartSix];
                }
            }
            
            //i cavi vengono cambiati casualmente dall'utente nel mezzo del tutorial -> accetto i nuovi cavi/colori usati
            if(dialogFlowState >= 7)
            {
                if (hole1.IsConnected && hole2.IsConnected)
                {
                    if (hole1.CableConnected.index == hole2.CableConnected.index && hole1.CableConnected.index != pinToConnect.Index)
                    {
                        pinToConnect = MapUtility.UpperPins.ElementAt(hole1.CableConnected.index);
                        lowerPinToConnect = MapUtility.LowerPins.First(pin => pin.Instance.GetComponent<Renderer>().material.color == pinToConnect.Instance.GetComponent<Renderer>().material.color);
                    }
                }
            }

            if (nextDialogTimer >= 8 && dialogFlowState == 7)
            {
                dialogFlowState = 8;
                nextDialogTimer = 0;
                spriteRenderer.sprite = dudeFine;
                textMesh.text = DialogsUtility.dialogs[DialogInstance.ExplanationHolePartSeven];
            }

            if (nextDialogTimer >= 8 && dialogFlowState == 8)
            {
                dialogFlowState = 9;
                nextDialogTimer = 0;
                spriteRenderer.sprite = dudeFine;
                textMesh.text = DialogsUtility.dialogs[DialogInstance.ExplanationHolePartEight];
            }

            //attendo che il pin inferiore venga sconnesso
            if (nextDialogTimer >= 3 && dialogFlowState == 9 && !lowerPinToConnect.IsConnected)
            {
                dialogFlowState = 10;
                nextDialogTimer = 0;
                spriteRenderer.sprite = dudeFine;
                textMesh.text = DialogsUtility.dialogs[DialogInstance.ExplanationHolePartNine];
            }

            //attendo che il primo buco venga sconnesso
            if (nextDialogTimer >= 3 && dialogFlowState == 10 && !lowerPinToConnect.IsConnected && (!hole1.IsConnected || !hole2.IsConnected))
            {
                dialogFlowState = 11;
                nextDialogTimer = 0;
                spriteRenderer.sprite = dudeFine;
                textMesh.text = DialogsUtility.dialogs[DialogInstance.ExplanationHolePartTen];
            }

            //attendo che il secondo buco venga sconnesso
            if (nextDialogTimer >= 3 && dialogFlowState == 11 && !lowerPinToConnect.IsConnected && !hole1.IsConnected && !hole2.IsConnected)
            {
                dialogFlowState = 12;
                nextDialogTimer = 0;
                spriteRenderer.sprite = dudeFine;
                textMesh.text = DialogsUtility.dialogs[DialogInstance.ExplanationHolePartEleven];
            }

            //attendo che il pin superiore venga sconnesso
            if (nextDialogTimer >= 3 && dialogFlowState == 12 && !lowerPinToConnect.IsConnected && !hole1.IsConnected && !hole2.IsConnected && !pinToConnect.IsConnected)
            {
                dialogFlowState = 13;
                nextDialogTimer = 0;
                spriteRenderer.sprite = dudeFine;
                textMesh.text = DialogsUtility.dialogs[DialogInstance.ExplanationHolePartTwelve];
            }
        }
        catch (System.Exception)
        {
        }

        if (nextDialogTimer >= 6 && dialogFlowState == 13)
        {
            dialogFlowState = 14;
            nextDialogTimer = 0;
            spriteRenderer.sprite = dudeFine;
            textMesh.text = DialogsUtility.dialogs[DialogInstance.ExplanationHolePartThirteen];
        }

        if (nextDialogTimer >= 8 && dialogFlowState == 14)
        {
            dialogFlowState = 15;
            nextDialogTimer = 0;
            spriteRenderer.sprite = dudeFine;
            textMesh.text = DialogsUtility.dialogs[DialogInstance.ExplanationHolePartFourteen];
        }

        if (nextDialogTimer >= 8 && dialogFlowState == 15)
        {
            MenuManager.Instance().LoadLevelCompleteMenu();
            dialogFlowState = -1;
            nextDialogTimer = 0;
            spriteRenderer.sprite = dudeFine;
            textMesh.text = DialogsUtility.dialogs[DialogInstance.ExplanationHolePartFifteen];
        }
    }


    private bool cableColorsMatchOrNoConnection(Pin upperPin,List<Pin> lowerPins)
    {
        foreach (Pin lower in lowerPins)
        {
            if(lower.CableConnected != null)
            {
                if (lower.CableConnected == upperPin.CableConnected)
                {
                    if (lower.Instance.GetComponent<Renderer>().material.color == upperPin.Instance.GetComponent<Renderer>().material.color)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }

    public void StandardBehavoiur()
    {

    }

    public void GameOverBehaviour()
    {
        spriteRenderer.sprite = dudePissed;
        textMesh.text = DialogsUtility.dialogs[DialogInstance.DamnCable];

        MapUtility.GamePaused = true;
        MenuManager.Instance().LoadLevelFailedMenu();

        dialogFlowState = -1;
    }

    public void SetUsingBridge(bool value)
    {

    }
}
