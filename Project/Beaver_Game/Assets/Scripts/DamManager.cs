using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Resources;
using UnityEngine;
using UnityEngine.UI;

public class DamManager : MonoBehaviourPunCallbacks
{
    public Sprite[] damSprite = new Sprite[4];      // 댐 이미지들
    private int degreeOfConstruction = 0;

    public int[] requiredResources = new int[4];    // 댐 건설에 필요한 자원 수 저장, 0: 나무, 1: 진흙, 2: 돌, 3: 강철
    public int totalDamRequiredResource = 20;       // 댐 만드는데 필요한 총 자원의 수, 이 숫자를 4개의 자원으로 랜덤으로 나눈다.
    public InventorySlotGroup inventorySlotGroup;   // 개인의 인벤토리
    public InventorySlotGroup storageInventorySlotGroup;    // 창고의 인벤토리
    public GameObject gaugePrefab;          // 댐 건설 게이지 프리팹
    private GameObject buildGauge;          // 프리팹으로 생성한 댐 건설 게이지를 저장
    public Transform cnavasGaugesTransform; // 게이지의 부모 위치(UI로 하기 위함)
    public Animator damBuildAnim;

    [SerializeField]
    private float gaugePlusYPos = 0.8f; // 게이지 위치(댐 중심으로부터 조금 위쪽에 생기도록)
    public bool buildNow = false;       // 댐 건설 중인지 여부
    public bool buildComplete = false;  // 댐 건설 완료했는지 여부

    public float obstract = 0.0f;   // 스파이 비버의 방해 속도
    public float accelerate = 0.0f; // 일반 비버의 가속 속도
    public GameWinManager gameWinManager;   // 댐 모두 건설 시 승리하게 하도록

    public float dameGaugeSpeedRate = 1.0f;    // 댐 게이지차는 속도 배율, 작업모 착용으로 변경


    [PunRPC]
    public void ObstructBuild(bool turnOn) // 댐 건설 방해(스파이 전용)
    {
        if (turnOn)
        {
            obstract = -0.02f * dameGaugeSpeedRate; // 방해 시 감속 속도
        }
        else
        {
            obstract = 0.0f;
        }
    }

    [PunRPC]
    public void AccelerateBuild(bool turnOn) // 댐 건설 가속(일반 비버 전용)
    {
        if (turnOn)
        {
            accelerate = 0.01f * dameGaugeSpeedRate; // 가속 시 가속 속도
            GetComponent<AudioSource>().Play();
        }
        else
        {
            accelerate = 0.0f;
            GetComponent<AudioSource>().Stop();
        }
        damBuildAnim.gameObject.SetActive(turnOn);
        damBuildAnim.SetBool("accelerate", turnOn);
    }

    public void DamCreate() // 창고 -> 인벤토리 순으로 자원 소비하여 댐 건설
    {
        bool damCreateBool = true;
        for (int i = 0; i < 4; i++) // 댐을 만들 자원이 충분한지 체크
        {
            if (requiredResources[i] > inventorySlotGroup.resourceCountInts[i] + storageInventorySlotGroup.resourceCountInts[i])
            {
                damCreateBool = false;
                break;
            }
        }

        if (damCreateBool)
        {
            int[] remainNum = requiredResources;    // 댐을 만드는데 필요한 자원

            for (int i = 0; i < 4; i++) // 창고에서 자원 빼는 부분
            {
                if (requiredResources[i] > storageInventorySlotGroup.resourceCountInts[i])  // 필요 자원이 창고에 저장된 자원보다 많을 경우
                {
                    storageInventorySlotGroup.itemSlots[i].gameObject.transform.GetChild(0).gameObject.GetPhotonView().RPC("ShowItemCount", RpcTarget.All, -storageInventorySlotGroup.resourceCountInts[i]);    // 창고에서 자원 빼기
                    remainNum[i] -= storageInventorySlotGroup.resourceCountInts[i]; // 남은 더 지불해야할 자원 수
                }
                else   // 필요 자원이 창고에 저장된 자원보다 적거나 같을 경우
                {
                    storageInventorySlotGroup.itemSlots[i].gameObject.transform.GetChild(0).gameObject.GetPhotonView().RPC("ShowItemCount", RpcTarget.All, -remainNum[i]);  // 창고에서 자원 빼기
                    remainNum[i] = 0;
                }
            }
            storageInventorySlotGroup.gameObject.GetPhotonView().RPC("NowResourceCount", RpcTarget.All);    // 창고의 자원 수 갱신

            inventorySlotGroup.UseResource(remainNum);  // 인벤토리의 자원 사용
            inventorySlotGroup.NowResourceCount();  // 인벤토리의 자원 수 갱신

            this.gameObject.GetPhotonView().RPC("TurnOnBuildGauge", RpcTarget.All); // 건설 게이지 활성화
        }
    }

    [PunRPC]
    public void TurnOnBuildGauge()  // 누군가 댐 건설을 하면 건설 게이지 활성화, 건설중 상태로 변경
    {
        buildGauge.SetActive(true);
        buildNow = true;
    }

    [PunRPC]
    public void SetDamCreateResource(int randomBoundary0, int randomBoundary1, int randomBoundary2)
    {
        requiredResources[0] = randomBoundary0;
        requiredResources[1] = randomBoundary1 - randomBoundary0;
        requiredResources[2] = randomBoundary2 - randomBoundary1;
        requiredResources[3] = totalDamRequiredResource - randomBoundary2;
    }

    public void DamRandomPrice()
    {
        List<int> randomBoundary = new List<int>(); // 댐 건설하는데 필요한 자원 랜덤으로 정하기

        for (int i = 0; i < 3; i++)
        {
            randomBoundary.Add(Random.Range(0, totalDamRequiredResource + 1));
        }
        randomBoundary.Sort();

        this.gameObject.GetPhotonView().RPC("SetDamCreateResource", RpcTarget.All, randomBoundary[0], randomBoundary[1], randomBoundary[2]);
    }

    void Start()
    {
        buildGauge = Instantiate(gaugePrefab, cnavasGaugesTransform);
        buildGauge.SetActive(false);
    }

    void Update()
    {
        if (buildNow)
        {
            buildGauge.transform.position = Camera.main.WorldToScreenPoint(new Vector3(this.transform.position.x, this.transform.position.y + gaugePlusYPos, 0.0f));    // 게이지 위치 설정(UI라서)
            buildGauge.transform.GetChild(2).gameObject.GetComponent<Image>().fillAmount += Time.deltaTime * (0.01f + accelerate + obstract); // 현재는 100초, 수치 조정 가능

            float constructFillAmount = buildGauge.transform.GetChild(2).gameObject.GetComponent<Image>().fillAmount;
            int constructDegree = (Mathf.FloorToInt(constructFillAmount * 100.0f) + 50) / 50;

            if (constructDegree != degreeOfConstruction)
            {
                this.gameObject.GetComponent<SpriteRenderer>().sprite = damSprite[constructDegree];
                degreeOfConstruction = constructDegree;
            }

            if (buildGauge.transform.GetChild(2).gameObject.GetComponent<Image>().fillAmount >= 1.0f)// 게이지가 다 찼을 경우
            {
                // 게이지 비활성화
                buildNow = false;
                buildComplete = true;
                buildGauge.SetActive(false);
                gameWinManager.DamCountCheck();
                GetComponent<PhotonView>().RPC("AccelerateBuild", RpcTarget.All, false);
            }
            else if (buildGauge.transform.GetChild(2).gameObject.GetComponent<Image>().fillAmount <= 0.0f)  // 댐이 방해에 의해 게이지가 다 떨어졌을 경우 건설 취소
            {
                buildNow = false;
                buildGauge.SetActive(false);
                degreeOfConstruction = 0;
                this.gameObject.GetComponent<SpriteRenderer>().sprite = damSprite[degreeOfConstruction];
            }
        }
    }
}
