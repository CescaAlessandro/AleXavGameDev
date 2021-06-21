using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //riferimenti agli audiosources
    public AudioSource electricZap;
    public AudioSource lifeLoss;
    public AudioSource startDownload;
    public AudioSource attachDetach;
    public AudioSource gameOver;
    public AudioSource levelCompleted;
    public AudioSource dudeVoice;
    public AudioSource music;

    public float musicMaxVolume = 0.2f;
    public float electricZapMaxVolume = 0.08f;
    public float lifeLossMaxVolume = 0.08f;
    public float startDownloadMaxVolume = 0.08f;
    public float attachDetachMaxVolume = 0.08f;
    public float gameOverMaxVolume = 0.08f;
    public float levelCompletedMaxVolume = 0.08f;
    public float dudeVoiceMaxVolume = 0.4f;

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

    #region Zap
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
    public void StopZap()
    {
        if (electricZap.isPlaying)
            electricZap.Stop();
    }
    #endregion
    #region Attach n' Detach
    public void PlayAttachDetach()
    {
        //Debug.Log(attachDetach.volume);
        attachDetach.loop = false;

        attachDetach.Play();
    }
    #endregion
    #region Download
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
    #endregion
    #region GameOver
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
    #endregion
    #region Level Completed
    public void PlayLevelCompleted()
    {
        levelCompleted.loop = false;
        levelCompleted.Play();
    }
    public void StopLevelCompleted()
    {
        if (levelCompleted.isPlaying)
            levelCompleted.Stop();
    }
    #endregion
    #region Dude
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
    #endregion
    #region Lose Life
    public void PlayLoseLife()
    {
        lifeLoss.loop = false;
        lifeLoss.Play();
    }
    #endregion
    #region General Methods
    public void SetMusicVolume(float volume)
    {
        music.volume = volume * musicMaxVolume;
    }
    public void SetSfxVolume(float volume)
    {
        electricZap.volume = volume * electricZapMaxVolume;
        lifeLoss.volume = volume * lifeLossMaxVolume;
        startDownload.volume = volume * startDownloadMaxVolume;
        attachDetach.volume = volume * attachDetachMaxVolume;
        dudeVoice.volume = volume * dudeVoiceMaxVolume;
        gameOver.volume = volume * gameOverMaxVolume;
        levelCompleted.volume = volume * levelCompletedMaxVolume;
    }
    public void StopAllInGameSfx()
    {
        StopGameOver();
        StopDudeVoice();
        StopLevelCompleted();
        StopStartDownload();
        StopZap();
    }
    #endregion
}
