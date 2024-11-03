using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProductionManager : MonoBehaviourPunCallbacks
{
    private ItemIndex itemIndex = null; // ������ ���
    private GameObject selectedItemGameObject;  // ������ ���� â���� ������ ���� �����ִ� ĭ
    private InventorySlotGroup inventorySlotGroup;  // �κ��丮
    private int nowItemNum = 0;     // ������ �������� ��ȣ
    public Transform productionCenter;  // ���۴� ��ġ


    public void SetSelectedItemmInfo(int itemNumber)    // ������ ������ ���� �����ֱ�
    {
        nowItemNum = itemNumber;    // ������ ��ȣ
        Transform itemInfoBox = selectedItemGameObject.transform.GetChild(0);
        Image itemIconImage = itemInfoBox.GetChild(0).gameObject.GetComponent<Image>();

        itemIconImage.sprite = itemIndex.items[itemNumber].GetComponent<SpriteRenderer>().sprite;   // ������ �̹��� �ֱ�
        itemIconImage.preserveAspect = true;    // ������ ���μ��� �� ���߱�
        itemInfoBox.GetChild(1).gameObject.GetComponent<TMP_Text>().text = itemIndex.items[itemNumber].itemName;   // ������ �̸�
        itemInfoBox.GetChild(2).gameObject.GetComponent<TMP_Text>().text = itemIndex.items[itemNumber].itemInformation.Replace("\\n", "\n"); ;    // ������ ����(����)

        for (int i = 0; i < 4; i++) // ���ۿ� �ʿ��� ��� �� ǥ��
        {
            selectedItemGameObject.transform.GetChild(1).GetChild(i + 1).GetChild(1).GetComponent<TMP_Text>().text = " " + inventorySlotGroup.resourceCountInts[i] + " / " + itemIndex.items[itemNumber].requiredResourceOfItem[i];
        }
    }

    public void OnClickCreateItemButton()   // ������ ����
    {
        if (inventorySlotGroup.RequireResourceCountCheck(itemIndex.items[nowItemNum].requiredResourceOfItem))   // ��ᰡ ����ϴٸ�
        {
            inventorySlotGroup.UseResource(itemIndex.items[nowItemNum].requiredResourceOfItem); // ��� ������ ���
            inventorySlotGroup.NowResourceCount();  // ���� �κ��丮�� ��� ����
            SetSelectedItemmInfo(nowItemNum);   // ���õ� �������� ���ŵ� ������ ���� �ٽ� �����ֱ�

            GameObject newItem = PhotonNetwork.Instantiate(itemIndex.items[nowItemNum].gameObject.name, productionCenter.position, Quaternion.identity);   // ������ ������ ����
        }
    }


    void Start()
    {
        itemIndex = GameObject.Find("ItemManager").GetComponent<ItemIndex>();
        selectedItemGameObject = GameObject.Find("SelectedItemBackground");
        inventorySlotGroup = GameObject.Find("InventorySlots").GetComponent<InventorySlotGroup>();
    }
}
