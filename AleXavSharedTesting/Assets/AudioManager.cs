using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource electricZap;
    public AudioSource lifeLoss;
    public AudioSource startDownload;
    public AudioSource attachDetach;
    public float zapMaxVolume = 0.001f;
    public AudioSource music;
    public static AudioManager am;


    //todo: da inserire
    public AudioSource gameOver;
    public AudioSource levelCompleted;
    public AudioSource buttonPressed;

    public void Setup()
    {
        DontDestroyOnLoad(this);

        if (am == null)
        {
            am = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public static AudioManager Instance()
    {
        return am;
    }
    public void PlayZap()
    {
        electricZap.loop = false;

        if (!electricZap.isPlaying)
            electricZap.Play();
    }
    public void PlayLoopZap()
    {
        electricZap.loop = true;

        if (!electricZap.isPlaying)
            electricZap.Play();
    }
    public void PlayAttachDetach()
    {
        attachDetach.loop = false;

        attachDetach.Play();
    }
    public void StopZap()
    {
        if(electricZap.isPlaying)
            electricZap.Stop();
    }
    public void PlayStartDownload()
    {
        startDownload.loop = false;
        startDownload.Play();
    }
    public void StopStartDownload()
    {
        if(startDownload.isPlaying)
            startDownload.Stop();
    }
    public void PlayLoseLife()
    {
        lifeLoss.loop = false;
        lifeLoss.Play();
    }
    public void SetMusicVolume(float volume)
    {
        music.volume = volume;
        //Debug.Log(music.volume);
    }
    public void SetSfxVolume(float volume)
    {
        //Debug.Log(zapMaxVolume);
        electricZap.volume = volume * zapMaxVolume;
    }
}
