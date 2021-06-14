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
        else
        {
            textMesh.text = DialogsUtility.dialogs[DialogInstance.BeautifulDay];
            spriteRenderer.sprite = dudeFine;
        }
    }

    // Update is called once per frame
    void Update()
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
        else
        {
            StandardBehavoiur();
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

        //Dude ti da la possibilità di rimediare (l'hai già fatto arrabbiare almeno una volta)
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

        //se non hai già fatto arrabbiare Dude una volta
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

            MenuManager.Instance().LoadLevelCompleteMenu();
        }

        //special goal
        if (upperPin.IsConnected && lowerPin.IsConnected &&
            dialogFlowState == 4)
        {
            spriteRenderer.sprite = dudeWink;
            textMesh.text = DialogsUtility.dialogs[DialogInstance.SkippedTutorial];
            dialogFlowState = -1;

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

        //speciale: finchè non fai quello che dice Dude, non si va avanti
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

        if(GameManager.Instance().GetNumberFluxesDepleteded() == 3)
        {
            spriteRenderer.sprite = dudeHappy;
            textMesh.text = DialogsUtility.dialogs[DialogInstance.GoalReached];

            MenuManager.Instance().LoadLevelCompleteMenu();

            dialogFlowState = -1;
        }
    }

    public void StandardBehavoiur()
    {

    }

    public void GameOverBehaviour()
    {
        spriteRenderer.sprite = dudePissed;
        textMesh.text = DialogsUtility.dialogs[DialogInstance.DamnCable];

        MenuManager.Instance().LoadLevelFailedMenu();

        dialogFlowState = -1;
    }
}
