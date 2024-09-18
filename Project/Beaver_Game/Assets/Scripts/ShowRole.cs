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
        if (isSpy)
        {
            this.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = beaverRoleSprites[1];
            this.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = "You are a SPY beaver";
            this.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().color = Color.red;
        }
        else
        {
            this.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = beaverRoleSprites[0];
            this.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = "You are a citizen beaver";
            this.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().color = Color.white;
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
