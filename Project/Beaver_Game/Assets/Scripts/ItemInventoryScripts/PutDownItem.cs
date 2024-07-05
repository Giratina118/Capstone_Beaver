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
        //newDropItem.transform.position = playerPos.position + Vector3.down * 2;
        //newDropItem.GetComponent<ItemCollisionManager>().itemCount = eventData.pointerDrag.GetComponent<ItemCount>().count;
        newDropItem.GetPhotonView().RPC("SetDropItemCount", RpcTarget.All, newDropItem.GetPhotonView().ViewID, eventData.pointerDrag.GetComponent<ItemCount>().count);


        copyItemImage.transform.position = new Vector3(2100.0f, 1200.0f, 0.0f); // 예비 아이템 치우기


        if (eventData.pointerDrag.gameObject.GetComponent<ItemDrag>().itemIndexNumber == 4) // 로프 내려놓을때 0개면 버튼 비활성화 하기
        {
            copyItemImage.transform.parent.gameObject.GetComponent<InventorySlotGroup>().UseItem(4, 0, eventData.pointerDrag.gameObject.GetComponent<ItemDrag>().keepItemCount > 0);
        }
        else if (eventData.pointerDrag.gameObject.GetComponent<ItemDrag>().itemIndexNumber == 5)    // 키 내려놓을때 0개면 버튼 비활성화 하기
        {
            playerPos.gameObject.GetComponent<PrisonManager>().keyCount -= eventData.pointerDrag.GetComponent<ItemCount>().count;
            copyItemImage.transform.parent.gameObject.GetComponent<InventorySlotGroup>().UseItem(5, 0, eventData.pointerDrag.gameObject.GetComponent<ItemDrag>().keepItemCount > 0);
            /*
            if (playerPos.gameObject.GetComponent<PrisonManager>().keyCount <= 0 && eventData.pointerDrag.gameObject.GetComponent<ItemDrag>().keepItemCount <= 0)
            {
                //playerPos.gameObject.GetComponent<PrisonManager>().escapePrisonButton.gameObject.SetActive(false);
                playerPos.gameObject.GetComponent<PrisonManager>().escapePrisonButton.enabled = false;
                Color escapeButtonColor = playerPos.gameObject.GetComponent<PrisonManager>().escapePrisonButton.GetComponent<Image>().color;
                escapeButtonColor.a = 0.5f;
                playerPos.gameObject.GetComponent<PrisonManager>().escapePrisonButton.GetComponent<Image>().color = escapeButtonColor;
            }
            */
        }

        if (eventData.pointerDrag.gameObject.GetComponent<ItemDrag>().normalParent.gameObject.GetComponent<ItemSlot>().equipSlotType > 0)   // 장비되어있던 아이템이라면
        {
            //Destroy(eventData.pointerDrag.gameObject.GetComponent<ItemDrag>().normalParent.gameObject.GetComponent<ItemSlot>().equipItem);  // 필드에 장비하고 있던 것도 삭제
            eventData.pointerDrag.gameObject.GetComponent<ItemDrag>().normalParent.gameObject.GetComponent<ItemSlot>().equipItem.GetPhotonView().RPC("equipItemDestroy", RpcTarget.All);
        }

        if (eventData.pointerDrag.gameObject.GetComponent<ItemDrag>().keepItemCount > 0)    // 만약 수를 나눈 상태라면 원래 있던 위치에 나눠뒀던 수만큼 되돌리기
        {
            eventData.pointerDrag.gameObject.GetComponent<ItemDrag>().ItemDrop(this.transform.position, this.transform, true);  // 앞의 두 변수는 뒤가 true면 안 쓰임
        }
        else
        {
            Destroy(eventData.pointerDrag);
        }


        this.gameObject.transform.parent.GetComponent<InventorySlotGroup>().NowResourceCount(); // 인벤토리의 이미지 갱신
    }

    


    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
