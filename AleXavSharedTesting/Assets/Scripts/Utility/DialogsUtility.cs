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
        {DialogInstance.Buffering, "<*... buffering ...*>"},
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
        {DialogInstance.BeautifulDay, "It's a beautiful day outside."},

        {DialogInstance.ExplanationBridgePartOne, "Hey Chip, today we expect a lot of data to come in. But we also have something new on the board to help us" },

        {DialogInstance.ExplanationBridgePartTwo, "That's a bridge, cables can be laid both above and below it. This will allow us to cross cables." },

        {DialogInstance.BridgeOrangePin, "Go to the orange pin to create a cable" },

        {DialogInstance.BridgeCrossVertically, "Now move through the bridge vertically to cross it from above" },

        {DialogInstance.CompleteFirstCable, "Complete the cable connecting it to the orange pin" },

        {DialogInstance.BridgeCrossHorizontally, "Good, now create a new cable on the pink pin and drag it across the bridge horizontally to pass underneath it" },

        {DialogInstance.CompleteSecondCable, "Connect the cable to the lower pink pin" },

        {DialogInstance.TwoFluxesArriving, "Two fluxing are arriving at the same time, without the bridge we couldn't catch both" },

        {DialogInstance.FluxesMissed, "You missed those fluxes... No problem but try to catch the next ones." },

        {DialogInstance.NotUsingBridge, "Chip, you're not using the bridge, we won't be able to deal with so many fluxes without using it." },
        {DialogInstance.WrongColorsConnectedPartOne, "You connected the wrong pins, I thought you learned about pin colors three level ago."},
        {DialogInstance.WrongColorsConnectedPartTwo, "Now please, disconnect those cables and connect them according to the pins' colors"},
        {DialogInstance.BridgeUsedWellDone, "Ok, now that you learned how to use bridges, deal with the incoming fluxes to complete the level"},

        { DialogInstance.Snake, "Chip, what happened? Chip!? CHIIIIIIIIIIIP!"},
        {DialogInstance.YouDied, "YOU DIED"},
        {DialogInstance.WhyLagging, "Why is it lagging?"},
        {DialogInstance.RestartRouter, "Should I restart the router?"},
        {DialogInstance.TutorialThreeFluxesArriving, "Fluxes incoming!"},
        {DialogInstance.KingBrowser, "So long, King-a Browser!"},
        {DialogInstance.FireInTheHole, "Fire in the hole!"},
        {DialogInstance.FusRohDah, "FUS ROH DAH!"},

        {DialogInstance.ExplanationHolePartOne, "Hey Chip, are you excited? You'll learn how to use a new tool today" },

        {DialogInstance.ExplanationHolePartTwo, "As you can see there are two holes on the board, cables can enter one and exit from the other" },

        {DialogInstance.ExplanationHolePartThree, "Connect a cable to the red pin, bring it to the nearest hole and drop it." },

        {DialogInstance.ExplanationHolePartFour, "Now move yourself to the other hole to grab the cable" },

        {DialogInstance.ExplanationHolePartFive, "Nice, now complete the cable attaching it to the lower pin with the same color" },

        {DialogInstance.ExplanationHoleDifferentColor, "You chose a different color? That's fine i like that one too" },

        {DialogInstance.ExplanationHoleWrongHole, "That's not the nearest hole but ok, keep going" },

        {DialogInstance.ExplanationHoleNotUsed, "Ok you connected the pins but now try doing it using the holes, so you learn how to use them" },

        {DialogInstance.ExplanationHolePartSix, "It seems there aren't any fluxes coming in today..." },

        {DialogInstance.ExplanationHolePartSeven, "Well, I'll teach you how to disconnect cables from holes and then we can call it a day, ok?" },

        {DialogInstance.ExplanationHolePartEight, "So, disconnect the cable from the lower pin, same as usual" },

        {DialogInstance.ExplanationHolePartNine, "Now drag and drop yourself to the hole to disconnect the cable you are holding" },

        {DialogInstance.ExplanationHolePartTen, "To disconnect the second half of the cable go to the other hole" },

        {DialogInstance.ExplanationHolePartEleven, "Ok, I think you know how to detach the cable from the upper pin" },

        {DialogInstance.ExplanationHolePartTwelve, "Nice, you learned everything about holes" },

        {DialogInstance.ExplanationHolePartThirteen, "I heard from management that they are gonna bring in more pins for the next level..." },

        {DialogInstance.ExplanationHolePartFourteen, "Also there are going to be both bridges and holes. I guess they expect a lot of fluxes..." },

        {DialogInstance.ExplanationHolePartFifteen, "I hope you're ready to deal with all that." },

        //todo: various gameover + dude presentation
    };
}
