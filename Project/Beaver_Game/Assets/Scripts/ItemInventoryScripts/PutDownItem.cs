using ExitGames.Client.Photon;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PutDownItem : MonoBehaviourPunCallbacks, IDropHandler
{
    public Transform playerPos; // �÷��̾� ��ġ(������ ���������� ���)
    public GameObject copyItemImage;    // ���� ������


    public void OnDrop(PointerEventData eventData)  // ������ ��������
    {
        // �ʵ忡 ������ ����
        GameObject newDropItem = PhotonNetwork.Instantiate(eventData.pointerDrag.GetComponent<ItemDrag>().itemPrefab.gameObject.name, playerPos.position + Vector3.down * 2.0f, Quaternion.identity);
        newDropItem.GetPhotonView().RPC("SetDropItemCount", RpcTarget.All, newDropItem.GetPhotonView().ViewID, eventData.pointerDrag.GetComponent<ItemCount>().count);

        copyItemImage.transform.position = new Vector3(2100.0f, 1200.0f, 0.0f); // ���� ������ ġ���
        ItemDrag itemDrag = eventData.pointerDrag.gameObject.GetComponent<ItemDrag>();
        InventorySlotGroup inventorySlotGroup = this.gameObject.transform.parent.GetComponent<InventorySlotGroup>();
        ItemSlot itemSlot = itemDrag.normalParent.gameObject.GetComponent<ItemSlot>();

        if (itemDrag.itemIndexNumber == 4) // ���� ���������� 0���� ��ư ��Ȱ��ȭ �ϱ�
        {
            inventorySlotGroup.UseItem(4, 0, itemDrag.keepItemCount > 0);
        }
        else if (itemDrag.itemIndexNumber == 5)    // Ű ���������� 0���� ��ư ��Ȱ��ȭ �ϱ�
        {
            playerPos.gameObject.GetComponent<PrisonManager>().keyCount -= eventData.pointerDrag.GetComponent<ItemCount>().count;
            inventorySlotGroup.UseItem(5, 0, itemDrag.keepItemCount > 0);
        }

        if (itemSlot.equipSlotType > 0)   // ���Ǿ��ִ� �������̶��
        {
            itemSlot.equipItem.GetPhotonView().RPC("equipItemDestroy", RpcTarget.All);    // �ʵ忡 ����ϰ� �ִ� �͵� ����
        }

        if (itemDrag.keepItemCount > 0)    // ���� ���� ���� ���¶�� ���� �ִ� ��ġ�� �����״� ����ŭ �ǵ�����
        {
            itemDrag.ItemDrop(this.transform.position, this.transform, true);  // ���� �� ������ �ڰ� true�� �� ����
        }
        else
        {
            Destroy(eventData.pointerDrag);
        }

        inventorySlotGroup.NowResourceCount(); // �κ��丮�� �̹��� ����
    }
}
