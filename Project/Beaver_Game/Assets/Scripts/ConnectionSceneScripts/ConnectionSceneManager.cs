using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionSceneManager : MonoBehaviour
{
    public GameObject titleObjects;
    public GameObject settingObjects;
    public AudioManager audioManager;
    public GameObject openingScreen;
    public Image openingImage;
    public TMP_Text openingText;
    public Sprite[] openingSprites;
    private int currentOpeningNum = 0;

    public int firstPlay = 1;
    private const string firstPlayKey = "firstPlayBool";

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
        PlayerPrefs.SetInt(firstPlayKey, 1);
        Application.Quit();
    }

    void Start()
    {
        audioManager = GameObject.FindObjectOfType<AudioManager>();

        audioManager.SetBGMVolume(PlayerPrefs.GetFloat(bgmVolumeKey, 1.0f));
        audioManager.SetSFXVolume(PlayerPrefs.GetFloat(sfxVolumeKey, 1.0f));
        firstPlay = PlayerPrefs.GetInt(firstPlayKey);

        if (firstPlay == 1)
        {
            firstPlay = 0;
            PlayerPrefs.SetInt(firstPlayKey, 0);
            openingScreen.gameObject.SetActive(true);
            openingImage.sprite = openingSprites[currentOpeningNum];
        }
    }

    void Update()
    {
        if (openingScreen.gameObject.activeSelf && Input.GetMouseButtonDown(0))
        {
            currentOpeningNum++;

            if (currentOpeningNum == 1)
            {
                openingImage.rectTransform.offsetMax = new Vector2(0.0f, 150.0f);
                openingImage.rectTransform.offsetMin = new Vector2(0.0f, 150.0f);
            }

            if (currentOpeningNum >= openingSprites.Length)
                openingScreen.gameObject.SetActive(false);
            else
                openingImage.sprite = openingSprites[currentOpeningNum];
        }
    }
}
