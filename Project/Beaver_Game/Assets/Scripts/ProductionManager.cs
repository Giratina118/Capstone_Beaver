using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProductionManager : MonoBehaviour
{
    private ItemIndex itemIndex = null; // 아이템 목록
    private GameObject selectedItemGameObject;  // 아이템 제작 창에서 아이템 정보 보여주는 칸
    private InventorySlotGroup inventorySlotGroup;  // 인벤토리
    private int nowItemNum = 0;     // 선택한 아이템의 번호
    public Transform productionCenter;  // 제작대 위치

    public void SetSelectedItemmInfo(int itemNumber)    // 선택한 아이템 정보 보여주기
    {
        nowItemNum = itemNumber;    // 아이템 번호

        selectedItemGameObject.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>().sprite = itemIndex.items[itemNumber].GetComponent<SpriteRenderer>().sprite;   // 아이템 이미지 넣기
        selectedItemGameObject.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>().preserveAspect = true;    // 아이템 가로세로 비 맞추기
        /*
        selectedItemGameObject.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>().color = itemIndex.items[itemNumber].GetComponent<SpriteRenderer>().color; // 나중에 아이템 그림 완성되고 나서는 빼기
        selectedItemGameObject.transform.GetChild(0).GetChild(0).transform.localRotation = itemIndex.items[itemNumber].gameObject.transform.localRotation;  // 나중에 아이템 그림 완성되고 나서는 빼기
        selectedItemGameObject.transform.GetChild(0).GetChild(0).transform.localScale = itemIndex.items[itemNumber].gameObject.transform.localScale;    // 나중에 아이템 그림 완성되고 나서는 빼기
        */
        selectedItemGameObject.transform.GetChild(0).GetChild(1).gameObject.GetComponent<TMP_Text>().text = itemIndex.items[itemNumber].itemName;   // 아이템 이름
        selectedItemGameObject.transform.GetChild(0).GetChild(2).gameObject.GetComponent<TMP_Text>().text = itemIndex.items[itemNumber].itemInformation;    // 아이템 정보(설명)

        for (int i = 0; i < 4; i++) // 제작에 필요한 재료 수 표시
        {
            selectedItemGameObject.transform.GetChild(1).GetChild(i + 1).GetChild(1).GetComponent<TMP_Text>().text = " " + inventorySlotGroup.resourceCountInts[i] + " / " + itemIndex.items[itemNumber].requiredResourceOfItem[i];
        }
    }

    public void OnClickCreateItemButton()   // 아이템 제작
    {
        if (inventorySlotGroup.RequireResourceCountCheck(itemIndex.items[nowItemNum].requiredResourceOfItem))   // 재료가 충분하다면
        {
            inventorySlotGroup.UseResource(itemIndex.items[nowItemNum].requiredResourceOfItem); // 재료 아이템 사용
            inventorySlotGroup.NowResourceCount();  // 현재 인벤토리의 재료 갱신
            SetSelectedItemmInfo(nowItemNum);   // 선택된 아이템을 갱신된 아이템 수로 다시 보여주기

            GameObject newItem = Instantiate(itemIndex.items[nowItemNum].gameObject);   // 제작한 아이템 생성
            newItem.transform.position = productionCenter.position; // 아이템 위치를 제작대로
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
