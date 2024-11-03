using ExitGames.Client.Photon;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviourPunCallbacks, IDropHandler
{
    public bool storageSlot = false;    // 창고의 슬롯인지 여부
    private InventorySlotGroup playerInventory = null;  // 인벤토리
    public GameObject equipItem = null; // 장착한 아이템(인벤토리가 아닌 맵 상의 플레이어가 장비하고 있는 오브젝트)
    public int equipSlotType = 0;   // 1: 머리, 2: 손 도구, 3: 다리 등..  itemInfo의 itemCategory와 숫자가 같도록, 0은 아이템 장착 슬롯이 아님을 의미

    public void OnDrop(PointerEventData eventData)  // 이 슬롯에 아이템을 드래그해서 놓으면 인식
    {
        ItemDrag itemDrag = eventData.pointerDrag.GetComponent<ItemDrag>();
        ItemCount itemCount = eventData.pointerDrag.GetComponent<ItemCount>();
        ItemSlot dragItemSlot = itemDrag.normalParent.gameObject.GetComponent<ItemSlot>();

        if (eventData.pointerDrag == null || itemDrag.dropped)    // 드래그 앤 드롭에서 오류 방지
        {
            return;
        }
        if (equipSlotType != 0 && itemDrag.itemPrefab.GetComponent<ItemInfo>().itemCategory != equipSlotType) // 장착슬롯에는 그에 해당하는 아이템만 들어가도록
        {
            return;
        }

        if (this.transform.childCount > 0)  // 해당 슬롯에 아이템이 있는 경우
        {
            if (dragItemSlot.equipSlotType != 0) // 장착슬롯에서 빼는 아이템은 빈 공간에만 넣도록
            {
                return;
            }

            if (this.transform.GetChild(0).gameObject.GetComponent<ItemDrag>().itemIndexNumber == itemDrag.itemIndexNumber)    // 드래그한 아이템과 같을 경우
            {
                if (storageSlot)    // 창고 슬롯인 경우 창고와 개인 인벤토리 양쪽에 자원 수 갱신
                {
                    this.gameObject.GetPhotonView().RPC("UpdateStorageSlotResourceCount", RpcTarget.All, itemCount.count); // 슬롯의 아이템 수 변경(드래그한 수 더하기)
                    itemDrag.ItemDrop(this.transform.position, this.transform, true);
                    playerInventory.NowResourceCount();
                }
                else
                {
                    this.transform.GetChild(0).gameObject.GetComponent<ItemCount>().ShowItemCount(itemCount.count);    // 슬롯의 아이템 수 변경(드래그한 수 더하기)
                    itemDrag.ItemDrop(this.transform.position, this.transform, true);
                }
            }
            else if (itemDrag.keepItemCount < 1 && !storageSlot)   // 아이템을 우클릭으로 나누지 않았을때(드래그 한 아이템과 다른 아이템인 경우)
            {
                // 드래그 한 아이템과 현재 슬롯의 아이템 교환
                this.transform.GetChild(0).GetComponent<ItemDrag>().ItemChange(itemDrag.normalPos, itemDrag.normalParent);
                itemDrag.ItemDrop(this.transform.position, this.transform, false);
            }
            else    // 나머지의 경우 다시 원래대로 돌리기
            {
                itemCount.ShowItemCount(itemDrag.keepItemCount);
            }
        }
        else    // 해당 슬롯에 아이템이 없는 경우
        {
            if (dragItemSlot.equipSlotType != 0) // 장착슬롯에서 빼는 아이템을 캐릭터에서 지우기
            {
                // 장착되어있던 아이템 효과 지우는 함수 만들기
                dragItemSlot.gameObject.transform.parent.gameObject.GetComponent<ItemEquipManager>().SetItemEffect(itemDrag.itemIndexNumber, false);
                dragItemSlot.equipItem.GetPhotonView().RPC("equipItemDestroy", RpcTarget.All);
            }

            if (itemDrag.normalPos == this.transform.position) // 원래 있던 슬롯에 그대로 둔 경우 다시 되돌리기
            {
                itemCount.ShowItemCount(itemDrag.keepItemCount);
            }
            else    // 원래 있던 슬롯이 아니면 도구 옮기기
            {
                itemDrag.ItemDrop(this.transform.position, this.transform, false);   // 드래그한 도구를 현재 슬롯으로 옮기기
            }
        }

        if (equipSlotType > 0)  // 장착슬롯에 아이템이 장착된 경우
        {
            if (equipItem != null)  // 기존에 같은 카테고리의 장착되어있던 아이템을 제거(캐릭터의 자식으로 만들어진 아이템 오브젝트)
            {
                // 장착되어있던 아이템 효과 지우는 함수 만들기
                this.transform.parent.gameObject.GetComponent<ItemEquipManager>().SetItemEffect(equipItem.GetComponent<ItemInfo>().GetItemIndexNumber(), false);

                Destroy(equipItem);
                equipItem.GetPhotonView().RPC("equipItemDestroy", RpcTarget.All);
            }

            Transform itemNormalTransform = itemDrag.itemPrefab.gameObject.transform;
            ItemEquipManager itemEquipManager = this.transform.parent.gameObject.GetComponent<ItemEquipManager>();
            GameObject playerObject = itemEquipManager.player;

            Vector3 newEquipItemPos = playerObject.transform.position + new Vector3(itemNormalTransform.localPosition.x * playerObject.transform.localScale.x, itemNormalTransform.localPosition.y * playerObject.transform.localScale.y, 0.0f);

            // 새로 장착한 아이템을 캐릭터의 자식으로 생성
            equipItem = PhotonNetwork.Instantiate(itemDrag.itemPrefab.gameObject.name, newEquipItemPos, Quaternion.identity);
            equipItem.GetPhotonView().RPC("equipItemSet", RpcTarget.All, playerObject.GetPhotonView().ViewID);

            playerObject.GetComponent<PlayerMove>().EquippedItemPos();

            // 장착된 아이템 효과 발동시키는 함수 만들기
            itemEquipManager.SetItemEffect(equipItem.GetComponent<ItemInfo>().GetItemIndexNumber(), true);
        }
        itemDrag.dropped = true;
    }

    [PunRPC]
    public void UpdateStorageSlotResourceCount(int count)
    {
        this.transform.GetChild(0).gameObject.GetComponent<ItemCount>().ShowItemCount(count);    // 슬롯의 아이템 수 변경(드래그한 수 더하기)
        this.transform.parent.gameObject.GetComponent<InventorySlotGroup>().StorageResourceCount();
    }

    void Start()
    {
        if (storageSlot)
        {
            playerInventory = GameObject.Find("InventorySlots").GetComponent<InventorySlotGroup>();
        }
    }
}
