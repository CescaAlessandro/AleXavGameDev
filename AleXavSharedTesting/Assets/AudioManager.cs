using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource electricZap;
    public AudioSource lifeLoss;
    public AudioSource startDownload;
    public AudioSource attachDetach;
    public AudioSource gameOver;
    public AudioSource levelCompleted;
    public AudioSource dudeVoice;
    public AudioSource music;

    public float fsxMaxVolume = 0.5f;

    public static AudioManager am;

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
        Debug.Log(attachDetach.volume);
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
    public void PlayGameOver()
    {
        gameOver.loop = false;
        gameOver.Play();
    }
    public void StopGameOver()
    {
        if(gameOver.isPlaying)
            gameOver.Stop();
    }
    public void PlayLevelCompleted()
    {
        levelCompleted.loop = false;
        levelCompleted.Play();
    }
    public void StopLevelCompleted()
    {
        if(levelCompleted.isPlaying)
            levelCompleted.Stop();
    }
    public void PlayDudeVoice()
    {
        dudeVoice.loop = false;
        dudeVoice.Play();
    }
    public void StopDudeVoice()
    {
        if(dudeVoice.isPlaying)
            dudeVoice.Stop();
    }
    public void PlayLoseLife()
    {
        lifeLoss.loop = false;
        lifeLoss.Play();
    }
    public void SetMusicVolume(float volume)
    {
        music.volume = volume;
    }
    public void SetSfxVolume(float volume)
    {
        electricZap.volume = volume * 0.5f;
        lifeLoss.volume = volume;
        startDownload.volume = volume;
        attachDetach.volume = volume;
        dudeVoice.volume = volume;
        gameOver.volume = volume;
        levelCompleted.volume = volume;
    }
    public void StopAllInGameSfx()
    {
        StopGameOver();
        StopDudeVoice();
        StopLevelCompleted();
        StopStartDownload();
        StopZap();
    }
}
