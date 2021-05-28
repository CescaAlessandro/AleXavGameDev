using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public string LastLevelPlayedName = "Level 1";
    public MenuManager menuManager;
    public bool debugIsFirstStart = false;

    void Start()
    {
        //Add loading saves here
        menuManager.loadMainMenu();
    }

    public bool isFirstStart()
    {
        return debugIsFirstStart;
    }
}
