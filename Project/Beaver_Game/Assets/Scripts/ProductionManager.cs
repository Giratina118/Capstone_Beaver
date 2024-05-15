using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProductionManager : MonoBehaviour
{
    private ItemIndex itemIndex = null;
    private GameObject selectedItemGameObject;
    private InventorySlotGroup inventorySlotGroup;
    private int nowItemNum = 0;
    public Transform productionCenter;

    public void SetSelectedItemmInfo(int itemNumber)
    {
        nowItemNum = itemNumber;

        selectedItemGameObject.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>().sprite = itemIndex.items[itemNumber].GetComponent<SpriteRenderer>().sprite;
        selectedItemGameObject.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>().preserveAspect = true;
        /*
        selectedItemGameObject.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>().color = itemIndex.items[itemNumber].GetComponent<SpriteRenderer>().color; // 나중에 아이템 그림 완성되고 나서는 빼기
        selectedItemGameObject.transform.GetChild(0).GetChild(0).transform.localRotation = itemIndex.items[itemNumber].gameObject.transform.localRotation;  // 나중에 아이템 그림 완성되고 나서는 빼기
        selectedItemGameObject.transform.GetChild(0).GetChild(0).transform.localScale = itemIndex.items[itemNumber].gameObject.transform.localScale;    // 나중에 아이템 그림 완성되고 나서는 빼기
        */
        selectedItemGameObject.transform.GetChild(0).GetChild(1).gameObject.GetComponent<TMP_Text>().text = itemIndex.items[itemNumber].itemName;
        selectedItemGameObject.transform.GetChild(0).GetChild(2).gameObject.GetComponent<TMP_Text>().text = itemIndex.items[itemNumber].itemInformation;

        for (int i = 0; i < 4; i++)
        {
            selectedItemGameObject.transform.GetChild(1).GetChild(i + 1).GetChild(1).GetComponent<TMP_Text>().text = " " + inventorySlotGroup.resourceCountInts[i] + " / " + itemIndex.items[itemNumber].requiredResourceOfItem[i];
        }
    }

    public void OnClickCreateItemButton()
    {
        if (inventorySlotGroup.RequireResourceCountCheck(itemIndex.items[nowItemNum].requiredResourceOfItem))
        {
            inventorySlotGroup.UseResource(itemIndex.items[nowItemNum].requiredResourceOfItem);
            inventorySlotGroup.NowResourceCount();
            SetSelectedItemmInfo(nowItemNum);

            GameObject newItem = Instantiate(itemIndex.items[nowItemNum].gameObject);
            newItem.transform.position = productionCenter.position;
        }
    }


    void Start()
    {
        itemIndex = GameObject.Find("ItemManager").GetComponent<ItemIndex>();
        selectedItemGameObject = GameObject.Find("SelectedItemBackground");
        inventorySlotGroup = GameObject.Find("InventorySlots").GetComponent<InventorySlotGroup>();

    }

    void Update()
    {
        
    }
}
