using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpyBeaverAction : MonoBehaviour
{
    public InventorySlotGroup inventorySlotGroup;
    public TowerInfo towerInfo;
    public TimerManager timerManager;
    private TowerInfo nowTower = null;

    public Button buildComunicationButton;
    public GameObject towerGaugePrefab;
    public Transform cnavasGaugesTransform;

    public GameWinManager gameWinManager;
    public Transform towerParentTransfotm;

    private bool spyBeaverEscape = false;
    public bool useEmergencyEscape = false;
    public GameObject escapePrisonButton;


    [SerializeField]
    private float decreaseTime = 30.0f;

    private bool onTower = false;

    // 나중에 타워 철거 관련 코드에 있는 트리거와 이 트리거를 한 곳에 합쳐서 타워 관리하기
    private void OnTriggerEnter2D(Collider2D collision) // 타워 위에 있는지 확인, 위에 있다면 타워 정보 가져오기
    {
        if (collision.gameObject.tag == "Tower")
        {
            if (onTower)    // 하나의 타워에서 완전히 벗어나기 전에 다른 타워를 밟았을 경우
            {
                nowTower.gauge.SetActive(false);
                timerManager.RadioComunicationTime(1.0f, nowTower);
            }

            buildComunicationButton.gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text = "Radio\nComuni\ncation";   // 나중에 text를 바꾸는 대신 글자가 써진 이미지만 button에서 바뀌게 하기

            onTower = true;
            nowTower = collision.gameObject.GetComponent<TowerInfo>();
            nowTower.gauge.SetActive(true);
            timerManager.SetTimeSpeedRecoverTimer(nowTower.remainComunicationTime);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)  // 타워에서 벗어나면 통신하던거 자동으로 종료
    {
        if (collision.gameObject.tag == "Tower")
        {
            buildComunicationButton.gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text = "Build\nTower";    // 나중에 text를 바꾸는 대신 글자가 써진 이미지만 button에서 바뀌게 하기

            onTower = false;
            nowTower.gauge.SetActive(false);
            timerManager.RadioComunicationTime(1.0f, nowTower);
        }
    }

    public void OnClickBuildOrRadioComunicationButton()    // 타워 건설 또는 통신 버튼
    {
        if (onTower)
        {
            if (nowTower.remainComunicationTime > 0.0f) // 타워에서 통신
            {
                timerManager.RadioComunicationTime(2.0f, nowTower);
            }
        }
        else if (inventorySlotGroup.RequireResourceCountCheck(towerInfo.requiredResourceOfTowers))  // 타워 만들 재료가 충분하면 건설
        {
            inventorySlotGroup.UseResource(towerInfo.requiredResourceOfTowers);
            inventorySlotGroup.NowResourceCount();

            GameObject newTower = GameObject.Instantiate(towerInfo.gameObject, towerParentTransfotm);
            newTower.transform.position = this.transform.position;
            timerManager.TowerTime(decreaseTime);

            GameObject newGauge = Instantiate(towerGaugePrefab, cnavasGaugesTransform);
            newTower.GetComponent<TowerInfo>().gauge = newGauge;

            gameWinManager.TowerCountCheck();
        }
    }

    void Start()
    {

    }

    void Update()
    {
        if (!spyBeaverEscape && !useEmergencyEscape && timerManager.GetNowTime() <= 120.0f)
        {
            spyBeaverEscape = true;
            escapePrisonButton.SetActive(true);
        }
    }
}
