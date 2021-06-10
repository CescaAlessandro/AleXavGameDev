using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DialogsUtility
{
    public static Dictionary<DialogInstance, string> dialogs = new Dictionary<DialogInstance, string>
    {
        {DialogInstance.Welcome, "Hi Chip! How are you? I know it is your first day, I'm sure we can be good friends." },
        
        {DialogInstance.GoalReached, "Mission accomplished! See you on the next level."},
        
        {DialogInstance.Pissed, "Are you trying to make me lose my temper?"},
        
        {DialogInstance.RedeemYourself, "Please, drag and drop yourself to the upper pin, again..."},
        
        {DialogInstance.Joke, "Are you joking?"},
        
        {DialogInstance.GoToUpperPin, "To begin, drag and drop yourself to the upper pin."},

        {DialogInstance.GoToLowerPin, "Great! Next, drag yourself to the lower pin."},

        {DialogInstance.GoToLowerPinSavagePartOne, "Wow! Have you already attended the movement training course?"},

        {DialogInstance.GoToLowerPinSavagePartTwo, "However, now drag and drop yourself to the lower pin."},

        {DialogInstance.CanDetach, "You must know that the cables can be also detached, of course."},

        {DialogInstance.NoTimeToLose, "No time to lose, uh? I understand. Wire it to the lower pin."},

        {DialogInstance.LongDay, "I think this will be a loooong day... please, restart from the blue one."},

        {DialogInstance.CreateBlue, "But first, you need to begin wiring. Start from the blue pin."},

        {DialogInstance.ISaidBlue, "I said blue... nevermind... wire the cable to the lower pin."},

        {DialogInstance.OkBlue, "Thanks. This time I will forgive you."},

        {DialogInstance.Attach, "And then, attach the cable to the lower pin."},

        {DialogInstance.Detach, "Great! Now, to detach the cable, drag an drop yourself to the lower pin."},

        {DialogInstance.Remove, "And next, delete it. I think you know how to do it."},

        {DialogInstance.BadTime, "You're gonna have a bad time. I suggest you to detach the cable."},

        {DialogInstance.RepeatForPink, "Excellent! Complete your daily job connecting the pink pin to the lower one."},

        {DialogInstance.WasteTime, "Ok, I have no more time to waste. See you on the next level."},

        {DialogInstance.IAmRuler, "No problem. We can stay here forever, until you wire the pink one."},

        {DialogInstance.WelcomeAgain, "Hi Chip! Let's add something new today."}
    };
}
