using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource electricZap;
    public float zapMaxVolume = 0.001f;
    public AudioSource music;
    public static AudioManager am;

    public void Setup()
    {
        am = this;
        DontDestroyOnLoad(this);
        //music.Play();
    }
    public static AudioManager Instance()
    {
        return am;
    }
    public void PlayZap()
    {
        electricZap.loop = false;
        electricZap.Play();
    }
    public void StopZap()
    {
        if(electricZap.isPlaying)
            electricZap.Stop();
    }
    public void SetMusicVolume(float volume)
    {
        music.volume = volume;
        Debug.Log(music.volume);
    }
    public void SetSfxVolume(float volume)
    {
        Debug.Log(zapMaxVolume);
        electricZap.volume = volume * zapMaxVolume;
    }
}
