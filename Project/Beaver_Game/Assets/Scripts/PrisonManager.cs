using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PrisonManager : MonoBehaviour
{
    private float prisonTimer = 20.0f;  // ���� Ÿ�̸�
    private bool inPrison = false;      // ���� �ȿ� �ִ��� ����
    private int caughtCount = 0;        // ������ ���� Ƚ��
    public float inPrisonTime = 30.0f;  // ���� Ÿ�̸��� �ʱ� �ð�
    public RectTransform mapImage;      // Ż�� �� ����ϴ� ����
    //public bool escapePosSelect = false;
    public TMP_Text prisonTimerText;    // ���� Ÿ�̸� ǥ���� �ؽ�Ʈ
    public int keyCount = 0;    // ���� ������ �ִ� ���� ��
    public InventorySlotGroup inventorySlotGroup;   // �κ��丮(���� ����)
    public Button escapePrisonButton;   // Ż�� ��ư

    public void ShowPrisonTimer()   // ���� Ÿ�̸� �����ֱ�
    {
        prisonTimerText.text = Mathf.FloorToInt(prisonTimer / 60.0f).ToString() + " : ";
        if (prisonTimer % 60.0f < 10)
            prisonTimerText.text += "0";
        prisonTimerText.text += Mathf.FloorToInt(prisonTimer % 60.0f).ToString();
    }

    public void CaughtByRope()  // ������ ������ ��
    {
        //prisonTimerText.gameObject.SetActive(true);
        caughtCount++;  // ���� Ƚ�� ����
        prisonTimer = inPrisonTime + (caughtCount - 1) * 10.0f; // ���� Ƚ���� ���� ���� �ð� ����
        inPrison = true;
    }

    public void EscapePrison(bool clickButton)  // ���� Ż��
    {
        if (!inPrison)  // ���� �ȿ� �������� ���
            return;

        inPrison = false;
        if (clickButton)    // ���� Ȥ�� ������ �ɷ����� Ż�� ��
        {
            if (keyCount <= 0)  // ���谡 ���� ��Ȳ���� ������ �ɷ����� Ż��
            {
                this.gameObject.GetComponent<SpyBeaverAction>().useEmergencyEscape = true;
                escapePrisonButton.gameObject.SetActive(false);
                Debug.Log(this.gameObject.name + " �����̰� �������� Ż���߽��ϴ�.");
                this.gameObject.GetComponent<SpriteRenderer>().color = Color.black;
            }
            else    // ����� Ż��
            {
                inventorySlotGroup.UseItem(5, 1);
            }
            keyCount--;
        }

        // �ð� �� �Ǿ Ż���ϴ� �Ͱ� ����, ������ �ɷ����� Ż���ϴ� �Ϳ� �������� ����
        mapImage.gameObject.transform.position = Vector3.zero;
        //mapImage.gameObject.SetActive(true);
        //prisonTimerText.gameObject.SetActive(false);
        prisonTimerText.text = "";
    }

    void Start()
    {
        
        mapImage = GameObject.Find("MapImages").GetComponent<RectTransform>();
        prisonTimerText = GameObject.Find("PrisonTimer").gameObject.GetComponent<TMP_Text>();
        prisonTimerText.text = "";
        inventorySlotGroup = GameObject.Find("InventorySlots").gameObject.GetComponent<InventorySlotGroup>();
        escapePrisonButton = GameObject.Find("EscapePrisonButton").gameObject.GetComponent<Button>();
        //escapePrisonButton.gameObject.SetActive(false);
    }

    void Update()
    {
        if (inPrison)   // ���� Ÿ�̸�
        {
            prisonTimer -= Time.deltaTime;
            ShowPrisonTimer();

            if (prisonTimer <= 0.0f)    // �ð� �� �Ǹ� Ǯ����
            {
                EscapePrison(false);
            }
        }

    }
}
