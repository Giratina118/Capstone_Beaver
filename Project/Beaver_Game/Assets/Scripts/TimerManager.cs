using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerManager : MonoBehaviour
{
    [SerializeField]
    private float timer = 60.0f * 15.0f;   // ���� Ÿ�̸�, 15��
    private TMP_Text timerText; // Ÿ�̸� �ؽ�Ʈ
    private float timeSpeed = 1.0f;    // Ÿ�̸� �ӵ�(�������� ��ſ� ���� ��ȭ)
    private bool basicTimeSpeedBool = true; // Ÿ�̸� �ӵ� ��ȭ�� ���(�������� ��ſ� ���� ��ȭ)
    private float timeSpeedRecoverTimer = 20.0f;    // ���� ��� �ð�
    private TowerInfo nowTower; // ���� ��ġ�� ����ž�� ����(��� �ð� ����)
    public GameWinManager gameWinManager;   // �ð� �� �Ǹ� ���� ����

    public float GetNowTime()   // ���� �ð� ����
    {
        return timer;
    }

    public void SetTimeSpeedRecoverTimer(float towerTime)   // ����ž�� ���� ��� �ð� ���
    {
        timeSpeedRecoverTimer = towerTime;
    }

    public void RadioComunicationTime(float speed, TowerInfo tower) // ����� ���� Ÿ�̸� ����
    {
        timeSpeed = speed;
        if (timeSpeed != 1.0f)  // ����� �������̾��ٸ� ��� ���߱�
        {
            nowTower = tower;
            basicTimeSpeedBool = false;
            timeSpeedRecoverTimer = tower.remainComunicationTime;   // �ش� Ÿ���� ���� ��� �ð� ���� (Ÿ�̸� -> Ÿ��)
        }
        else    // ��� ���� �ƴϾ��ٸ� ��� ����
        {
            basicTimeSpeedBool = true;
            tower.remainComunicationTime = timeSpeedRecoverTimer;   // �ش� Ÿ���� ���� ��� �ð��� �� Ÿ�̸ӿ� ��� (Ÿ�� -> Ÿ�̸�)
        }
        
    }

    public void TowerTime(float addTime)    // Ÿ�̸ӿ� ���� �ð� ��ȭ(����ž �Ǽ� �� ����, ö�� �� ȸ��)
    {
        timer -= addTime;
        ShowTimer();
    }

    public void ShowTimer() // Ÿ�̸� �ؽ�Ʈ�� ������
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
        timer -= timeSpeed * Time.deltaTime;    // Ÿ�̸� �ð� �帧
        ShowTimer();    // Ÿ�̸� �ؽ�Ʈ�� �����ֱ�

        if (timer <= 0) // �ð� �� �Ǹ� ���� ����
        {
            timer = 0.0f;
            ShowTimer();
            gameWinManager.TimeCheck();
        }

        if (!basicTimeSpeedBool && nowTower.remainComunicationTime >= 0.0f) // ��� ���� ���
        {
            nowTower.gauge.transform.GetChild(2).gameObject.GetComponent<Image>().fillAmount = 1 - timeSpeedRecoverTimer / 20.0f; // ��� ������, ��ġ�� ���� �ö�, �ִ밡 1.0, �ּ� 0.0

            timeSpeedRecoverTimer -= Time.deltaTime;    // ����ž�� ���� ��� ���� �ð�

            if (timeSpeedRecoverTimer <= 0.0f)  // ��� ���̾��ٰ� �ش� Ÿ���� ��� ���� �ð��� �� �Ǹ� ��� ����
            {
                RadioComunicationTime(1.0f, nowTower);
            }
        }

    }
}
