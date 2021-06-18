using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveManager : MonoBehaviour
{
    public MenuManager menuManager;
    public bool debugIsFirstStart = false;
    private bool firstStart = true;
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
        var firstTimePlaying = PlayerPrefs.GetInt("First start", 1);
        if (firstTimePlaying == 1)
        {
            firstStart = true;
        }
        else
        {
            firstStart = false;
        }
        MenuManager.Instance().loadMainMenu();
        firstStart = false;
        PlayerPrefs.SetInt("First start", 0);
        UnlockLevel("Level 1");
        //UnlockLevel("Level 7");
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
        return debugIsFirstStart || firstStart;
    }
    public void SaveMusicVolume(float volume)
    {
        PlayerPrefs.SetFloat("MusicVolume", volume);
        //Debug.Log("mus: " + volume);
    }
    public void SaveSfxVolume(float volume)
    {
        PlayerPrefs.SetFloat("SfxVolume", volume);
        //Debug.Log("sfx: " + volume);
    }
    public void SaveLastScene(string SceneName)
    {
        PlayerPrefs.SetString("LastScene", SceneName);
        //Debug.Log("sfx: " + volume);
    }
    public void UnlockLevel(string SceneName)
    {
        PlayerPrefs.SetInt(SceneName, 1);
        //Debug.Log("sfx: " + volume);
    }
    public void LockLevel(string SceneName)
    {
        PlayerPrefs.SetInt(SceneName, 0);
        //Debug.Log("sfx: " + volume);
    }
    public int GetLevelState(string SceneName)
    {
        return PlayerPrefs.GetInt(SceneName);
        //Debug.Log("sfx: " + volume);
    }
}
