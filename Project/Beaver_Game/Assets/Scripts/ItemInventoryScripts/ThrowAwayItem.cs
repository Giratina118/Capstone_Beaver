using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ThrowAwayItem : MonoBehaviour, IDropHandler
{
    public GameObject copyItemImage;    // 예비 이미지
    public PrisonManager prisonManager; // 감옥(열쇠 수 관리)


    public void OnDrop(PointerEventData eventData)  // 아이템 버리기
    {
        copyItemImage.transform.position = new Vector3(2100.0f, 1200.0f, 0.0f); // 예비 아이템 치움
        ItemDrag itemDrag = eventData.pointerDrag.gameObject.GetComponent<ItemDrag>();

        if (itemDrag == null)
        {
            return;
        }

        if (itemDrag.itemIndexNumber == 4) // 로프 버릴때 0개면 버튼 비활성화 하기
        {
            copyItemImage.transform.parent.gameObject.GetComponent<InventorySlotGroup>().UseItem(4, 0, itemDrag.keepItemCount > 0);
        }
        else if (itemDrag.itemIndexNumber == 5)    // 키 버릴때 0개면 버튼 비활성화 하기
        {
            prisonManager.keyCount -= eventData.pointerDrag.GetComponent<ItemCount>().count;
            copyItemImage.transform.parent.gameObject.GetComponent<InventorySlotGroup>().UseItem(5, 0, itemDrag.keepItemCount > 0);
        }

        if (itemDrag.normalParent.gameObject.GetComponent<ItemSlot>().equipSlotType > 0)   // 장비된 아이템이었다면 필드의 아이템도 삭제
        {
            itemDrag.normalParent.gameObject.GetComponent<ItemSlot>().equipItem.GetPhotonView().RPC("equipItemDestroy", RpcTarget.All);
        }

        if (itemDrag.keepItemCount > 0)    // 만약 수를 나눈 상태라면
        {
            itemDrag.ItemDrop(this.transform.position, this.transform, true);  // 앞의 두 변수는 뒤가 true면 안 쓰임
        }
        else
        {
            Destroy(eventData.pointerDrag);
        }

        this.gameObject.transform.parent.GetComponent<InventorySlotGroup>().NowResourceCount(); // 아이템 수 갱신
    }
}
