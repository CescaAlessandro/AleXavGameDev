using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public string LastLevelPlayedName = "Level 1";
    public MenuManager menuManager;
    public bool debugIsFirstStart = false;

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
        //Debug.Log("mus: " + volume);
    }
    public void SaveSfxVolume(float volume)
    {
        PlayerPrefs.SetFloat("SfxVolume", volume);
        //Debug.Log("sfx: " + volume);
    }
}
