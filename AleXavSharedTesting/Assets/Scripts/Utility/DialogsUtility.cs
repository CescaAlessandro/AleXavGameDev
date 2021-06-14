using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DialogsUtility
{
    public static Dictionary<DialogInstance, string> dialogs = new Dictionary<DialogInstance, string>
    {
        {DialogInstance.Presentation, "Hi Chip! How are you? My name is Dude, but you can call me Dude." },

        {DialogInstance.Welcome, "From now on, you will be my assistant. I'm sure we can be good friends." },
        
        {DialogInstance.GoalReached, "Mission accomplished! See you on the next level."},
        
        {DialogInstance.Pissed, "Are you trying to make me lose my temper?"},
        
        {DialogInstance.RedeemYourself, "Please, drag and drop yourself to the upper pin, again..."},
        
        {DialogInstance.Joke, "Are you joking?"},
        
        {DialogInstance.GoToUpperPin, "To begin, drag and drop yourself to the upper pin."},

        {DialogInstance.GoToLowerPin, "Great! Next, drag yourself to the lower pin."},

        {DialogInstance.GoToLowerPinSavagePartOne, "Wow! Have you already attended the movement training course?"},

        {DialogInstance.GoToLowerPinSavagePartTwo, "However, now drag and drop yourself to the lower pin."},

        {DialogInstance.SkippedTutorial, "Well, maybe you need more difficult challenges. See you on the next level."},

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

        {DialogInstance.WelcomeAgain, "Hi Chip! Let's add something new today."},

        {DialogInstance.WelcomeThird, "Good morning Chip! I think we are ready to play seriously."},

        {DialogInstance.ExplanationPartOne, "The goal of your job is to let data fluxes reach their destination."},

        {DialogInstance.ExplanationPartTwo, "How? Connecting the corresponding upper pin to a lower one."},

        {DialogInstance.ExplanationPartThree, "Be sure to connect pins which share the same colour."},

        {DialogInstance.ExplanationPartFour, "Otherwise, the flux won't start downloading!"},

        {DialogInstance.ExplanationPartFive, "When there are fluxes ready to being satisfied, you will hear this sound."},

        {DialogInstance.ExplanationPartSix, "Pay attention! If a flux isn't satisfied for a long time, you will lose a life."},

        {DialogInstance.ExplanationPartSeven, "Your remaining lives are represented by the Wi-Fi bars, on the bottom-right."},

        {DialogInstance.ExplanationPartEight, "I hope you will never let us be offline..."},

        {DialogInstance.HereItIs, "Oh! Here it is. New data flux incoming!"},

        {DialogInstance.NotScared, "Don't be scared. For the sake of this tutorial you will not lose lifes today."},

        {DialogInstance.WhatAreYouWaitingFor, "What are you waiting for? Connect those two pins!"},

        {DialogInstance.KindaSus, "<*Today Chip is kinda sus... I hope that flux will be satisified sooner or later...*>"},

        {DialogInstance.DailyGoal, "And that's all! Satisfy all fluxes to complete your daily job."},

        {DialogInstance.ByTheWay, "Oh, maybe I forgot to mention: you can not overlap two cables."},

        {DialogInstance.ALotTime, "Hmm... Maybe I should start looking for a new job..."},

        {DialogInstance.DamnCable, "All we had to do, was wiring those damn pins, Chip!"},

        {DialogInstance.BeautifulDay, "It's a beautiful day outside."}

        //todo: various gameover + dude presentation
    };
}
