using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject LevelSelectMenu;
    public GameObject SettingsMenu;
    public GameObject FirstStartMainMenu;
    public SaveManager SaveManager;
    public UnityEngine.UI.Slider musicSlider;
    public UnityEngine.UI.Slider sfxSlider;
    public bool DebugStart = false;

    public static MenuManager mm;
    public void Setup()
    {
        mm = this;
    }

    public static MenuManager Instance()
    {
        return mm;
    }
    //loads the right main menu based on whether it's the first time the game is played or not
    public void loadMainMenu()
    {
        if (SaveManager.isFirstStart())
        {
            FirstStartMainMenu.SetActive(true);
        }
        else
        {
            MainMenu.SetActive(true);
        }
    }

    public void FromMainToLevelSelect()
    {
        MainMenu.SetActive(false);
        FirstStartMainMenu.SetActive(false);
        LevelSelectMenu.SetActive(true);
    }
    public void FromMainToSettings()
    {
        MainMenu.SetActive(false);
        FirstStartMainMenu.SetActive(false);
        SettingsMenu.SetActive(true);
    }
    public void FromMainToContinue()
    {
        MainMenu.SetActive(false);
        FirstStartMainMenu.SetActive(false);
        LevelManager.LoadLevel(SaveManager.LastLevelPlayedName);
    }
    public void FromLevelSelectToMain()
    {
        LevelSelectMenu.SetActive(false);
        loadMainMenu();
    }
    public void FromSettingsToMain()
    {
        SettingsMenu.SetActive(false);
        loadMainMenu();
    }
    public void FromLevelSelectToLevel(TMPro.TextMeshProUGUI levelName)
    {
        LevelManager.LoadLevel(levelName.text);
    }
    public void MusicVolumeWidgetChanged(float volume)
    {
        SaveManager.SaveMusicVolume(volume);
        AudioManager.Instance().SetMusicVolume(volume);
    }
    public void SfxVolumeWidgetChanged(float volume)
    {
        SaveManager.SaveSfxVolume(volume);
        AudioManager.Instance().SetSfxVolume(volume);
    }
    public void changeMusicVolumeWidgetValue(float value)
    {
        musicSlider.SetValueWithoutNotify(value);
    }
    public void changeSfxVolumeWidgetValue(float value)
    {
        sfxSlider.SetValueWithoutNotify(value);
    }
}
