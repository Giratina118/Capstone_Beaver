using ExitGames.Client.Photon;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PutDownItem : MonoBehaviourPunCallbacks, IDropHandler
{
    public Transform playerPos; // 플레이어 위치(아이템 내려놓을때 사용)
    public GameObject copyItemImage;    // 예비 아이템


    public void OnDrop(PointerEventData eventData)  // 아이템 내려놓기
    {
        // 필드에 아이템 생성
        GameObject newDropItem = PhotonNetwork.Instantiate(eventData.pointerDrag.GetComponent<ItemDrag>().itemPrefab.gameObject.name, playerPos.position + Vector3.down * 2.0f, Quaternion.identity);
        newDropItem.GetPhotonView().RPC("SetDropItemCount", RpcTarget.All, newDropItem.GetPhotonView().ViewID, eventData.pointerDrag.GetComponent<ItemCount>().count);

        copyItemImage.transform.position = new Vector3(2100.0f, 1200.0f, 0.0f); // 예비 아이템 치우기
        ItemDrag itemDrag = eventData.pointerDrag.gameObject.GetComponent<ItemDrag>();
        InventorySlotGroup inventorySlotGroup = this.gameObject.transform.parent.GetComponent<InventorySlotGroup>();
        ItemSlot itemSlot = itemDrag.normalParent.gameObject.GetComponent<ItemSlot>();

        if (itemDrag.itemIndexNumber == 4) // 로프 내려놓을때 0개면 버튼 비활성화 하기
        {
            inventorySlotGroup.UseItem(4, 0, itemDrag.keepItemCount > 0);
        }
        else if (itemDrag.itemIndexNumber == 5)    // 키 내려놓을때 0개면 버튼 비활성화 하기
        {
            playerPos.gameObject.GetComponent<PrisonManager>().keyCount -= eventData.pointerDrag.GetComponent<ItemCount>().count;
            inventorySlotGroup.UseItem(5, 0, itemDrag.keepItemCount > 0);
        }

        if (itemSlot.equipSlotType > 0)   // 장비되어있던 아이템이라면
        {
            itemSlot.equipItem.GetPhotonView().RPC("equipItemDestroy", RpcTarget.All);    // 필드에 장비하고 있던 것도 삭제
        }

        if (itemDrag.keepItemCount > 0)    // 만약 수를 나눈 상태라면 원래 있던 위치에 나눠뒀던 수만큼 되돌리기
        {
            itemDrag.ItemDrop(this.transform.position, this.transform, true);  // 앞의 두 변수는 뒤가 true면 안 쓰임
        }
        else
        {
            Destroy(eventData.pointerDrag);
        }

        inventorySlotGroup.NowResourceCount(); // 인벤토리의 이미지 갱신
    }
}
