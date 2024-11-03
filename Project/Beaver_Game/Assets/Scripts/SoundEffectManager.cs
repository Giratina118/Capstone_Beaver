using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectManager : MonoBehaviour
{
    public AudioClip[] audioClips;
    
    public AudioSource playerAudioSource;
    public AudioSource bgmAudio;
    public AudioSource buttonClickAudio;
    public AudioSource getResourceAudio;

    AudioManager audioManager;


    public void PlayGetResourceSound(int resourceNum)
    {
        getResourceAudio.clip = audioClips[resourceNum + 3];
        getResourceAudio.Play();
    }

    public void StopGetResourceSound()
    {
        getResourceAudio.Stop();
    }

    public void SetPlayerAudioClip(int clipNum)
    {
        playerAudioSource.clip = audioClips[clipNum];
        if (clipNum == 1)
            playerAudioSource.pitch = 2.0f;
        else
            playerAudioSource.pitch = 1.0f;
    }

    public void PlayPalyerAudio()
    {
        playerAudioSource.Play();
    }

    public void StopPlayerAudio()
    {
        playerAudioSource.Stop();
    }

    public void ButtonClickSound()
    {
        buttonClickAudio.Play();
    }

    public void SetVolume()
    {
        audioManager.SetBGMVolume(audioManager.GetBGMVolume());
        audioManager.SetSFXVolume(audioManager.GetSFXVolume());
    }

    void Start()
    {
        audioManager = GameObject.FindObjectOfType<AudioManager>();
        audioManager.soundEffectManager = this;
        SetVolume();
    }
}
