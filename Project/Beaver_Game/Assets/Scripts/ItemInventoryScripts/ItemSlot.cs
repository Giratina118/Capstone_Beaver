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
    public bool storageSlot = false;    // â���� �������� ����
    private InventorySlotGroup playerInventory = null;  // �κ��丮
    public GameObject equipItem = null; // ������ ������(�κ��丮�� �ƴ� �� ���� �÷��̾ ����ϰ� �ִ� ������Ʈ)
    public int equipSlotType = 0;   // 1: �Ӹ�, 2: �� ����, 3: �ٸ� ��..  itemInfo�� itemCategory�� ���ڰ� ������, 0�� ������ ���� ������ �ƴ��� �ǹ�

    public void OnDrop(PointerEventData eventData)  // �� ���Կ� �������� �巡���ؼ� ������ �ν�
    {
        ItemDrag itemDrag = eventData.pointerDrag.GetComponent<ItemDrag>();
        ItemCount itemCount = eventData.pointerDrag.GetComponent<ItemCount>();
        ItemSlot dragItemSlot = itemDrag.normalParent.gameObject.GetComponent<ItemSlot>();

        if (eventData.pointerDrag == null || itemDrag.dropped)    // �巡�� �� ��ӿ��� ���� ����
        {
            return;
        }
        if (equipSlotType != 0 && itemDrag.itemPrefab.GetComponent<ItemInfo>().itemCategory != equipSlotType) // �������Կ��� �׿� �ش��ϴ� �����۸� ������
        {
            return;
        }

        if (this.transform.childCount > 0)  // �ش� ���Կ� �������� �ִ� ���
        {
            if (dragItemSlot.equipSlotType != 0) // �������Կ��� ���� �������� �� �������� �ֵ���
            {
                return;
            }

            if (this.transform.GetChild(0).gameObject.GetComponent<ItemDrag>().itemIndexNumber == itemDrag.itemIndexNumber)    // �巡���� �����۰� ���� ���
            {
                if (storageSlot)    // â�� ������ ��� â��� ���� �κ��丮 ���ʿ� �ڿ� �� ����
                {
                    this.gameObject.GetPhotonView().RPC("UpdateStorageSlotResourceCount", RpcTarget.All, itemCount.count); // ������ ������ �� ����(�巡���� �� ���ϱ�)
                    itemDrag.ItemDrop(this.transform.position, this.transform, true);
                    playerInventory.NowResourceCount();
                }
                else
                {
                    this.transform.GetChild(0).gameObject.GetComponent<ItemCount>().ShowItemCount(itemCount.count);    // ������ ������ �� ����(�巡���� �� ���ϱ�)
                    itemDrag.ItemDrop(this.transform.position, this.transform, true);
                }
            }
            else if (itemDrag.keepItemCount < 1 && !storageSlot)   // �������� ��Ŭ������ ������ �ʾ�����(�巡�� �� �����۰� �ٸ� �������� ���)
            {
                // �巡�� �� �����۰� ���� ������ ������ ��ȯ
                this.transform.GetChild(0).GetComponent<ItemDrag>().ItemChange(itemDrag.normalPos, itemDrag.normalParent);
                itemDrag.ItemDrop(this.transform.position, this.transform, false);
            }
            else    // �������� ��� �ٽ� ������� ������
            {
                itemCount.ShowItemCount(itemDrag.keepItemCount);
            }
        }
        else    // �ش� ���Կ� �������� ���� ���
        {
            if (dragItemSlot.equipSlotType != 0) // �������Կ��� ���� �������� ĳ���Ϳ��� �����
            {
                // �����Ǿ��ִ� ������ ȿ�� ����� �Լ� �����
                dragItemSlot.gameObject.transform.parent.gameObject.GetComponent<ItemEquipManager>().SetItemEffect(itemDrag.itemIndexNumber, false);
                dragItemSlot.equipItem.GetPhotonView().RPC("equipItemDestroy", RpcTarget.All);
            }

            if (itemDrag.normalPos == this.transform.position) // ���� �ִ� ���Կ� �״�� �� ��� �ٽ� �ǵ�����
            {
                itemCount.ShowItemCount(itemDrag.keepItemCount);
            }
            else    // ���� �ִ� ������ �ƴϸ� ���� �ű��
            {
                itemDrag.ItemDrop(this.transform.position, this.transform, false);   // �巡���� ������ ���� �������� �ű��
            }
        }

        if (equipSlotType > 0)  // �������Կ� �������� ������ ���
        {
            if (equipItem != null)  // ������ ���� ī�װ��� �����Ǿ��ִ� �������� ����(ĳ������ �ڽ����� ������� ������ ������Ʈ)
            {
                // �����Ǿ��ִ� ������ ȿ�� ����� �Լ� �����
                this.transform.parent.gameObject.GetComponent<ItemEquipManager>().SetItemEffect(equipItem.GetComponent<ItemInfo>().GetItemIndexNumber(), false);

                Destroy(equipItem);
                equipItem.GetPhotonView().RPC("equipItemDestroy", RpcTarget.All);
            }

            Transform itemNormalTransform = itemDrag.itemPrefab.gameObject.transform;
            ItemEquipManager itemEquipManager = this.transform.parent.gameObject.GetComponent<ItemEquipManager>();
            GameObject playerObject = itemEquipManager.player;

            Vector3 newEquipItemPos = playerObject.transform.position + new Vector3(itemNormalTransform.localPosition.x * playerObject.transform.localScale.x, itemNormalTransform.localPosition.y * playerObject.transform.localScale.y, 0.0f);

            // ���� ������ �������� ĳ������ �ڽ����� ����
            equipItem = PhotonNetwork.Instantiate(itemDrag.itemPrefab.gameObject.name, newEquipItemPos, Quaternion.identity);
            equipItem.GetPhotonView().RPC("equipItemSet", RpcTarget.All, playerObject.GetPhotonView().ViewID);

            playerObject.GetComponent<PlayerMove>().EquippedItemPos();

            // ������ ������ ȿ�� �ߵ���Ű�� �Լ� �����
            itemEquipManager.SetItemEffect(equipItem.GetComponent<ItemInfo>().GetItemIndexNumber(), true);
        }
        itemDrag.dropped = true;
    }

    [PunRPC]
    public void UpdateStorageSlotResourceCount(int count)
    {
        this.transform.GetChild(0).gameObject.GetComponent<ItemCount>().ShowItemCount(count);    // ������ ������ �� ����(�巡���� �� ���ϱ�)
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
