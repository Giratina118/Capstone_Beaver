using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDrag : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public bool isItem = false;
    
    public SpriteRenderer itemPrefab;
    private Image copyItemImage;

    public Vector3 normalPos;
    public Transform normalParent;

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("0");
        if (isItem)
        {
            normalParent = this.transform.parent;

            
            
        }

    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("11");

        copyItemImage.sprite = itemPrefab.sprite;
        copyItemImage.gameObject.transform.position = normalPos;

        Color copyColor = itemPrefab.color;
        copyColor.a = 0.5f;
        copyItemImage.color = copyColor;


        this.transform.SetParent(this.transform.parent.parent);

        this.GetComponent<Image>().raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("222");
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("3333");
        copyItemImage.transform.position = new Vector3(2100.0f, 1200.0f, 0.0f);
        this.transform.position = normalPos;
        this.transform.SetParent(normalParent);


        this.GetComponent<Image>().raycastTarget = true;
    }

    public void ItemDrop(Vector3 dropItmePos, Transform dropItemParent)
    {
        normalPos = dropItmePos;
        normalParent = dropItemParent;

        Debug.Log("555555");
    }

    public void ItemChange(Vector3 changeItmePos, Transform changeItemParent)
    {
        normalPos = changeItmePos;
        normalParent = changeItemParent;
        this.transform.position = normalPos;
        this.transform.SetParent(normalParent);
    }


    void Start()
    {
        normalPos = this.transform.position;

        copyItemImage = GameObject.Find("CopyItemImage").GetComponent<Image>();

        // 테스트용
        if (itemPrefab != null)
        {
            isItem = true;
            this.GetComponent<Image>().sprite = itemPrefab.sprite;
            this.GetComponent<Image>().color = itemPrefab.color;

        }
    }

    void Update()
    {
        
    }
}
