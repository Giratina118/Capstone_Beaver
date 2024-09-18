using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionSceneManager : MonoBehaviour
{
    public GameObject titleObjects;
    public GameObject settingObjects;
    public AudioManager audioManager;

    private const string bgmVolumeKey = "bgmVolumeFloat";
    private const string sfxVolumeKey = "sfxVolumeFloat";


    public void OnClickSettingButton()
    {
        titleObjects.SetActive(false);
        settingObjects.SetActive(true);
    }

    public void OnClickBackToTitleButton()
    {
        settingObjects.SetActive(false);
        titleObjects.SetActive(true);

        PlayerPrefs.SetFloat(bgmVolumeKey, audioManager.GetBGMVolume());
        PlayerPrefs.SetFloat(sfxVolumeKey, audioManager.GetSFXVolume());
    }

    public void OnClickExitGameButton()
    {
        PlayerPrefs.SetFloat(bgmVolumeKey, audioManager.GetBGMVolume());
        PlayerPrefs.SetFloat(sfxVolumeKey, audioManager.GetSFXVolume());

        Application.Quit();
    }

    void Start()
    {
        audioManager = GameObject.FindObjectOfType<AudioManager>();

        audioManager.SetBGMVolume(PlayerPrefs.GetFloat(bgmVolumeKey));
        audioManager.SetSFXVolume(PlayerPrefs.GetFloat(sfxVolumeKey));
    }

    void Update()
    {
        
    }
}
