using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotGroup : MonoBehaviourPunCallbacks
{
    public List<ItemSlot> itemSlots = new List<ItemSlot>();     // 아이템 도감
    public int[] resourceCountInts = new int[4] { 0, 0, 0, 0 }; // 기본 자원(진흙, 나무, 돌, 철)의 수
    public TMP_Text[] resourceCountTexts = new TMP_Text[4];     // 기본 자원의 수를 적는 텍스트(UI 좌상단)
    public Button throwRopeButton = null;   // 로프 던지기 버튼
    public Button escapePrisonButton = null;    // 감옥 탈출 버튼
    public SpyBoolManager spyBoolManager = null;    // 스파이 여부

    public void ShowResourceText()  // 기본 자원 개수 텍스트로 출력
    {
        for (int i = 0; i < resourceCountInts.Length; i++)
        {
            resourceCountTexts[i].text = resourceCountInts[i].ToString();
        }
    }

    public void StorageResourceCount()
    {
        photonView.RPC("NowResourceCount", RpcTarget.All);
    }

    [PunRPC]
    public void NowResourceCount()   // 인벤토리 내 자원 숫자 세기
    {
        for (int i = 0; i < resourceCountInts.Length; i++)  // 기본 자원 개수 센거 0으로 초기화
        {
            resourceCountInts[i] = 0;
        }

        for (int i = 0; i < itemSlots.Count; i++)   // 슬롯들을 하나씩 체크
        {
            if (itemSlots[i].gameObject.transform.childCount != 0)  // 슬롯에 아이템이 존재할 경우
            {
                GameObject nowItemSlotChild = itemSlots[i].gameObject.transform.GetChild(0).gameObject; // 현재 체크하는 슬롯의 아이템

                if (nowItemSlotChild.GetComponent<ItemDrag>().itemIndexNumber < resourceCountInts.Length)   // 그 슬롯의 아이템이 기본 자원일 경우 수 더하기
                {
                    resourceCountInts[nowItemSlotChild.GetComponent<ItemDrag>().itemIndexNumber] += nowItemSlotChild.GetComponent<ItemCount>().count;
                }
            }
        }
        ShowResourceText(); // 자원 수 텍스트로 출력
    }

    public bool RequireResourceCountCheck(int[] requirement)    // 인벤토리 내 자원 숫자가 건설, 제작 시 필요 자원량보다 많은지 체크
    {
        bool buildable = true;
        NowResourceCount(); // 현재 자원 수 체크
        for (int i = 0; i < resourceCountInts.Length; i++)  // 현재 자원 수가 필요 자원 수보다 적으면 건설 불가
        {
            if (resourceCountInts[i] < requirement[i])
            {
                buildable = false;
                break;
            }
        }
        return buildable;
    }

    public void UseItem(int itemIndexNum, int useItemCount, bool keepItem) // 하나의 도구를 인벤토리 전체를 살펴서 쓰는 방식(자원 이외의 로프나 열쇠 등을 사용할때 쓴다)
    {
        int remainResourceCount = useItemCount; // 사용할 아이템 수
        int ropeIndexNum = 4;   // 만약 루프의 인덱스 넘버가 바뀌면 이 숫자 바꿔주기
        int keyIndexNum = 5;    // 만약 탈출키의 인덱스 넘버가 바뀌면 이 숫자 바꿔주기
        bool haveRope = false;  // 로프 가지고 있는지
        bool haveKey = false;   // 열쇠 가지고 있는지


        for (int i = 0; i < itemSlots.Count; i++)   // 슬롯들을 하나씩 체크
        {
            // 현재 체크하는 슬롯의 아이템이 사용한 아이템일 경우
            if (itemSlots[i].gameObject.transform.childCount != 0 && itemIndexNum == itemSlots[i].gameObject.transform.GetChild(0).gameObject.GetComponent<ItemDrag>().itemPrefab.gameObject.GetComponent<ItemInfo>().GetItemIndexNumber())
            {
                GameObject childItem = itemSlots[i].gameObject.transform.GetChild(0).gameObject;    // 현재 슬롯의 아이템
                if (childItem.GetComponent<ItemCount>().count < remainResourceCount)    // 현재 슬롯의 아이템 수가 사용할 아이템 수보다 적으면
                {
                    remainResourceCount -= childItem.GetComponent<ItemCount>().count;   // 사용할 아이템 수를 현재 슬롯의 아이템 수만큼 빼기
                    Destroy(childItem); // 현재 슬롯의 아이템 삭제(모두 사용)
                }
                else    // 현재 슬롯의 아이템 수가 사용한 아이템 수보다 많거나 같으면
                {
                    childItem.GetComponent<ItemCount>().ShowItemCount(-remainResourceCount);    // 현재 슬롯의 아이템 수를 사용할 아이템 수만큼 감소
                    remainResourceCount = 0;

                    if (childItem.GetComponent<ItemCount>().count <= 0) // 슬롯의 아이템 수가 사용한 아이템 수와 같은 경우에만 인벤토리에서 삭제
                    {
                        Destroy(childItem);
                    }
                    else if (itemIndexNum == ropeIndexNum)  // 로프 유무
                    {
                        haveRope = true;
                    }
                    else if (itemIndexNum == keyIndexNum)   // 열쇠 유무
                    {
                        haveKey = true;
                    }

                }
            }
        }

        

        if (throwRopeButton != null && itemIndexNum == ropeIndexNum && !haveRope && !keepItem)  // 로프가 없을 경우 버튼 비활성화
        {
            //throwRopeButton.gameObject.SetActive(false);
            throwRopeButton.enabled = false;
            Color throwRopeButtonColor = throwRopeButton.GetComponent<Image>().color;
            throwRopeButtonColor.a = 0.5f;
            throwRopeButton.GetComponent<Image>().color = throwRopeButtonColor;
        }
        else if (escapePrisonButton != null && itemIndexNum == keyIndexNum && !haveKey && !keepItem) // 열쇠가 없고 (스파이면서 긴급탈출을 사용할 수 있는 경우가 아니면) 버튼 비활성화
        {
            if (spyBoolManager.isSpy() && !spyBoolManager.gameObject.GetComponent<SpyBeaverAction>().spyBeaverEscape)     // 이 위의 if문에 하나로 합치기 가능
            {

            }
            else
            {
                //escapePrisonButton.SetActive(false);
                escapePrisonButton.enabled = false;
                Color escapeButtonColor = escapePrisonButton.GetComponent<Image>().color;
                escapeButtonColor.a = 0.5f;
                escapePrisonButton.GetComponent<Image>().color = escapeButtonColor;
            }
                
        }

    }

    public void UseResource(int[] useResourceCount) // 인벤토리 전체를 살피면서 4개의 자원이라면 사용하는 방식
    {
        int[] remainResource = new int[4];
        
        for (int i = 0; i < 4; i++)
        {
            remainResource[i] = useResourceCount[i];    // 사용할 자원
        }

        for (int i = 0; i < itemSlots.Count; i++)   // 슬롯들을 하나씩 체크
        {
            // 해당 슬롯에 아이템(자식)이 있고 그 아이템이 자원일 경우(아이템 도감 번호 4 미만)
            if (itemSlots[i].gameObject.transform.childCount != 0 && 4 > itemSlots[i].gameObject.transform.GetChild(0).gameObject.GetComponent<ItemDrag>().itemPrefab.gameObject.GetComponent<ItemInfo>().GetItemIndexNumber())
            {
                GameObject childItemObj = itemSlots[i].gameObject.transform.GetChild(0).gameObject; // 해당 슬롯에 있는 아이템
                int itemNum = childItemObj.GetComponent<ItemDrag>().itemPrefab.gameObject.GetComponent<ItemInfo>().GetItemIndexNumber();    // 해당 슬롯에 있는 아이템의 도감 번호

                if (remainResource[itemNum] <= 0)   // 해당 자원을 더 지불할 필요가 없다면 넘기기
                    continue;

                if (childItemObj.GetComponent<ItemCount>().count <= remainResource[itemNum]) // 지불해야할 자원이 현재 슬롯에 있는 아이템 수보다 많거나 같을 경우
                {
                    remainResource[itemNum] -= childItemObj.GetComponent<ItemCount>().count;    // 남은 사용할 자원을 현재 슬롯의 아이템 수로 뺌
                    childItemObj.GetComponent<ItemCount>().ShowItemCount(-childItemObj.GetComponent<ItemCount>().count);    // 현재 슬롯의 아이템 수를 0으로

                    if (!itemSlots[i].gameObject.GetComponent<ItemSlot>().storageSlot) // 창고슬롯이 아니면 -> 플레이어 인벤토리 슬롯이라면 아이템 삭제
                    {
                        Destroy(childItemObj);
                    }
                }
                else    // 지불해야할 자원이 현재 슬롯에 있는 아이템 수보다 적을 경우
                {
                    childItemObj.GetComponent<ItemCount>().ShowItemCount(-remainResource[itemNum]); // 현재 슬롯의 아이템 수를 감소
                    remainResource[itemNum] = 0;    // 남은 사용할 자원 수를 0으로

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
