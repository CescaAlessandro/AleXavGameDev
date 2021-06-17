using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveManager : MonoBehaviour
{
    public MenuManager menuManager;
    public bool debugIsFirstStart = false;
    public LevelSelectionBehaviour levelSelectionMenuObject;

    public static SaveManager sm;
    public void Setup()
    {
        //Add loading saves here
        sm = this;
        
        menuManager.loadMainMenu();
        float musVol = PlayerPrefs.GetFloat("MusicVolume",1f);
        float sfxVol = PlayerPrefs.GetFloat("SfxVolume",1f);
        MenuManager.Instance().changeMusicVolumeWidgetValue(musVol);
        MenuManager.Instance().changeSfxVolumeWidgetValue(sfxVol);
        AudioManager.Instance().SetMusicVolume(musVol);
        AudioManager.Instance().SetSfxVolume(sfxVol);

        UnlockLevel("Level 1");

        //debug only
        //LockLevel("Level 2");
        //LockLevel("Level 3");
        //LockLevel("Level 4");

        levelSelectionMenuObject.UpdateLevels();
    }

    public static SaveManager Instance()
    {
        return sm;
    }
    public bool isFirstStart()
    {
        return debugIsFirstStart;
    }
    public void SaveMusicVolume(float volume)
    {
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }
    public void SaveSfxVolume(float volume)
    {
        PlayerPrefs.SetFloat("SfxVolume", volume);
    }
    public void SaveLastScene(string SceneName)
    {
        PlayerPrefs.SetString("LastScene", SceneName);
    }
    public void UnlockLevel(string SceneName)
    {
        PlayerPrefs.SetInt(SceneName, 1);
    }
    public void LockLevel(string SceneName)
    {
        PlayerPrefs.SetInt(SceneName, 0);
    }
    public int GetLevelState(string SceneName)
    {
        return PlayerPrefs.GetInt(SceneName);
    }
}
