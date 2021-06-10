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
            textMesh.text = DialogsUtility.dialogs[DialogInstance.Welcome];
        }
        else if (sceneName.Equals("Level 2"))
        {
            textMesh.text = DialogsUtility.dialogs[DialogInstance.WelcomeAgain];
        }
        else
        {

        }

        spriteRenderer.sprite = dudeHappy;
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
            (dialogFlowState == 2 || dialogFlowState == 3))
        {
            nextDialogTimer = 0;
            dialogFlowState = 4;
            mistakeDone = true;

            spriteRenderer.sprite = dudePissed;
            textMesh.text = DialogsUtility.dialogs[DialogInstance.Pissed];
        }

        //Dude ti da la possibilità di rimediare (l'hai già fatto arrabbiare almeno una volta)
        if (nextDialogTimer >= 6 && !upperPin.IsConnected && !lowerPin.IsConnected &&
            dialogFlowState == 4)
        {
            spriteRenderer.sprite = dudePissed;
            textMesh.text = DialogsUtility.dialogs[DialogInstance.RedeemYourself];
            dialogFlowState = 5;
        }

        //primo dialogo
        if (nextDialogTimer > 10 && !upperPin.IsConnected &&
            dialogFlowState == 0)
        {
            nextDialogTimer = 0;
            dialogFlowState = 1;
            spriteRenderer.sprite = dudeFine;
            textMesh.text = DialogsUtility.dialogs[DialogInstance.GoToUpperPin];
        }

        //se primo dialogo skippato... parte 1
        if (nextDialogTimer <= 10 && upperPin.IsConnected && !lowerPin.IsConnected &&
            dialogFlowState == 0)
        {
            nextDialogTimer = 0;
            dialogFlowState = 3;
            //da cambiare con dude wow
            spriteRenderer.sprite = dudeHappy;
            textMesh.text = DialogsUtility.dialogs[DialogInstance.GoToLowerPinSavagePartOne];
        }

        //se non hai già fatto arrabbiare Dude una volta
        if (!mistakeDone)
            //se primo dialogo skippato... parte 2
            if (nextDialogTimer >= 6 && upperPin.IsConnected && !lowerPin.IsConnected &&
                dialogFlowState == 3)
            {
                dialogFlowState = 2;
                spriteRenderer.sprite = dudeFine;
                textMesh.text = DialogsUtility.dialogs[DialogInstance.GoToLowerPinSavagePartTwo];
            }

        //altrimenti, prosegui normale
        if (upperPin.IsConnected && !lowerPin.IsConnected &&
            (dialogFlowState == 1 || dialogFlowState == 5 || dialogFlowState == 4))
        {
            dialogFlowState = 2;
            spriteRenderer.sprite = dudeFine;
            textMesh.text = DialogsUtility.dialogs[DialogInstance.GoToLowerPin];
        }

        //goal
        if (upperPin.IsConnected && lowerPin.IsConnected &&
            dialogFlowState == 2)
        {
            spriteRenderer.sprite = dudeHappy;
            textMesh.text = DialogsUtility.dialogs[DialogInstance.GoalReached];
        }
    }

    public void SecondTutorialBehavoiur()
    {
        nextDialogTimer += Time.deltaTime;

        var blueUpperPin = MapUtility.UpperPins.ElementAt(0);
        var pinkUpperPin = MapUtility.UpperPins.ElementAt(1);
        var lowerPin = MapUtility.LowerPins.First();

        //primo dialogo
        if (nextDialogTimer >= 10 && !blueUpperPin.IsConnected && !pinkUpperPin.IsConnected &&
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
            //da sostituire con wow
            spriteRenderer.sprite = dudeHappy;
            textMesh.text = DialogsUtility.dialogs[DialogInstance.NoTimeToLose];
        }

        //secondo dialogo
        if (nextDialogTimer > 10 && !blueUpperPin.IsConnected && !pinkUpperPin.IsConnected &&
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
            //nextDialogTimer = 0;
            spriteRenderer.sprite = dudeFine;
            textMesh.text = DialogsUtility.dialogs[DialogInstance.Attach];
        }

        //quarto dialogo
        if (lowerPin.IsConnected &&
            (dialogFlowState == 3 || dialogFlowState == 7 || dialogFlowState == 11 || dialogFlowState == 9))
        {
            dialogFlowState = 4;
            //nextDialogTimer = 0;
            spriteRenderer.sprite = dudeHappy;
            textMesh.text = DialogsUtility.dialogs[DialogInstance.Detach];
        }

        //quinto dialogo
        if (!lowerPin.IsConnected &&
            (dialogFlowState == 4 || dialogFlowState == 10))
        {
            dialogFlowState = 5;
            //nextDialogTimer = 0;

            //da sostituire con DudeTwinkling
            spriteRenderer.sprite = dudeFine;
            textMesh.text = DialogsUtility.dialogs[DialogInstance.Remove];
        }

        //speciale: hai attaccato il cavo anche se Dude ti ha chiesto di rimuoverlo
        if (lowerPin.IsConnected &&
            dialogFlowState == 5)
        {
            dialogFlowState = 10;
            mistakeDone = true;
            //nextDialogTimer = 0;

            //da sostituire con DudeTwinkling
            spriteRenderer.sprite = dudePissed;
            textMesh.text = DialogsUtility.dialogs[DialogInstance.BadTime];
        }

        //speciale: Dude ha perso la pazienza
        if (!lowerPin.IsConnected && !blueUpperPin.IsConnected && !pinkUpperPin.IsConnected &&
            dialogFlowState == 5 && mistakeDone)
        {
            dialogFlowState = 10;
            //nextDialogTimer = 0;

            //da sostituire con DudeTwinkling
            spriteRenderer.sprite = dudePissed;
            textMesh.text = DialogsUtility.dialogs[DialogInstance.WasteTime];
        }

        //sesto dialogo
        if (!blueUpperPin.IsConnected && !pinkUpperPin.IsConnected && !mistakeDone &&
            dialogFlowState == 5)
        {
            dialogFlowState = 6;
            //nextDialogTimer = 0;
            spriteRenderer.sprite = dudeFine;
            textMesh.text = DialogsUtility.dialogs[DialogInstance.RepeatForPink];
        }

        //speciale: finchè non fai quello che dice Dude, non si va avanti
        if (blueUpperPin.IsConnected &&
            dialogFlowState == 6)
        {
            dialogFlowState = 12;
            //nextDialogTimer = 0;
            spriteRenderer.sprite = dudePissed;
            textMesh.text = DialogsUtility.dialogs[DialogInstance.IAmRuler];
        }

        //sesto dialogo
        if (!blueUpperPin.IsConnected && pinkUpperPin.IsConnected && lowerPin.IsConnected &&
            (dialogFlowState == 6 || dialogFlowState == 12))
        {
            //nextDialogTimer = 0;
            spriteRenderer.sprite = dudeFine;
            textMesh.text = DialogsUtility.dialogs[DialogInstance.GoalReached];
        }
    }

    public void StandardBehavoiur()
    {

    }
}
