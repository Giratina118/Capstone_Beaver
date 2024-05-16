using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpyBeaverAction : MonoBehaviour
{
    public InventorySlotGroup inventorySlotGroup;   // �κ��丮
    public TowerInfo towerInfo;     // Ÿ�� ����(�Ǽ��Ҷ� ���)
    public TimerManager timerManager;   // Ÿ�̸�(Ÿ�� �Ǽ� ��)
    private TowerInfo nowTower = null;  // ���� ��ġ�� Ÿ��(����� ����)

    public Button buildComunicationButton;  // ž �Ǽ� <-> ��� ��ư
    public GameObject towerGaugePrefab;     // Ÿ�� ��� ������
    public Transform cnavasGaugesTransform; // ��� �������� �θ� ��ġ

    public GameWinManager gameWinManager;   // �¸�(Ÿ�� ���� �� �̻� �ʵ忡 ���ÿ� ������ ���)
    public Transform towerParentTransfotm;  // Ÿ���� �θ�

    private bool spyBeaverEscape = false;   // ������ ��� ��� Ż�� ���� ����(Ư�� �ð��� �Ǿ�����)
    public bool useEmergencyEscape = false; // ������ ��� ��� Ż�� ��� ����(�̹� �� �� ����ߴ���)
    public GameObject escapePrisonButton;   // ���� Ż�� ��ư


    [SerializeField]
    private float decreaseTime = 30.0f; // ����ž �Ǽ� �� ��� �پ��� �ð�
    private bool onTower = false;   // Ÿ�� ���� �ִ��� ����(Ÿ�� �Ǽ��� ����� ��Ȳ�� �����ϱ� ����)


    private void OnTriggerEnter2D(Collider2D collision) // Ÿ�� ���� �ִ��� Ȯ��, ���� �ִٸ� Ÿ�� ���� ��������
    {
        if (collision.gameObject.tag == "Tower")    // Ÿ�� ���� ������
        {
            if (onTower)    // �ϳ��� Ÿ������ ������ ����� ���� �ٸ� Ÿ���� ����� ��� �� ���� ����ִ� Ÿ���� ��� ����
            {
                nowTower.gauge.SetActive(false);
                timerManager.RadioComunicationTime(1.0f, nowTower);
            }

            // Ÿ�� ���� ������ �Ǽ� ��ư�� ��� ��ư���� �ٲ�, ���߿� text�� �ٲٴ� ��� ���ڰ� ���� �̹����� button���� �ٲ�� �ϱ�
            buildComunicationButton.gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text = "Radio\nComuni\ncation";   

            // ���� ���� Ÿ���� Ÿ�� ���� ����
            onTower = true;
            nowTower = collision.gameObject.GetComponent<TowerInfo>();
            nowTower.gauge.SetActive(true);
            timerManager.SetTimeSpeedRecoverTimer(nowTower.remainComunicationTime);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)  // Ÿ������ ����� ����ϴ��� �ڵ����� ����
    {
        if (collision.gameObject.tag == "Tower")
        {
            buildComunicationButton.gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text = "Build\nTower";    // ���߿� text�� �ٲٴ� ��� ���ڰ� ���� �̹����� button���� �ٲ�� �ϱ�

            // ��� �ִ� Ÿ�� ���� �����
            onTower = false;
            nowTower.gauge.SetActive(false);
            timerManager.RadioComunicationTime(1.0f, nowTower);
        }
    }

    public void OnClickBuildOrRadioComunicationButton()    // Ÿ�� �Ǽ� �Ǵ� ��� ��ư
    {
        if (onTower)    // Ÿ�� ���� ������ ���
        {
            if (nowTower.remainComunicationTime > 0.0f) // Ÿ������ ���
            {
                timerManager.RadioComunicationTime(2.0f, nowTower); // �ð� �پ��� �ӵ� ������
            }
        }
        else if (inventorySlotGroup.RequireResourceCountCheck(towerInfo.requiredResourceOfTowers))  // Ÿ�� ���� ��ᰡ ����ϸ� Ÿ�� �Ǽ�
        {
            inventorySlotGroup.UseResource(towerInfo.requiredResourceOfTowers); // ��� ���
            inventorySlotGroup.NowResourceCount();  // �κ��丮 ���� ����

            GameObject newTower = GameObject.Instantiate(towerInfo.gameObject, towerParentTransfotm);   // Ÿ�� ����
            newTower.transform.position = this.transform.position;  // Ÿ�� ��ġ ����
            timerManager.TowerTime(decreaseTime);   // Ÿ�� �Ǽ��� ���� �ð� ����

            GameObject newGauge = Instantiate(towerGaugePrefab, cnavasGaugesTransform); // ������ ����
            newTower.GetComponent<TowerInfo>().gauge = newGauge;    // Ÿ���� �������� ����

            gameWinManager.TowerCountCheck();   // Ÿ���� ���� �� �̻� ���������� Ȯ��
        }
    }

    void Start()
    {
        inventorySlotGroup = GameObject.Find("InventorySlots").gameObject.GetComponent<InventorySlotGroup>();
        //buildComunicationButton
    }

    void Update()
    {
        if (!spyBeaverEscape && !useEmergencyEscape && timerManager.GetNowTime() <= 120.0f) // ������ ����� ��� Ż�� ���� üũ
        {
            spyBeaverEscape = true;
            escapePrisonButton.SetActive(true);
        }
    }
}
