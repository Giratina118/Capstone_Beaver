using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCollisionManager : MonoBehaviour
{
    private InventorySlotGroup inventorySlotGroup;  // 인벤토리
    private Button throwRopeButton; // 로프 던지기 버튼
    [SerializeField]
    private Button escapePrisonButton;  // 감옥 탈출 버튼
    private GameObject itemImage;   // 인벤토리용 아이템 아이콘
    private ItemIndex itemIndex;    // 아이템 도감
    public int itemCount = 1;   // 아이템 수
    private NetworkManager networkManager;

    
    private void OnTriggerEnter2D(Collider2D collision) // 아이템 획득(줍기)
    {
        if (!this.enabled)
        {
            return;
        }

        if (collision.gameObject.tag != "Player" || !collision.gameObject.GetComponent<PhotonView>().IsMine)
        {
            if (collision.gameObject.GetComponent<PhotonView>() != null && !collision.gameObject.GetComponent<PhotonView>().IsMine)    // 다른 사람이 아이템을 획득한 경우 삭제
            {
                Destroy(this.gameObject);
            }
            return;
        }
            

        int emptySlotNum = 0;
        bool findEmptySlot = false;
        bool addInventory = false;

        for (int i = 0; i < inventorySlotGroup.itemSlots.Count; i++)   // 인벤토리 한 바퀴 돌기
        {
            if (inventorySlotGroup.itemSlots[i].gameObject.transform.childCount > 0)    // 빈 칸이 아니라면
            {
                if (inventorySlotGroup.itemSlots[i].gameObject.transform.GetChild(0).gameObject.GetComponent<ItemDrag>().itemPrefab.GetComponent<ItemInfo>().GetItemIndexNumber()
                    == this.GetComponent<ItemInfo>().GetItemIndexNumber())  // 접촉한 아이템과 같은 종류의 아이템이 있을 경우
                {
                    inventorySlotGroup.itemSlots[i].gameObject.transform.GetChild(0).gameObject.GetComponent<ItemCount>().ShowItemCount(itemCount); // 아이템 수 더하기
                    addInventory = true;    // 아이템을 더했다는 체크

                    if (this.gameObject.GetComponent<ItemInfo>().itemName == "Key") // 열쇠의 경우 따로 카운트도 더하기
                    {
                        collision.gameObject.GetComponent<PrisonManager>().keyCount++;
                    }

                    break;
                }
            }
            else if (!findEmptySlot)    // 같은 아이템이 없을 경우를 위한 빈 칸 기록
            {
                emptySlotNum = i;
                findEmptySlot = true;
            }
        }

        if (!addInventory && findEmptySlot)  // 인벤토리에 같은 아이템이 없어 아이템을 더하지 못했다면
        {
            GameObject newItemImage = Instantiate(itemImage);   // 인벤토리용 새 아이콘 생성

            newItemImage.GetComponent<ItemDrag>().SetSpriteRender(itemIndex.items[this.gameObject.GetComponent<ItemInfo>().GetItemIndexNumber()].gameObject.GetComponent<SpriteRenderer>());    // 새로 생성한 인벤토리용 아이콘에 Sprite 넣기

            newItemImage.transform.SetParent(inventorySlotGroup.itemSlots[emptySlotNum].gameObject.transform);  // 생성한 아이템 부모 설정
            newItemImage.GetComponent<ItemDrag>().SetNromalState(); // 생성한 아이템 위치 설정

            newItemImage.GetComponent<ItemCount>().SetCountText();  // 생성한 아이템의 수를 보여줄 TMP 연결
            newItemImage.GetComponent<ItemCount>().ShowItemCount(itemCount);    // 연결한 TMP를 통해 생성한 아이템의 수 보여주기


            if (this.gameObject.GetComponent<ItemInfo>().itemName == "Rope")    // 로프를 획득하면 로프 던지기 버튼 활성화
            {
                //throwRopeButton.gameObject.SetActive(true);
                throwRopeButton.enabled = true;
                Color throwRopeButtonColor = throwRopeButton.GetComponent<Image>().color;
                throwRopeButtonColor.a = 1.0f;
                throwRopeButton.GetComponent<Image>().color = throwRopeButtonColor;
            }
            else if (this.gameObject.GetComponent<ItemInfo>().itemName == "Key")    // 열쇠를 획득하면 감옥 탈출 버튼 활성화
            {
                collision.gameObject.GetComponent<PrisonManager>().keyCount++;
                //escapePrisonButton.gameObject.SetActive(true);
                escapePrisonButton.enabled = true;
                Color escapeButtonColor = escapePrisonButton.GetComponent<Image>().color;
                escapeButtonColor.a = 1.0f;
                escapePrisonButton.GetComponent<Image>().color = escapeButtonColor;
            }
        }

        inventorySlotGroup.NowResourceCount();  // 인벤토리의 자원 수 갱신

        // 아이템을 주웠을 경우(기존의 자원에 더했거나 새로 빈 칸에 만들었을때) 바닥의 아이템 삭제
        if (addInventory || findEmptySlot)
        {
            //PhotonNetwork.Destroy(this.gameObject);
            Destroy(this.gameObject);
        }
            
    }

    [PunRPC]
    public void SetDropItemCount(int dropItemViewID, int dropItemCount)
    {
        PhotonView.Find(dropItemViewID).GetComponent<ItemCollisionManager>().itemCount = dropItemCount;
    }


    void Start()
    {

        inventorySlotGroup = GameObject.Find("InventorySlots").GetComponent<InventorySlotGroup>();
        itemImage = GameObject.Find("ItemImage").gameObject;
        itemIndex = GameObject.Find("ItemManager").GetComponent<ItemIndex>();
        networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();

        if (this.gameObject.GetComponent<ItemInfo>().itemName == "Rope")
        {
            throwRopeButton = GameObject.Find("ThrowRopeLine").GetComponent<RopeManager>().throwRopeButton;
        }
        else if (this.gameObject.GetComponent<ItemInfo>().itemName == "Key")
        {
            //escapePrisonButton = GameObject.Find("PlayerBeaver").GetComponent<PrisonManager>().escapePrisonButton;
            escapePrisonButton = GameObject.Find("EscapePrisonButton").GetComponent<Button>();
        }

    }

    void Update()
    {

    }
}
