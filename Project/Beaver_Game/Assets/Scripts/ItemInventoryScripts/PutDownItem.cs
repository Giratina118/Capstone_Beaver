using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PutDownItem : MonoBehaviour, IDropHandler
{
    public Transform playerPos;
    public GameObject copyItemImage;



    public void OnDrop(PointerEventData eventData)
    {
        GameObject newResource = Instantiate(eventData.pointerDrag.GetComponent<ItemDrag>().itemPrefab.gameObject);
        newResource.transform.position = playerPos.position + Vector3.down * 2;
        newResource.GetComponent<ItemCollisionManager>().itemCount = eventData.pointerDrag.GetComponent<ItemCount>().count;

        copyItemImage.transform.position = new Vector3(2100.0f, 1200.0f, 0.0f);




        if (eventData.pointerDrag.gameObject.GetComponent<ItemDrag>().itemIndexNumber == 4) // 로프 내려놓을때 0개면 버튼 비활성화 하기
        {
            copyItemImage.transform.parent.gameObject.GetComponent<InventorySlotGroup>().UseItem(4, 0);
        }
        else if (eventData.pointerDrag.gameObject.GetComponent<ItemDrag>().itemIndexNumber == 5)    // 키 내려놓을때 0개면 버튼 비활성화 하기
        {
            playerPos.gameObject.GetComponent<PrisonManager>().keyCount -= eventData.pointerDrag.GetComponent<ItemCount>().count;
            if (playerPos.gameObject.GetComponent<PrisonManager>().keyCount <= 0)
            {
                playerPos.gameObject.GetComponent<PrisonManager>().escapePrisonButton.gameObject.SetActive(false);
            }
        }

        if (eventData.pointerDrag.gameObject.GetComponent<ItemDrag>().normalParent.gameObject.GetComponent<ItemSlot>().equipSlotType > 0)
        {
            Destroy(eventData.pointerDrag.gameObject.GetComponent<ItemDrag>().normalParent.gameObject.GetComponent<ItemSlot>().equipItem);
        }

        if (eventData.pointerDrag.gameObject.GetComponent<ItemDrag>().keepItemCount > 0)    // 만약 수를 나눈 상태라면
        {
            eventData.pointerDrag.gameObject.GetComponent<ItemDrag>().ItemDrop(this.transform.position, this.transform, true);  // 앞의 두 변수는 뒤가 true면 안 쓰임
        }
        else
        {
            Destroy(eventData.pointerDrag);
        }


        this.gameObject.transform.parent.GetComponent<InventorySlotGroup>().NowResourceCount();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
