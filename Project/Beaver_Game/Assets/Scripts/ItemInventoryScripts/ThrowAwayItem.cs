using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ThrowAwayItem : MonoBehaviour, IDropHandler
{
    public GameObject copyItemImage;    // ���� �̹���
    public PrisonManager prisonManager; // ����(���� �� ����)


    public void OnDrop(PointerEventData eventData)  // ������ ������
    {
        copyItemImage.transform.position = new Vector3(2100.0f, 1200.0f, 0.0f); // ���� ������ ġ��
        ItemDrag itemDrag = eventData.pointerDrag.gameObject.GetComponent<ItemDrag>();

        if (itemDrag == null)
        {
            return;
        }

        if (itemDrag.itemIndexNumber == 4) // ���� ������ 0���� ��ư ��Ȱ��ȭ �ϱ�
        {
            copyItemImage.transform.parent.gameObject.GetComponent<InventorySlotGroup>().UseItem(4, 0, itemDrag.keepItemCount > 0);
        }
        else if (itemDrag.itemIndexNumber == 5)    // Ű ������ 0���� ��ư ��Ȱ��ȭ �ϱ�
        {
            prisonManager.keyCount -= eventData.pointerDrag.GetComponent<ItemCount>().count;
            copyItemImage.transform.parent.gameObject.GetComponent<InventorySlotGroup>().UseItem(5, 0, itemDrag.keepItemCount > 0);
        }

        if (itemDrag.normalParent.gameObject.GetComponent<ItemSlot>().equipSlotType > 0)   // ���� �������̾��ٸ� �ʵ��� �����۵� ����
        {
            itemDrag.normalParent.gameObject.GetComponent<ItemSlot>().equipItem.GetPhotonView().RPC("equipItemDestroy", RpcTarget.All);
        }

        if (itemDrag.keepItemCount > 0)    // ���� ���� ���� ���¶��
        {
            itemDrag.ItemDrop(this.transform.position, this.transform, true);  // ���� �� ������ �ڰ� true�� �� ����
        }
        else
        {
            Destroy(eventData.pointerDrag);
        }

        this.gameObject.transform.parent.GetComponent<InventorySlotGroup>().NowResourceCount(); // ������ �� ����
    }
}
