using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PrisonManager : MonoBehaviour
{
    private float prisonTimer = 20.0f;  // 감옥 타이머
    private bool inPrison = false;      // 감옥 안에 있는지 여부
    private int caughtCount = 0;        // 감옥에 갇힌 횟수
    public float inPrisonTime = 30.0f;  // 감옥 타이머의 초기 시간
    public MapImages mapImage;      // 탈출 시 사용하는 지도
    //public bool escapePosSelect = false;
    public TMP_Text prisonTimerText;    // 감옥 타이머 표시할 텍스트
    public int keyCount = 0;    // 현재 가지고 있는 열쇠 수
    public InventorySlotGroup inventorySlotGroup;   // 인벤토리(열쇠 사용시)
    public Button escapePrisonButton;   // 탈출 버튼
    private Transform prisonTransform;  // 감옥 위치

    public void ShowPrisonTimer()   // 감옥 타이머 보여주기
    {
        prisonTimerText.text = Mathf.FloorToInt(prisonTimer / 60.0f).ToString() + " : ";
        if (prisonTimer % 60.0f < 10)
            prisonTimerText.text += "0";
        prisonTimerText.text += Mathf.FloorToInt(prisonTimer % 60.0f).ToString();
    }

    [PunRPC]
    public void CaughtByRope()  // 로프에 잡혔을 때
    {
        this.gameObject.transform.position = prisonTransform.position; // 맞은 플레리어를 감옥으로 위프
        //prisonTimerText.gameObject.SetActive(true);
        caughtCount++;  // 잡힌 횟수 증가
        prisonTimer = inPrisonTime + (caughtCount - 1) * 10.0f; // 잡힌 횟수에 따른 투옥 시간 설정
        inPrison = true;
    }

    public void EscapePrison(bool clickButton)  // 감옥 탈출
    {
        if (!inPrison)  // 감옥 안에 있을때만 사용
            return;

        inPrison = false;
        if (clickButton)    // 열쇠 혹은 스파이 능력으로 탈출 시
        {
            if (keyCount <= 0)  // 열쇠가 없는 상황에서 스파이 능력으로 탈출
            {
                this.gameObject.GetComponent<SpyBeaverAction>().useEmergencyEscape = true;
                //escapePrisonButton.gameObject.SetActive(false);
                escapePrisonButton.enabled = false;
                Color escapeButtonColor = escapePrisonButton.GetComponent<Image>().color;
                escapeButtonColor.a = 0.5f;
                escapePrisonButton.GetComponent<Image>().color = escapeButtonColor;
                Debug.Log(this.gameObject.name + " 스파이가 감옥에서 탈출했습니다.");
                this.gameObject.GetComponent<SpriteRenderer>().color = Color.black;
            }
            else    // 열쇠로 탈출
            {
                inventorySlotGroup.UseItem(5, 1, false);
            }
            keyCount--;
        }

        // 시간 다 되어서 탈출하는 것과 열쇠, 스파이 능력으로 탈출하는 것에 공통으로 적용
        mapImage.gameObject.transform.localPosition = Vector3.zero;
        //mapImage.gameObject.SetActive(true);
        //prisonTimerText.gameObject.SetActive(false);
        prisonTimerText.text = "";
    }

    void Start()
    {
        if (!GetComponent<PhotonView>().IsMine)
            return;

        prisonTransform = GameObject.Find("Prison").transform;
        mapImage = GameObject.Find("MapImagePieces").GetComponent<MapImages>();
        prisonTimerText = GameObject.Find("PrisonTimer").gameObject.GetComponent<TMP_Text>();
        prisonTimerText.text = "";
        inventorySlotGroup = GameObject.Find("InventorySlots").gameObject.GetComponent<InventorySlotGroup>();
        escapePrisonButton = GameObject.Find("EscapePrisonButton").gameObject.GetComponent<Button>();
        escapePrisonButton.onClick.AddListener(() => EscapePrison(true));
        //escapePrisonButton.gameObject.SetActive(false);
        escapePrisonButton.enabled = false;
    }

    void Update()
    {
        if (inPrison)   // 감옥 타이머
        {
            prisonTimer -= Time.deltaTime;
            ShowPrisonTimer();

            if (prisonTimer <= 0.0f)    // 시간 다 되면 풀려남
            {
                EscapePrison(false);
            }
        }

    }
}
