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
    public GameObject LevelFailedMenu;
    public GameObject PauseMenu;
    public GameObject LevelCompleteMenu;
    private GameObject AreYouSureMenu;

    public SaveManager SaveManager;
    public UnityEngine.UI.Slider musicSlider;
    public UnityEngine.UI.Slider sfxSlider;
    public bool DebugStart = false;

    public static MenuManager mm;
    public void Setup()
    {
        if(mm == null)
        {
            mm = this;
        }
        else
        {
            Destroy(gameObject);
        }
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
            LevelCompleteMenu.SetActive(false);
        }
        else
        {
            MainMenu.SetActive(true);
            LevelCompleteMenu.SetActive(false);
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
        LevelSelectMenu.SetActive(false);
        LevelManager.LoadLevel(levelName.text);
    }
    public void LoadPauseMenu()
    {
        Time.timeScale = 0;
        PauseMenu.SetActive(true);
    }
    public void FromPauseToMainMenu()
    {
        Time.timeScale = 1;
        MainMenu.SetActive(true);
        PauseMenu.SetActive(false);
        LevelManager.LoadLevel("MenuScene");
    }
    public void FromPauseToResume()
    {
        Time.timeScale = 1;
        PauseMenu.SetActive(false);
    }
    public void FromPauseToRestartLevel()
    {
        Time.timeScale = 1;
        PauseMenu.SetActive(false);
        LevelManager.LoadLevel(SceneManager.GetActiveScene().name);
    }
    public void LoadLevelCompleteMenu()
    {
        Time.timeScale = 0;
        LevelCompleteMenu.SetActive(true);
    }
    public void FromLevelCompleteToNextLevel()
    {
        Time.timeScale = 1;
        var nextLevelIndex = MapUtility.GetLevelNumber(SceneManager.GetActiveScene().name, "") + 1;

        string nextLevel = "Level " + nextLevelIndex;
        LevelCompleteMenu.SetActive(false);
        LevelManager.LoadLevel(nextLevel);
    }
    public void FromLevelCompleteToMainMenu()
    {
        Time.timeScale = 1;
        LevelCompleteMenu.SetActive(false);
        loadMainMenu();
        LevelManager.LoadLevel("MenuScene");
    }
    public void FromLevelCompleteToRestartLevel()
    {
        Time.timeScale = 1;
        LevelCompleteMenu.SetActive(false);
        LevelManager.LoadLevel(SceneManager.GetActiveScene().name);
    }
    public void LoadLevelFailedMenu()
    {
        Time.timeScale = 0;
        LevelFailedMenu.SetActive(true);
    }
    public void FromLevelFailedToSelectionLevel()
    {
        LevelFailedMenu.SetActive(false);
        LevelSelectMenu.SetActive(true);
    }
    public void FromLevelFailedToMainMenu()
    {
        Time.timeScale = 1;
        LevelFailedMenu.SetActive(false);
        LevelManager.LoadLevel("MenuScene");
    }
    public void FromLevelFailedToRestartLevel()
    {
        Time.timeScale = 1;
        LevelFailedMenu.SetActive(false);
        LevelManager.LoadLevel(SceneManager.GetActiveScene().name);
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
