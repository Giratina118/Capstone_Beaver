using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ThrowAwayItem : MonoBehaviour, IDropHandler
{
    public GameObject copyItemImage;
    public PrisonManager prisonManager;


    public void OnDrop(PointerEventData eventData)
    {
        copyItemImage.transform.position = new Vector3(2100.0f, 1200.0f, 0.0f);


        if (eventData.pointerDrag.gameObject.GetComponent<ItemDrag>().itemIndexNumber == 4) // 로프 버릴때 0개면 버튼 비활성화 하기
        {
            copyItemImage.transform.parent.gameObject.GetComponent<InventorySlotGroup>().UseItem(4, 0);
        }
        else if (eventData.pointerDrag.gameObject.GetComponent<ItemDrag>().itemIndexNumber == 5)    // 키 버릴때 0개면 버튼 비활성화 하기
        {
            prisonManager.keyCount -= eventData.pointerDrag.GetComponent<ItemCount>().count;
            if (prisonManager.keyCount <= 0)
            {
                prisonManager.escapePrisonButton.gameObject.SetActive(false);
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
