using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDrag : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public SpriteRenderer itemPrefab = null;    // 아이템 프리팹
    public Image copyItemImage; // 드래그 시 원래 있던 위치에 위치할 반투명 복사 이미지 (예비 아이템)
    public InventorySlotGroup storageInventorySlotGroup;    // 창고 인벤토리

    public Vector3 normalPos;   // 슬롯의 좌표
    public Transform normalParent;  // 슬롯의 위치(아이템의 부모)

    public int itemIndexNumber;     // 아이템 번호
    public int keepItemCount = 0;   // 우클릭으로 아이템을 나눴을때 들고있지 않은 쪽의 아이템 수
    private bool keepItemBool = false;  // 우클릭으로 나눴는지 여부
    public bool dropped = false;    // 아이템을 놨는지 여부


    public void OnPointerDown(PointerEventData eventData)   // 아이템을 클릭했을때
    {
        normalParent = this.transform.parent;   // 그 슬롯을 부모로 설정
        normalPos = this.transform.position;    // 그 슬롯의 위치로 이동
        keepItemCount = 0;
        copyItemImage.gameObject.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = keepItemCount.ToString(); // 예비 아이템의 개수
        dropped = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        copyItemImage.sprite = itemPrefab.sprite;   // 예비 아이템의 이미지 설정
        copyItemImage.gameObject.transform.position = normalPos;    // 예비 아이템의 위치 설정

        Color copyColor = itemPrefab.color; // 예비 아이템을 반투명하게
        copyColor.a = 0.5f;
        copyItemImage.color = copyColor;

        this.transform.SetParent(this.transform.parent.parent.parent);  // 부모를 더 상위의 오브젝트로 -> 드래그 하고 있는 아이템으 가장 위에 보이도록

        this.GetComponent<Image>().raycastTarget = false;   // 이 슬롯은 마우스로 인식 못 하도록
        keepItemBool = true;
    }

    public void OnDrag(PointerEventData eventData)  // 드래그 중일때 아이템 위치가 마우스를 따라오도록
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)   // 드래그 종료 시
    {
        keepItemBool = false;
        copyItemImage.transform.position = new Vector3(2100.0f, 1200.0f, 0.0f); // 예비 아이템 치우기
        this.transform.position = normalPos;    // 이 아이템을 원위치로
        this.transform.SetParent(normalParent); // 이 아이템을 원래 부모로

        if (this.transform.parent.gameObject.name != "StorageSlots")    // 창고 슬롯이 아닌 경우
        {
            this.GetComponent<Image>().raycastTarget = true;    // 다시 원래대로 마우스를 인식 가능하도록
        }
        else
        {
            storageInventorySlotGroup.NowResourceCount();   // 창고인 경우 아이템 수 갱신
        }
    }

    public void ItemDrop(Vector3 dropItmePos, Transform dropItemParent, bool combine)   // 아이템을 놓았을때
    {
        keepItemBool = false;
        if (keepItemCount > 0)  // 드래그할때 원래 장소에 아이템을 남겨뒀다면(수를 나눴다면) 그 위치에 새로 생성
        {
            GameObject newObj = Instantiate(this.gameObject);   // 아이템 생성
            ItemCount itemCount = newObj.GetComponent<ItemCount>();
            newObj.transform.position = normalPos;  // 새로 생긴 아이템 위치 설정
            newObj.transform.SetParent(normalParent);   // 새로 생긴 아이템 부모 설정  
            itemCount.SetCountText();    // 아이템 수 적는 텍스트 연결
            itemCount.count = 0;     // 아이템 수
            itemCount.ShowItemCount(keepItemCount);  // 연결한 텍스트에 아이템 수 출력
            newObj.GetComponent<Image>().raycastTarget = true;  // 마우스를 인식할 수 있도록
        }

        if (combine)    // 다른 곳에 아이템이 합쳐진 경우 반투명의 예비 이미지를 원래자리로 되돌리고 현재 드래그한 아이템 삭제
        {
            copyItemImage.transform.position = new Vector3(2100.0f, 1200.0f, 0.0f); // 예비 아이템 치우기
            Destroy(this.gameObject);   // 드래그 한 아이템 삭제
        }

        normalPos = dropItmePos;
        normalParent = dropItemParent;
    }

    public void ItemChange(Vector3 changeItmePos, Transform changeItemParent)   // 아이템 교체(서로 부모(슬롯) 위치와 현재 위치(좌표)를 변경)
    {
        normalPos = changeItmePos;
        normalParent = changeItemParent;
        this.transform.position = normalPos;
        this.transform.SetParent(normalParent);
    }

    public void SetSpriteRender(SpriteRenderer prefabSprite)    // 아이템의 이미지 설정
    {
        itemPrefab = prefabSprite;
        this.GetComponent<Image>().sprite = itemPrefab.sprite;
        itemIndexNumber = itemPrefab.gameObject.GetComponent<ItemInfo>().GetItemIndexNumber();  // 아이템 번호 저장
    }

    public void SetNromalState()    // 아이템 생성될 때 기본 좌표 및 부모 설정
    {
        this.transform.localPosition = Vector3.zero;
        normalParent = this.transform.parent;
        normalPos = this.transform.position;
    }

    void Start()
    {
        copyItemImage = GameObject.Find("CopyItemImage").GetComponent<Image>();

        if (itemPrefab != null) // 아이템 이미지 설정
        {
            SetSpriteRender(itemPrefab);
        }
    }

    void Update()
    {
        if (keepItemBool && Input.GetMouseButtonDown(1))    // 아이템을 든 채로 우클릭 시 수를 반으로 나눔
        {
            keepItemCount += this.GetComponent<ItemCount>().ItemCountHalf();
            copyItemImage.gameObject.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = keepItemCount.ToString();
        }
    }
}
