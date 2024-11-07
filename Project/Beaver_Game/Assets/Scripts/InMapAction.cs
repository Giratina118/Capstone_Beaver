using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InMapAction : MonoBehaviour
{
    public GameObject productionImage;  // 아이템 제작 화면
    public GetResourceManager getResourceManager;   // 자원 채취 화면
    public ButtonIconManager btnManager;    // 버튼 매니저
    private string tagName = "";    // 현재 접하고 있는 곳의 태그의 이름 저장
    private GameObject damGameObject = null;    // 현재 위치해 있는 댐
    private Transform ResourcePos;  // 현재 접하고있는 곳의 위치
    public InventorySlotGroup storageSlotGroup; // 창고 인벤토리
    public CCTVManager cctvManager; // cctv 관리

    public GameObject damUI;
    GameObject onTriggerObject = null;


    private void OnTriggerEnter2D(Collider2D collision) // 버튼 활성화
    {
        if (!this.GetComponent<PhotonView>().IsMine)
            return;

        // 액션 버튼으로 상호작용 할 수 있는 곳에 위치했을 경우
        if (collision.gameObject.transform.tag == "Forest" || collision.gameObject.transform.tag == "Mud" || collision.gameObject.transform.tag == "Stone" || collision.gameObject.transform.tag == "Dump" 
            || collision.gameObject.transform.tag == "Storage" || collision.gameObject.transform.tag == "Dam" || collision.gameObject.tag == "ProductionCenter"
            || collision.gameObject.tag == "CCTV1" || collision.gameObject.tag == "CCTV2")
        {
            // 액션 버튼 활성화 및 강조, 나중에는 위치한 곳에 따라 버튼 그림 바뀌게
            if (collision.gameObject.transform.tag == "Storage")
            {
                btnManager.ChangeActionButtonIcon(3);
            }
            else if (collision.gameObject.tag == "ProductionCenter")
            {
                btnManager.ChangeActionButtonIcon(2);
            }
            else if (collision.gameObject.tag == "CCTV1" || collision.gameObject.tag == "CCTV2")
            {
                btnManager.ChangeActionButtonIcon(0);
            }
            else
            {
                btnManager.ChangeActionButtonIcon(1);
            }
            btnManager.actionButton.interactable = true;

            if (collision.gameObject.transform.tag == "Dam")    // 댐에 위치해있을 경우 해당 댐의 정보 저장
            {
                if (tagName == "Dam")
                    onTriggerObject.GetPhotonView().RPC("AccelerateBuild", RpcTarget.All, false);

                damGameObject = collision.gameObject;

                if (!damGameObject.GetComponent<DamManager>().buildComplete)    // 댐 완공 전일 경우에만 상호작용
                {
                    if (damGameObject.GetComponent<DamManager>().buildNow)  // 댐 건설 중이라면
                    {
                        btnManager.ChangeActionButtonIcon(7);   // 댐 건설 가속/감속 버튼
                    }
                    else    // 댐 건설 시작 전이라면
                    {
                        btnManager.ChangeActionButtonIcon(6);   // 댐 건설 버튼
                        for (int i = 0; i < 4; i++) // 댐 건설에 필요한 자원 수 표시
                        {
                            damUI.gameObject.SetActive(true);
                            damUI.transform.GetChild(i).GetChild(0).gameObject.GetComponent<TMP_Text>().text = damGameObject.GetComponent<DamManager>().requiredResources[i].ToString();
                        }
                    }
                }
                else
                {
                    btnManager.ChangeActionButtonIcon(8);   // 댐 완공
                }
            }
            else    // 댐이 아닐 경우 해당 위치 저장(자원 생성 시 해당 위치에 생성하기 위함)
            {
                ResourcePos = this.gameObject.transform;
            }

            tagName = collision.gameObject.tag; // 위치한 곳의 태그 저장
            onTriggerObject = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)  // 버튼 비활성화
    {
        if (!this.GetComponent<PhotonView>().IsMine)
            return;

        // 액션 버튼으로 상호작용 할 수 있는 곳에서 벗어났을 경우
        if (collision.gameObject.transform.tag == "Mud" || collision.gameObject.transform.tag == "Forest" || collision.gameObject.transform.tag == "Stone" || collision.gameObject.transform.tag == "Dump" 
            || collision.gameObject.transform.tag == "Storage" || collision.gameObject.transform.tag == "Dam" || collision.gameObject.tag == "ProductionCenter")
        {
            if (collision.gameObject.transform.tag == "Dam")    // 댐에 있었을 경우 댐 건설의 가속, 감속을 원래대로 없앰
            {
                if (this.gameObject.GetComponent<SpyBoolManager>().isSpy())
                {
                    damGameObject.GetPhotonView().RPC("ObstructBuild", RpcTarget.All, false);
                }
                else
                {
                    damGameObject.GetPhotonView().RPC("AccelerateBuild", RpcTarget.All, false);
                }
            }

            if (collision.gameObject == onTriggerObject)
            {
                btnManager.ChangeActionButtonIcon(0);   // 기본 버튼으로 되돌리기
                btnManager.actionButton.interactable = true;    // 버튼 비활성화

                tagName = "";
                damUI.gameObject.SetActive(false);
            }
        }
    }

    public void OnClickActionButton()   // 액션 버튼 클릭
    {
        if (!this.GetComponent<PhotonView>().IsMine)
            return;

        switch (tagName)
        {
            // 자원 채취하는 곳 위에 있을 경우에는 자원 채취 화면 띄우고 자원에 따라 설정하기
            case "Mud":
                getResourceManager.GetResourceActive(0, ResourcePos);   // 자원 정보 설정
                getResourceManager.gameObject.transform.localPosition = Vector3.zero;   // 자원 채취 화면 띄우기
                break;
            case "Forest":
                getResourceManager.GetResourceActive(1, ResourcePos);
                getResourceManager.gameObject.transform.localPosition = Vector3.zero;
                break;
            case "Stone":
                getResourceManager.GetResourceActive(2, ResourcePos);
                getResourceManager.gameObject.transform.localPosition = Vector3.zero;
                break;
            case "Dump":
                getResourceManager.GetResourceActive(3, ResourcePos);
                getResourceManager.gameObject.transform.localPosition = Vector3.zero;
                break;
            case "Storage":
                storageSlotGroup.gameObject.transform.parent.localPosition = Vector3.zero;  // 인벤토리 화면 띄우기
                storageSlotGroup.gameObject.transform.localPosition = new Vector3(-350.0f, 0.0f, 0.0f); // 창고 화면 띄우기
                break;
            case "Dam":
                if (!damGameObject.GetComponent<DamManager>().buildComplete)    // 댐 완공 전일 경우에만 상호작용
                {

                    if (damGameObject.GetComponent<DamManager>().buildNow)  // 댐 건설 중이라면
                    {
                        if (this.gameObject.GetComponent<SpyBoolManager>().isSpy()) // 스파이 비버는 댐 건설 방해
                        {
                            damGameObject.GetPhotonView().RPC("ObstructBuild", RpcTarget.All, true);
                        }
                        else    // 일반 비버는 댐 건설 가속
                        {
                            damGameObject.GetPhotonView().RPC("AccelerateBuild", RpcTarget.All, true);
                        }
                    }
                    else    // 댐 건설 시작 전이라면 댐 건설 시작
                    {
                        damGameObject.GetComponent<DamManager>().DamCreate();
                    }
                }
                break;
            case "ProductionCenter":
                productionImage.transform.localPosition = Vector3.zero; // 제작 화면 띄우기
                break;
            case "CCTV1":
                cctvManager.ShowCCTV(0);
                break;
            case "CCTV2":
                cctvManager.ShowCCTV(1);
                break;
            default:
                break;
        }
    }

    void Start()
    {
        if (!this.GetComponent<PhotonView>().IsMine)
            return;

        productionImage = GameObject.Find("ProductionImage");
        getResourceManager = GameObject.Find("GetResourceBackground").GetComponent<GetResourceManager>();
        btnManager = GameObject.Find("Buttons").GetComponent<ButtonIconManager>();
        storageSlotGroup = GameObject.Find("StorageSlots").GetComponent<InventorySlotGroup>();
        cctvManager = GameObject.Find("CCTVManager").GetComponent<CCTVManager>();

        btnManager.actionButton.onClick.AddListener(OnClickActionButton);
        
        damUI = GameObject.Find("DamUI");
        damUI.transform.localPosition = new Vector3(0.0f, 300.0f, 0.0f);
        damUI.SetActive(false);
    }
}
