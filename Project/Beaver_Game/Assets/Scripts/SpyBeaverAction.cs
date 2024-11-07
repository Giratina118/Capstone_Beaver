using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpyBeaverAction : MonoBehaviourPunCallbacks
{
    public InventorySlotGroup inventorySlotGroup;   // �κ��丮
    public TowerInfo towerInfo;     // Ÿ�� ����(�Ǽ��Ҷ� ���)
    public TimerManager timerManager;   // Ÿ�̸�(Ÿ�� �Ǽ� ��)
    private TowerInfo nowTower = null;  // ���� ��ġ�� Ÿ��(����� ����)

    public ButtonIconManager btnManager;
    public GameObject towerGaugePrefab;     // Ÿ�� ��� ������
    public Transform cnavasGaugesTransform; // ��� �������� �θ� ��ġ

    public GameWinManager gameWinManager;   // �¸�(Ÿ�� ���� �� �̻� �ʵ忡 ���ÿ� ������ ���)
    public Transform towerParentTransfotm;  // Ÿ���� �θ�

    public bool spyBeaverEscape = false;   // ������ ��� ��� Ż�� ���� ����(Ư�� �ð��� �Ǿ�����)
    public bool useEmergencyEscape = false; // ������ ��� ��� Ż�� ��� ����(�̹� �� �� ����ߴ���)

    [SerializeField]
    private float decreaseTime = 30.0f; // ����ž �Ǽ� �� ��� �پ��� �ð�
    private bool onTower = false;   // Ÿ�� ���� �ִ��� ����(Ÿ�� �Ǽ��� ����� ��Ȳ�� �����ϱ� ����)


    private void OnTriggerEnter2D(Collider2D collision) // Ÿ�� ���� �ִ��� Ȯ��, ���� �ִٸ� Ÿ�� ���� ��������
    {
        if (!this.GetComponent<PhotonView>().IsMine || !this.enabled)    // �ڽ��� ĳ���͸� �����̵���
        {
            return;
        }

        if (collision.gameObject.tag == "Tower")    // Ÿ�� ���� ������
        {
            if (onTower)    // �ϳ��� Ÿ������ ������ ����� ���� �ٸ� Ÿ���� ����� ��� �� ���� ����ִ� Ÿ���� ��� ����
            {
                nowTower.gauge.SetActive(false);
                timerManager.gameObject.GetPhotonView().RPC("RadioComunicationTime", RpcTarget.MasterClient, 1.0f, nowTower.gameObject.GetPhotonView().ViewID);
            }
            btnManager.ChangeBuildTowerComunicationButton(true); 

            // ���� ���� Ÿ���� Ÿ�� ���� ����
            onTower = true;
            nowTower = collision.gameObject.GetComponent<TowerInfo>();
            nowTower.gauge.SetActive(true);

            timerManager.gameObject.GetPhotonView().RPC("SetTimeSpeedRecoverTimer", RpcTarget.MasterClient, nowTower.remainComunicationTime);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)  // Ÿ������ ����� ����ϴ��� �ڵ����� ����
    {
        if (!this.GetComponent<PhotonView>().IsMine || !this.enabled)    // �ڽ��� ĳ���͸� �����̵���
        {
            return;
        }

        if (collision.gameObject.tag == "Tower")
        {
            btnManager.ChangeBuildTowerComunicationButton(false);
            nowTower.SetTowerComunicationEffect(false);

            // ��� �ִ� Ÿ�� ���� �����
            onTower = false;
            nowTower.gauge.SetActive(false);

            timerManager.gameObject.GetPhotonView().RPC("RadioComunicationTime", RpcTarget.MasterClient, 1.0f, nowTower.gameObject.GetPhotonView().ViewID);
        }
    }

    public void OnClickBuildOrRadioComunicationButton()    // Ÿ�� �Ǽ� �Ǵ� ��� ��ư
    {
        if (!this.GetComponent<PhotonView>().IsMine)    // �ڽ��� ĳ���͸� �����̵���
        {
            return;
        }

        if (onTower)    // Ÿ�� ���� ������ ���
        {
            if (nowTower.remainComunicationTime >= 0.0f) // Ÿ������ ���
            {
                timerManager.gameObject.GetPhotonView().RPC("RadioComunicationTime", RpcTarget.MasterClient, 1.0f, nowTower.gameObject.GetPhotonView().ViewID);
                nowTower.SetTowerComunicationEffect(true);
                timerManager.gameObject.GetPhotonView().RPC("RadioComunicationTime", RpcTarget.MasterClient, 2.0f, nowTower.gameObject.GetPhotonView().ViewID); // �ð� �پ��� �ӵ� ������
            }
        }
        else if (inventorySlotGroup.RequireResourceCountCheck(towerInfo.requiredResourceOfTowers))  // Ÿ�� ���� ��ᰡ ����ϸ� Ÿ�� �Ǽ�
        {
            inventorySlotGroup.UseResource(towerInfo.requiredResourceOfTowers); // ��� ���
            inventorySlotGroup.NowResourceCount();  // �κ��丮 ���� ����

            GameObject newTower = PhotonNetwork.Instantiate("TowerPrefab", this.gameObject.transform.position, Quaternion.identity);    // Ÿ�� ����

            newTower.transform.position = this.transform.position;  // Ÿ�� ��ġ ����

            if (PhotonNetwork.IsMasterClient)   // Ÿ�� �Ǽ��� ���� �ð� ����
            {
                timerManager.TowerTime(decreaseTime);
            }
            else
            {
                timerManager.timerPhotonView.RPC("TowerTime", RpcTarget.MasterClient, decreaseTime);
            }

            GameObject newGauge = PhotonNetwork.Instantiate("GaugePrefab", Vector3.zero, Quaternion.identity);  // ������ ����
            newTower.GetComponent<PhotonView>().RPC("SetGauge", RpcTarget.All, newGauge.GetComponent<PhotonView>().ViewID); // Ÿ���� �������� ����

            gameWinManager.gameObject.GetPhotonView().RPC("TowerCountCheck", RpcTarget.All);    // Ÿ���� ���� �� �̻� ���������� Ȯ��
        }
    }

    void Start()
    {
        if (!this.gameObject.GetPhotonView().IsMine)
            return;

        inventorySlotGroup = GameObject.Find("InventorySlots").GetComponent<InventorySlotGroup>();
        timerManager = GameObject.Find("Timer").GetComponent<TimerManager>();
        cnavasGaugesTransform = GameObject.Find("Gauges").transform;
        gameWinManager = GameObject.Find("GameOverManager").GetComponent<GameWinManager>();
        towerParentTransfotm = GameObject.Find("Towers").transform;
        btnManager = GameObject.Find("Buttons").GetComponent<ButtonIconManager>();
        btnManager.buildTowerComunicationButton.onClick.AddListener(OnClickBuildOrRadioComunicationButton);
    }

    void Update()
    {
        if (!spyBeaverEscape && !useEmergencyEscape && timerManager.GetNowTime() <= 120.0f) // ������ ����� ��� Ż�� ���� üũ
        {
            spyBeaverEscape = true;
            btnManager.escapePrisonButton.interactable = true;
        }
        //if (nowTower != null && nowTower.comunicationEffect.gameObject.activeSelf && nowTower.remainComunicationTime <= 0)
        //    nowTower.SetTowerComunicationEffect(false);
    }
}
