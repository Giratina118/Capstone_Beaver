using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScriptAdapter : MonoBehaviour
{
    /*
    // InMapAction
    private GameObject productionImage;  // 아이템 제작 화면
    private GetResourceManager getResourceManager;   // 자원 채취 화면
    private Button actionButton;     // 액션 버튼
    private InventorySlotGroup storageSlotGroup; // 창고 인벤토리

    // PlayerMove
    private RopeManager ropeManager; // 로프 예비 선
    Animator animator;  // 비버 애니메이션

    // SpyBeaverAction
    private InventorySlotGroup inventorySlotGroup;   // 인벤토리
    private TimerManager timerManager;   // 타이머(타워 건설 시)
    private Button buildComunicationButton;  // 탑 건설 <-> 통신 버튼
    private Transform cnavasGaugesTransform; // 통신 게이지의 부모 위치
    private GameWinManager gameWinManager;   // 승리(타워 일정 수 이상 필드에 동시에 존재할 경우)
    private Transform towerParentTransfotm;  // 타워의 부모
    private GameObject escapePrisonButton;   // 감옥 탈출 버튼

    // SpyBoolManager
    private Button buildTowerButton;
    SpyBeaverAction spyAction;

    // DemolishTower
    private Button demolishTowerButton;  // 타워 철거 버튼
    private GetResourceManager getResourceManager;   // 타워 철거 후 자원 돌려받기 위함
    private TimerManager timerManager;   // 타워 철거에 따른 시간 복구 위함

    // PrisonManager
    private RectTransform mapImage;      // 탈출 시 사용하는 지도
    private TMP_Text prisonTimerText;    // 감옥 타이머 표시할 텍스트
    private InventorySlotGroup inventorySlotGroup;   // 인벤토리(열쇠 사용시)
    private Button escapePrisonButton;   // 탈출 버튼

    // RopeManager
    private Button throwRopeButton;  // 로프 던지기 버튼
    */


    private MapImages mapImages;
    private ItemEquipManager itemEquipManager;
    private PutDownItem putDownItem;
    private ThrowAwayItem throwAwayItem;
    private InventorySlotGroup inventorySlotGroup;


    void Start()
    {
        /*
        // InMapAction
        productionImage = GameObject.Find("ProductionImage");
        getResourceManager = GameObject.Find("GetResourceBackground").GetComponent<GetResourceManager>();
        actionButton = GameObject.Find("ActionButton").GetComponent<Button>();
        storageSlotGroup = GameObject.Find("StorageSlots").GetComponent<InventorySlotGroup>();
        //actionButton.onClick.AddListener(OnClickActionButton);

        // PlayerMove
        animator = GetComponent<Animator>();
        ropeManager = this.transform.GetChild(0).GetComponent<RopeManager>();

        // SpyBeaverAction
        inventorySlotGroup = GameObject.Find("InventorySlots").GetComponent<InventorySlotGroup>();
        timerManager = GameObject.Find("Timer").GetComponent<TimerManager>();
        buildComunicationButton = GameObject.Find("BuildTowerButton").GetComponent<Button>();
        cnavasGaugesTransform = GameObject.Find("Gauges").transform;
        gameWinManager = GameObject.Find("GameOverManager").GetComponent<GameWinManager>();
        towerParentTransfotm = GameObject.Find("Towers").transform;
        escapePrisonButton = GameObject.Find("EscapePrisonButton");

        // SpyBoolManager
        spyAction = GetComponent<SpyBeaverAction>();
        buildTowerButton = GameObject.Find("SpyChangeButton").GetComponent<Button>();

        // DemolishTower
        demolishTowerButton = GameObject.Find("DemolishTowerButton").GetComponent<Button>();
        getResourceManager = GameObject.Find("GetResourceBackground").GetComponent<GetResourceManager>();
        timerManager = GameObject.Find("Timer").GetComponent<TimerManager>();

        // PrisonManager
        mapImage = GameObject.Find("MapImages").GetComponent<RectTransform>();
        prisonTimerText = GameObject.Find("PrisonTimer").GetComponent<TMP_Text>();
        inventorySlotGroup = GameObject.Find("InventorySlots").GetComponent<InventorySlotGroup>();
        escapePrisonButton = GameObject.Find("EscapePrisonButton").GetComponent<Button>();

        // RopeManager
        throwRopeButton = GameObject.Find("ThrowRopeButton").GetComponent<Button>();
        */


        mapImages = GameObject.Find("MapImagePieces").GetComponent<MapImages>();
        mapImages.player = this.gameObject;
        itemEquipManager = GameObject.Find("EquipSlots").GetComponent<ItemEquipManager>();
        itemEquipManager.player = this.gameObject;
        putDownItem = GameObject.Find("PutDownSolt").GetComponent<PutDownItem>();
        putDownItem.playerPos = this.gameObject.transform;
        throwAwayItem = GameObject.Find("ThrowAwaySlot").GetComponent<ThrowAwayItem>();
        throwAwayItem.prisonManager = this.gameObject.GetComponent<PrisonManager>();

        inventorySlotGroup = GameObject.Find("InventorySlots").GetComponent<InventorySlotGroup>();
        inventorySlotGroup.spyBoolManager = this.gameObject.GetComponent<SpyBoolManager>();



    }

    void Update()
    {
        
    }
}
