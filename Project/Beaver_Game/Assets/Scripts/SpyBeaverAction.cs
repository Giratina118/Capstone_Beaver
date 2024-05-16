using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpyBeaverAction : MonoBehaviour
{
    public InventorySlotGroup inventorySlotGroup;   // 인벤토리
    public TowerInfo towerInfo;     // 타워 정보(건설할때 사용)
    public TimerManager timerManager;   // 타이머(타워 건설 시)
    private TowerInfo nowTower = null;  // 현재 위치한 타워(통신을 위해)

    public Button buildComunicationButton;  // 탑 건설 <-> 통신 버튼
    public GameObject towerGaugePrefab;     // 타워 통신 게이지
    public Transform cnavasGaugesTransform; // 통신 게이지의 부모 위치

    public GameWinManager gameWinManager;   // 승리(타워 일정 수 이상 필드에 동시에 존재할 경우)
    public Transform towerParentTransfotm;  // 타워의 부모

    private bool spyBeaverEscape = false;   // 스파이 비버 긴급 탈출 가능 여부(특정 시간이 되었는지)
    public bool useEmergencyEscape = false; // 스파이 비버 긴급 탈출 사용 여부(이미 한 번 사용했는지)
    public GameObject escapePrisonButton;   // 감옥 탈출 버튼


    [SerializeField]
    private float decreaseTime = 30.0f; // 전파탑 건설 시 즉시 줄어드는 시간
    private bool onTower = false;   // 타워 위에 있는지 여부(타워 건설과 통신의 상황을 구분하기 위함)


    private void OnTriggerEnter2D(Collider2D collision) // 타워 위에 있는지 확인, 위에 있다면 타워 정보 가져오기
    {
        if (collision.gameObject.tag == "Tower")    // 타워 위에 있을때
        {
            if (onTower)    // 하나의 타워에서 완전히 벗어나기 전에 다른 타워를 밟았을 경우 이 전에 밟고있던 타워의 통신 멈춤
            {
                nowTower.gauge.SetActive(false);
                timerManager.RadioComunicationTime(1.0f, nowTower);
            }

            // 타워 위에 있으면 건설 버튼을 통신 버튼으로 바꿈, 나중에 text를 바꾸는 대신 글자가 써진 이미지만 button에서 바뀌게 하기
            buildComunicationButton.gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text = "Radio\nComuni\ncation";   

            // 지금 밟은 타워로 타워 정보 설정
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

            // 밟고 있던 타워 정보 지우기
            onTower = false;
            nowTower.gauge.SetActive(false);
            timerManager.RadioComunicationTime(1.0f, nowTower);
        }
    }

    public void OnClickBuildOrRadioComunicationButton()    // 타워 건설 또는 통신 버튼
    {
        if (onTower)    // 타워 위에 있으면 통신
        {
            if (nowTower.remainComunicationTime > 0.0f) // 타워에서 통신
            {
                timerManager.RadioComunicationTime(2.0f, nowTower); // 시간 줄어드는 속도 빠르게
            }
        }
        else if (inventorySlotGroup.RequireResourceCountCheck(towerInfo.requiredResourceOfTowers))  // 타워 만들 재료가 충분하면 타워 건설
        {
            inventorySlotGroup.UseResource(towerInfo.requiredResourceOfTowers); // 재료 사용
            inventorySlotGroup.NowResourceCount();  // 인벤토리 상태 갱신

            GameObject newTower = GameObject.Instantiate(towerInfo.gameObject, towerParentTransfotm);   // 타워 전설
            newTower.transform.position = this.transform.position;  // 타워 위치 조정
            timerManager.TowerTime(decreaseTime);   // 타워 건설에 따른 시간 감소

            GameObject newGauge = Instantiate(towerGaugePrefab, cnavasGaugesTransform); // 게이지 생성
            newTower.GetComponent<TowerInfo>().gauge = newGauge;    // 타워와 게이지를 연결

            gameWinManager.TowerCountCheck();   // 타워가 일정 수 이상 지어졌는지 확인
        }
    }

    void Start()
    {
        inventorySlotGroup = GameObject.Find("InventorySlots").gameObject.GetComponent<InventorySlotGroup>();
        //buildComunicationButton
    }

    void Update()
    {
        if (!spyBeaverEscape && !useEmergencyEscape && timerManager.GetNowTime() <= 120.0f) // 스파이 비버의 긴급 탈출 조건 체크
        {
            spyBeaverEscape = true;
            escapePrisonButton.SetActive(true);
        }
    }
}
