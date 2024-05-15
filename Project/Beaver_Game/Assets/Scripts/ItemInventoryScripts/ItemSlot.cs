using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    public bool storageSlot = false;
    private InventorySlotGroup playerInventory = null;
    public GameObject equipItem = null;

    public int equipSlotType = 0;   // 1: 머리, 2: 손 도구, 3: 다리 등..  itemInfo의 itemCategory와 숫자가 같도록, 0은 아이템 장착 슬롯이 아님을 의미

    private int clawNum = 18;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null || eventData.pointerDrag.GetComponent<ItemDrag>().dropped)
        {
            return;
        }
            
        if (equipSlotType != 0 && eventData.pointerDrag.GetComponent<ItemDrag>().itemPrefab.GetComponent<ItemInfo>().itemCategory != equipSlotType) // 장착슬롯에는 그에 해당하는 아이템만 들어가도록
        {
            return;
        }



        if (this.transform.childCount > 0)  // 해당 슬롯에 아이템이 있는 경우
        {
            if (eventData.pointerDrag.GetComponent<ItemDrag>().normalParent.gameObject.GetComponent<ItemSlot>().equipSlotType != 0) // 장착슬롯에서 빼는 아이템은 빈 공간에만 넣도록
            {
                return;
            }


            if (this.transform.GetChild(0).gameObject.GetComponent<ItemDrag>().itemIndexNumber == eventData.pointerDrag.gameObject.GetComponent<ItemDrag>().itemIndexNumber)    // 드래그한 아이템과 같을 경우
            {
                this.transform.GetChild(0).gameObject.GetComponent<ItemCount>().ShowItemCount(eventData.pointerDrag.gameObject.GetComponent<ItemCount>().count);    // 슬롯의 아이템 수 변경(드래그한 수 더하기)
                eventData.pointerDrag.GetComponent<ItemDrag>().ItemDrop(this.transform.position, this.transform, true);

                if (storageSlot)
                {
                    this.transform.parent.gameObject.GetComponent<InventorySlotGroup>().NowResourceCount();
                    playerInventory.NowResourceCount();
                }
            }
            else if (eventData.pointerDrag.gameObject.GetComponent<ItemDrag>().keepItemCount < 1 && !storageSlot)
            {
                this.transform.GetChild(0).GetComponent<ItemDrag>().ItemChange(eventData.pointerDrag.GetComponent<ItemDrag>().normalPos, eventData.pointerDrag.GetComponent<ItemDrag>().normalParent);
                eventData.pointerDrag.GetComponent<ItemDrag>().ItemDrop(this.transform.position, this.transform, false);
            }
            else
            {
                eventData.pointerDrag.gameObject.GetComponent<ItemCount>().ShowItemCount(eventData.pointerDrag.gameObject.GetComponent<ItemDrag>().keepItemCount);
            }
            
        }
        else    // 해당 슬롯에 아이템이 없는 경우
        {
            if (eventData.pointerDrag.GetComponent<ItemDrag>().normalParent.gameObject.GetComponent<ItemSlot>().equipSlotType != 0) // 장착슬롯에서 빼는 아이템을 캐릭터에서 지우기
            {

                // 장착되어있던 아이템 효과 지우는 함수 만들기

                Destroy(eventData.pointerDrag.GetComponent<ItemDrag>().normalParent.gameObject.GetComponent<ItemSlot>().equipItem);
            }


            if (eventData.pointerDrag.gameObject.GetComponent<ItemDrag>().normalPos == this.transform.position)
            {
                eventData.pointerDrag.gameObject.GetComponent<ItemCount>().ShowItemCount(eventData.pointerDrag.gameObject.GetComponent<ItemDrag>().keepItemCount);
            }
            else
            {
                eventData.pointerDrag.GetComponent<ItemDrag>().ItemDrop(this.transform.position, this.transform, false);   // 드래그한 도구를 현재 슬롯으로 옮기기
            }
            
        }


        if (equipSlotType > 0)  // 장착슬롯에 아이템이 장착된 경우
        {
            if (equipItem != null)  // 기존에 같은 카테고리의 장착되어있던 아이템을 제거(캐릭터의 자식으로 만들어진 아이템 오브젝트)
            {

                // 장착되어있던 아이템 효과 지우는 함수 만들기

                Destroy(equipItem);
            }

            // 새로 장착한 아이템을 캐릭터의 자식으로 생성
            equipItem = Instantiate(eventData.pointerDrag.gameObject.GetComponent<ItemDrag>().itemPrefab.gameObject, this.transform.parent.GetComponent<ItemEquipManager>().player.transform);
            equipItem.transform.localScale = Vector3.one;
            equipItem.GetComponent<ItemCollisionManager>().enabled = false;
            equipItem.GetComponent<SpriteRenderer>().sortingOrder = 11; // 캐릭터보다 위에 보이게 하기 위해서 조정(캐릭터는 10)
            equipItem.layer = 7;    // 장착한 아이템이 인벤토리의 아이템 장착화면에 보이도록 Layer를 7(EquipItem)으로 변경

            if (equipItem.GetComponent<ItemInfo>().GetItemIndexNumber() == clawNum)  // 손톱일 경우 크기 조정
            {
                equipItem.transform.localScale = Vector3.one * 0.5f;
            }
        }

        // 장착된 아이템 효과 발동시키는 함수 만들기

        eventData.pointerDrag.GetComponent<ItemDrag>().dropped = true;
    }


    void Start()
    {
        if (storageSlot)
        {
            playerInventory = GameObject.Find("InventorySlots").GetComponent<InventorySlotGroup>();
        }


    }

    void Update()
    {
        
    }
}
