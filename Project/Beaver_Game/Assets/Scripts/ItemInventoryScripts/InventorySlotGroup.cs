using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotGroup : MonoBehaviour
{
    public List<ItemSlot> itemSlots = new List<ItemSlot>();
    public int[] resourceCountInts = new int[4] { 0, 0, 0, 0 };
    public TMP_Text[] resourceCountTexts = new TMP_Text[4];
    public Button throwRopeButton = null;
    public GameObject escapePrisonButton = null;
    public SpyBoolManager spyBoolManager = null;

    public void ShowResourceText()
    {
        for (int i = 0; i < resourceCountInts.Length; i++)
        {
            resourceCountTexts[i].text = resourceCountInts[i].ToString();
        }
    }

    public void NowResourceCount()   // 인벤토리 내 자원 숫자 세는거
    {
        for (int i = 0; i < resourceCountInts.Length; i++)
        {
            resourceCountInts[i] = 0;
        }

        for (int i = 0; i < itemSlots.Count; i++)
        {
            if (itemSlots[i].gameObject.transform.childCount != 0)
            {
                GameObject nowItemSlotChild = itemSlots[i].gameObject.transform.GetChild(0).gameObject;

                if (nowItemSlotChild.GetComponent<ItemDrag>().itemIndexNumber < resourceCountInts.Length)
                {
                    resourceCountInts[nowItemSlotChild.GetComponent<ItemDrag>().itemIndexNumber] += nowItemSlotChild.GetComponent<ItemCount>().count;
                }
            }
        }
        ShowResourceText();
    }

    public bool RequireResourceCountCheck(int[] requirement)    // 인벤토리 내 자원 숫자가 건설, 제작 시 필요 자원량보다 많은지 체크
    {
        bool buildable = true;
        NowResourceCount();
        for (int i = 0; i < resourceCountInts.Length; i++)
        {
            if (resourceCountInts[i] < requirement[i])
            {
                buildable = false;
                break;
            }
        }
        return buildable;
    }

    public void UseItem(int itemIndexNum, int useItemCount) // 하나의 도구를 인벤토리 전체를 살펴서 쓰는 방식
    {
        int remainResourceCount = useItemCount;
        int ropeIndexNum = 4;   // 만약 루프의 인덱스 넘버가 바뀌면 이 숫자 바꿔주기
        int keyIndexNum = 5;    // 만약 탈출키의 인덱스 넘버가 바뀌면 이 숫자 바꿔주기
        bool haveRope = false;
        bool haveKey = false;


        for (int i = 0; i < itemSlots.Count; i++)
        {
            // 현재 체크하는 슬롯의 아이템이 사용한 아이템일 경우
            if (itemSlots[i].gameObject.transform.childCount != 0 && itemIndexNum == itemSlots[i].gameObject.transform.GetChild(0).gameObject.GetComponent<ItemDrag>().itemPrefab.gameObject.GetComponent<ItemInfo>().GetItemIndexNumber())
            {
                GameObject childItem = itemSlots[i].gameObject.transform.GetChild(0).gameObject;

                if (childItem.GetComponent<ItemCount>().count < remainResourceCount)    // 현재 슬롯의 아이템 수가 사용한 아이템 수보다 적으면
                {
                    remainResourceCount -= childItem.GetComponent<ItemCount>().count;
                    Destroy(childItem);
                }
                else    // 현재 슬롯의 아이템 수가 사용한 아이템 수보다 많거나 같으면
                {
                    childItem.GetComponent<ItemCount>().ShowItemCount(-remainResourceCount);
                    remainResourceCount = 0;

                    if (childItem.GetComponent<ItemCount>().count <= 0) // 슬롯의 아이템 수가 사용한 아이템 수와 같은 경우에만 인벤토리에서 삭제
                    {
                        Destroy(childItem);
                    }
                    else if (itemIndexNum == ropeIndexNum)
                    {
                        haveRope = true;
                    }
                    else if (itemIndexNum == keyIndexNum)
                    {
                        haveKey = true;
                    }

                }
            }
        }

        if (throwRopeButton != null && itemIndexNum == ropeIndexNum && !haveRope)  // 버튼 비활성화
        {
            throwRopeButton.gameObject.SetActive(false);
        }
        else if (escapePrisonButton != null && itemIndexNum == keyIndexNum && !haveKey)
        {
            if (spyBoolManager.isSpy() && !spyBoolManager.gameObject.GetComponent<SpyBeaverAction>().useEmergencyEscape)
            {

            }
            else
                escapePrisonButton.SetActive(false);
        }

    }

    public void UseResource(int[] useResourceCount) // 인벤토리 전체를 살피면서 4개의 자원이라면 사용하는 방식
    {
        int[] remainResource = new int[4];
        
        for (int i = 0; i < 4; i++)
        {
            remainResource[i] = useResourceCount[i];
        }

        for (int i = 0; i < itemSlots.Count; i++)
        {
            // 해당 슬롯에 아이템(자식)이 있고 그 아이템이 자원일 경우(아이템 도감 번호 4 미만)
            if (itemSlots[i].gameObject.transform.childCount != 0 && 4 > itemSlots[i].gameObject.transform.GetChild(0).gameObject.GetComponent<ItemDrag>().itemPrefab.gameObject.GetComponent<ItemInfo>().GetItemIndexNumber())
            {
                GameObject childItemObj = itemSlots[i].gameObject.transform.GetChild(0).gameObject; // 해당 슬롯에 있는 아이템
                int itemNum = childItemObj.GetComponent<ItemDrag>().itemPrefab.gameObject.GetComponent<ItemInfo>().GetItemIndexNumber();    // 해당 슬롯에 있는 아이템의 도감 번호

                if (remainResource[itemNum] <= 0)   // 해당 자원이 더 지불할 필요가 없다면 넘기기
                    continue;

                if (childItemObj.GetComponent<ItemCount>().count <= remainResource[itemNum]) // 지불해야할 자원이 현재 슬롯에 있는 아이템 수보다 많거나 같을 경우
                {
                    remainResource[itemNum] -= childItemObj.GetComponent<ItemCount>().count;
                    childItemObj.GetComponent<ItemCount>().ShowItemCount(-childItemObj.GetComponent<ItemCount>().count);

                    if (!itemSlots[i].gameObject.GetComponent<ItemSlot>().storageSlot) // 창고슬롯이 아니면 -> 플레이어 인벤토리 슬롯이라면 아이템 삭제
                    {
                        Destroy(childItemObj);
                    }
                }
                else    // 지불해야할 자원이 현재 슬롯에 있는 아이템 수보다 적을 경우
                {
                    childItemObj.GetComponent<ItemCount>().ShowItemCount(-remainResource[itemNum]);
                    remainResource[itemNum] = 0;

                }

            }
        }

    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
