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

        if (sceneName.Equals("Level 1"))
        {
            nextDialogTimer = 0;
            textMesh.text = DialogsUtility.dialogs[DialogInstance.Welcome].Text;
        }
        else if (sceneName.Equals("Level 2"))
        {

        }
        else
        {

        }

        spriteRenderer.sprite = dudeFine;
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
            (DialogsUtility.dialogs[DialogInstance.GoToLowerPin].AlreadyShowed || DialogsUtility.dialogs[DialogInstance.GoToLowerPinSavagePartOne].AlreadyShowed))
        {
            if (!DialogsUtility.dialogs[DialogInstance.Pissed].AlreadyShowed)
                nextDialogTimer = 0;

            spriteRenderer.sprite = dudePissed;
            textMesh.text = DialogsUtility.dialogs[DialogInstance.Pissed].Text;
            DialogsUtility.dialogs[DialogInstance.Pissed].AlreadyShowed = true;
        }

        //Dude ti da la possibilità di rimediare (l'hai già fatto arrabbiare almeno una volta)
        if (nextDialogTimer >= 3 && !upperPin.IsConnected && !lowerPin.IsConnected &&
            DialogsUtility.dialogs[DialogInstance.Pissed].AlreadyShowed)
        {
            spriteRenderer.sprite = dudePissed;
            textMesh.text = DialogsUtility.dialogs[DialogInstance.RedeemYourself].Text;

            DialogsUtility.dialogs[DialogInstance.RedeemYourself].AlreadyShowed = true;
        }

        //primo dialogo
        if (nextDialogTimer > 6 && !upperPin.IsConnected &&
            !DialogsUtility.dialogs[DialogInstance.GoToLowerPinSavagePartOne].AlreadyShowed &&
            !DialogsUtility.dialogs[DialogInstance.GoToLowerPin].AlreadyShowed)
        {
            spriteRenderer.sprite = dudeFine;
            textMesh.text = DialogsUtility.dialogs[DialogInstance.GoToUpperPin].Text;

            nextDialogTimer = 0;
            DialogsUtility.dialogs[DialogInstance.GoToUpperPin].AlreadyShowed = true;
        }

        //se primo dialogo skippato... parte 1
        if (nextDialogTimer <= 6 && upperPin.IsConnected && !lowerPin.IsConnected &&
            !DialogsUtility.dialogs[DialogInstance.GoToUpperPin].AlreadyShowed)
        {
            if(!DialogsUtility.dialogs[DialogInstance.GoToLowerPinSavagePartOne].AlreadyShowed)
                nextDialogTimer = 0;

            //da cambiare con dude wow
            spriteRenderer.sprite = dudeHappy;
            textMesh.text = DialogsUtility.dialogs[DialogInstance.GoToLowerPinSavagePartOne].Text;

            DialogsUtility.dialogs[DialogInstance.GoToLowerPinSavagePartOne].AlreadyShowed = true; 
        }

        //se non hai già fatto arrabbiare Dude una volta
        if(!DialogsUtility.dialogs[DialogInstance.Pissed].AlreadyShowed)
        //se primo dialogo skippato... parte 2
            if (nextDialogTimer >= 3 && upperPin.IsConnected && !lowerPin.IsConnected &&
                DialogsUtility.dialogs[DialogInstance.GoToLowerPinSavagePartOne].AlreadyShowed &&
                !DialogsUtility.dialogs[DialogInstance.GoToLowerPin].AlreadyShowed)
            {
                spriteRenderer.sprite = dudeFine;
                textMesh.text = DialogsUtility.dialogs[DialogInstance.GoToLowerPinSavagePartTwo].Text;

                DialogsUtility.dialogs[DialogInstance.GoToLowerPinSavagePartTwo].AlreadyShowed = true;
            }

        //altrimenti, prosegui normale
        if (upperPin.IsConnected && !lowerPin.IsConnected && 
            ((!DialogsUtility.dialogs[DialogInstance.GoToLowerPinSavagePartOne].AlreadyShowed && DialogsUtility.dialogs[DialogInstance.GoToUpperPin].AlreadyShowed) ||
             DialogsUtility.dialogs[DialogInstance.RedeemYourself].AlreadyShowed))
        {
            spriteRenderer.sprite = dudeFine;
            textMesh.text = DialogsUtility.dialogs[DialogInstance.GoToLowerPin].Text;

            DialogsUtility.dialogs[DialogInstance.GoToLowerPin].AlreadyShowed = true;
        }

        //goal
        if (upperPin.IsConnected && lowerPin.IsConnected)
        {
            spriteRenderer.sprite = dudeHappy;
            textMesh.text = DialogsUtility.dialogs[DialogInstance.GoalReached].Text;
        }   
    }

    public void SecondTutorialBehavoiur()
    {

    }

    public void StandardBehavoiur()
    {

    }
}
