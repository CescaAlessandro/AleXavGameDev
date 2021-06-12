using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PinType
{
    Lower,
    Upper
}

public enum FluxLifeState
{
    Idle,
    Moving,
    Depleting,
    Depleted
}

public enum DialogInstance
{
    #region General
    GoalReached,
    Joke,
    Mistake,
    BeautifulDay,
    #endregion
    #region FirstLevel
    Presentation,
    Welcome,
    Pissed,
    RedeemYourself,
    GoToUpperPin,
    GoToLowerPin,
    GoToLowerPinSavagePartOne,
    GoToLowerPinSavagePartTwo,
    SkippedTutorial,
    #endregion
    #region SecondLevel
    WelcomeAgain,
    CanDetach,
    CreateBlue,
    Attach,
    Detach,
    Remove,
    NoTimeToLose,
    LongDay,
    ISaidBlue,
    OkBlue,
    BadTime,
    WasteTime,
    RepeatForPink,
    IAmRuler,
    #endregion
    #region ThirdLevel
    WelcomeThird,
    ExplanationPartOne,
    ExplanationPartTwo,
    ExplanationPartThree,
    ExplanationPartFour,
    ExplanationPartFive,
    ExplanationPartSix,
    ExplanationPartSeven,
    ExplanationPartEight,
    HereItIs,
    WhatAreYouWaitingFor,
    KindaSus,
    NotScared,
    ByTheWay,
    DailyGoal,
    ALotTime,
    #endregion
}
public enum directions
{
    Top,
    Bottom,
    Left,
    Right
}
