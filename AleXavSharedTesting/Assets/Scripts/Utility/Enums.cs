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
    #endregion
    #region FirstLevel
    Welcome,
    Pissed,
    RedeemYourself,
    GoToUpperPin,
    GoToLowerPin,
    GoToLowerPinSavagePartOne,
    GoToLowerPinSavagePartTwo,
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
}
