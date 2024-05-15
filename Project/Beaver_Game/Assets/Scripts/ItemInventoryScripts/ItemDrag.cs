using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDrag : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public SpriteRenderer itemPrefab = null;
    public Image copyItemImage;
    public InventorySlotGroup storageInventorySlotGroup;

    public Vector3 normalPos;
    public Transform normalParent;

    public int itemIndexNumber;
    public int keepItemCount = 0;
    private bool keepItemBool = false;
    public bool dropped = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        normalParent = this.transform.parent;
        normalPos = this.transform.position;
        keepItemCount = 0;
        copyItemImage.gameObject.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = keepItemCount.ToString();
        dropped = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        
        copyItemImage.sprite = itemPrefab.sprite;
        /*
        copyItemImage.transform.localRotation = itemPrefab.transform.localRotation; // 나중에 아이템 그림 완성되고 나서는 빼기
        copyItemImage.transform.localScale = itemPrefab.transform.localScale; // 나중에 아이템 그림 완성되고 나서는 빼기
        */
        copyItemImage.gameObject.transform.position = normalPos;

        Color copyColor = itemPrefab.color;
        copyColor.a = 0.5f;
        copyItemImage.color = copyColor;


        this.transform.SetParent(this.transform.parent.parent.parent);

        this.GetComponent<Image>().raycastTarget = false;
        keepItemBool = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        keepItemBool = false;
        copyItemImage.transform.position = new Vector3(2100.0f, 1200.0f, 0.0f);
        this.transform.position = normalPos;
        this.transform.SetParent(normalParent);

        if (this.transform.parent.gameObject.name != "StorageSlots")
        {
            this.GetComponent<Image>().raycastTarget = true;
        }
        else
        {
            storageInventorySlotGroup.NowResourceCount();
        }
    }

    public void ItemDrop(Vector3 dropItmePos, Transform dropItemParent, bool combine)
    {
        keepItemBool = false;
        if (keepItemCount > 0)  // 드래그할때 원래 장소에 아이템을 남겨뒀다면(수를 나눴다면) 그 위치에 새로 생성
        {
            GameObject newObj = Instantiate(this.gameObject);
            newObj.transform.position = normalPos;
            newObj.transform.SetParent(normalParent);
            newObj.GetComponent<ItemCount>().SetCountText();
            newObj.GetComponent<ItemCount>().count = 0;
            newObj.GetComponent<ItemCount>().ShowItemCount(keepItemCount);
            newObj.GetComponent<Image>().raycastTarget = true;
        }

        if (combine)    // 다른 곳에 아이템이 합쳐진 경우 반투명의 예비 이미지를 원래자리로 되돌리고 현재 드래그한 아이템 삭제
        {
            copyItemImage.transform.position = new Vector3(2100.0f, 1200.0f, 0.0f);
            Destroy(this.gameObject);
        }

        normalPos = dropItmePos;
        normalParent = dropItemParent;
    }

    public void ItemChange(Vector3 changeItmePos, Transform changeItemParent)
    {
        normalPos = changeItmePos;
        normalParent = changeItemParent;
        this.transform.position = normalPos;
        this.transform.SetParent(normalParent);
    }

    public void SetSpriteRender(SpriteRenderer prefabSprite)
    {
        itemPrefab = prefabSprite;
        this.GetComponent<Image>().sprite = itemPrefab.sprite;
        /*
        this.GetComponent<Image>().color = itemPrefab.color;
        this.transform.localRotation = itemPrefab.transform.localRotation;
        this.transform.localScale = itemPrefab.transform.localScale;
        */
        itemIndexNumber = itemPrefab.gameObject.GetComponent<ItemInfo>().GetItemIndexNumber();
    }

    public void SetNromalState()
    {
        this.transform.localPosition = Vector3.zero;
        normalParent = this.transform.parent;
        normalPos = this.transform.position;

    }



    void Start()
    {
        copyItemImage = GameObject.Find("CopyItemImage").GetComponent<Image>();

        if (itemPrefab != null)
        {
            SetSpriteRender(itemPrefab);
        }
    }

    void Update()
    {
        if (keepItemBool && Input.GetMouseButtonDown(1))
        {
            keepItemCount += this.GetComponent<ItemCount>().ItemCountHalf();
            copyItemImage.gameObject.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = keepItemCount.ToString();
        }
    }
}
