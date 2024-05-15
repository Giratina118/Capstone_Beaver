using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerManager : MonoBehaviour
{
    [SerializeField]
    private float timer = 60.0f * 15;
    private TMP_Text timerText;
    private float timeSpeed = 1;
    private bool basicTimeSpeedBool = true;
    private float timeSpeedRecoverTimer = 20.0f;
    private TowerInfo nowTower;
    public GameWinManager gameWinManager;

    public float GetNowTime()
    {
        return timer;
    }

    public void SetTimeSpeedRecoverTimer(float towerTime)
    {
        timeSpeedRecoverTimer = towerTime;
    }

    public void RadioComunicationTime(float speed, TowerInfo tower)
    {
        timeSpeed = speed;
        if (timeSpeed != 1.0f)
        {
            nowTower = tower;
            basicTimeSpeedBool = false;
            timeSpeedRecoverTimer = tower.remainComunicationTime;
        }
        else
        {
            basicTimeSpeedBool = true;
            tower.remainComunicationTime = timeSpeedRecoverTimer;
        }
        
    }

    public void TowerTime(float addTime)
    {
        timer -= addTime;
        ShowTimer();
    }

    public void ShowTimer()
    {
        timerText.text = Mathf.FloorToInt(timer / 60.0f).ToString() + " : ";
        if (timer % 60.0f < 10)
            timerText.text += "0";
        timerText.text += Mathf.FloorToInt(timer % 60.0f).ToString();
    }

    void Start()
    {
        timerText = this.GetComponent<TMP_Text>();
    }

    void Update()
    {
        timer -= timeSpeed * Time.deltaTime;
        ShowTimer();

        if (timer <= 0)
        {
            timer = 0.0f;
            ShowTimer();
            gameWinManager.TimeCheck();
        }

        if (!basicTimeSpeedBool && nowTower.remainComunicationTime >= 0.0f)
        {
            nowTower.gauge.transform.GetChild(2).gameObject.GetComponent<Image>().fillAmount = 1 - timeSpeedRecoverTimer / 20.0f; // 이거 점차 올라가도록 하기, 최대가 1.0, 최소 0.0

            Debug.Log(timeSpeedRecoverTimer);
            timeSpeedRecoverTimer -= Time.deltaTime;

            if (timeSpeedRecoverTimer <= 0.0f)
            {
                RadioComunicationTime(1.0f, nowTower);
            }
        }

    }
}
