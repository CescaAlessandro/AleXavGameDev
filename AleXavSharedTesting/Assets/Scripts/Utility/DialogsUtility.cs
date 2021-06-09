using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DialogsUtility
{
    public static Dictionary<DialogInstance, Dialog> dialogs = new Dictionary<DialogInstance, Dialog>
    {
        {DialogInstance.Welcome, new Dialog("Hi Chip! How are you? I know it is your first day, I'm sure we can be good friends.", false) },
        
        {DialogInstance.GoalReached, new Dialog("Mission accomplished! See you on the next level.", false)},
        
        {DialogInstance.Pissed, new Dialog("Are you trying to make me lose my temper?", false)},
        
        {DialogInstance.RedeemYourself, new Dialog("Please, drag and drop yourself to the upper pin, again...", false)},
        
        {DialogInstance.Joke, new Dialog("views", false)},
        
        {DialogInstance.GoToUpperPin, new Dialog("To begin, drag and drop yourself to the upper pin.", false)},

        {DialogInstance.GoToLowerPin, new Dialog("Great! Next, drag yourself to the lower pin.", false)},

        {DialogInstance.GoToLowerPinSavagePartOne, new Dialog("Wow! Have you already attended the movement training course?", false)},

        {DialogInstance.GoToLowerPinSavagePartTwo, new Dialog("However, now drag and drop yourself to the lower pin.", false)},

        {DialogInstance.SecondTutorial, new Dialog("boh", false)}
    };
}
