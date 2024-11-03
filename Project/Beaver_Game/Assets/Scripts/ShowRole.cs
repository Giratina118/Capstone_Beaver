using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowRole : MonoBehaviour
{
    public Sprite[] beaverRoleSprites;
    public TimerManager timerManager;
    private float closeTime;


    public void SetShowRuleImage(bool isSpy)
    {
        Image isSpyImage = this.transform.GetChild(0).gameObject.GetComponent<Image>();
        TMP_Text isSpyText = this.transform.GetChild(1).gameObject.GetComponent<TMP_Text>();

        if (isSpy)
        {
            isSpyImage.sprite = beaverRoleSprites[1];
            isSpyText.text = "You are a SPY beaver";
            isSpyText.color = Color.red;
        }
        else
        {
            isSpyImage.sprite = beaverRoleSprites[0];
            isSpyText.text = "You are a citizen beaver";
            isSpyText.color = Color.white;
        }
    }

    void Start()
    {
        closeTime = timerManager.GetNowTime() - 5.0f;
    }

    void Update()
    {
        if (timerManager.GetNowTime() <= closeTime)
            this.gameObject.SetActive(false);
    }
}
