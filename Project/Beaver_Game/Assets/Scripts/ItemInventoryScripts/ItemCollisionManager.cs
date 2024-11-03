using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCollisionManager : MonoBehaviour
{
    private InventorySlotGroup inventorySlotGroup;  // �κ��丮
    private Button throwRopeButton; // ���� ������ ��ư
    [SerializeField]
    private Button escapePrisonButton;  // ���� Ż�� ��ư
    private GameObject itemImage;   // �κ��丮�� ������ ������
    private ItemIndex itemIndex;    // ������ ����
    public int itemCount = 1;   // ������ ��
    private NetworkManager networkManager;

    
    private void OnTriggerEnter2D(Collider2D collision) // ������ ȹ��(�ݱ�)
    {
        if (!this.enabled)
        {
            return;
        }

        if (collision.gameObject.tag != "Player" || !collision.gameObject.GetComponent<PhotonView>().IsMine)
        {
            if (collision.gameObject.GetComponent<PhotonView>() != null && !collision.gameObject.GetComponent<PhotonView>().IsMine)    // �ٸ� ����� �������� ȹ���� ��� ����
            {
                Destroy(this.gameObject);
            }
            return;
        }

        int emptySlotNum = 0;
        bool findEmptySlot = false;
        bool addInventory = false;

        for (int i = 0; i < inventorySlotGroup.itemSlots.Count; i++)   // �κ��丮 �� ���� ����
        {
            if (inventorySlotGroup.itemSlots[i].gameObject.transform.childCount > 0)    // �� ĭ�� �ƴ϶��
            {
                if (inventorySlotGroup.itemSlots[i].gameObject.transform.GetChild(0).gameObject.GetComponent<ItemDrag>().itemPrefab.GetComponent<ItemInfo>().GetItemIndexNumber()
                    == this.GetComponent<ItemInfo>().GetItemIndexNumber())  // ������ �����۰� ���� ������ �������� ���� ���
                {
                    inventorySlotGroup.itemSlots[i].gameObject.transform.GetChild(0).gameObject.GetComponent<ItemCount>().ShowItemCount(itemCount); // ������ �� ���ϱ�
                    addInventory = true;    // �������� ���ߴٴ� üũ

                    if (this.gameObject.GetComponent<ItemInfo>().itemName == "Key") // ������ ��� ���� ī��Ʈ�� ���ϱ�
                    {
                        collision.gameObject.GetComponent<PrisonManager>().keyCount++;
                    }
                    break;
                }
            }
            else if (!findEmptySlot)    // ���� �������� ���� ��츦 ���� �� ĭ ���
            {
                emptySlotNum = i;
                findEmptySlot = true;
            }
        }

        if (!addInventory && findEmptySlot)  // �κ��丮�� ���� �������� ���� �������� ������ ���ߴٸ�
        {
            GameObject newItemImage = Instantiate(itemImage);   // �κ��丮�� �� ������ ����
            ItemDrag itemIconDrag = newItemImage.GetComponent<ItemDrag>();
            ItemCount itemIconCount = newItemImage.GetComponent<ItemCount>();
            itemIconDrag.SetSpriteRender(itemIndex.items[this.gameObject.GetComponent<ItemInfo>().GetItemIndexNumber()].gameObject.GetComponent<SpriteRenderer>());    // ���� ������ �κ��丮�� �����ܿ� Sprite �ֱ�

            newItemImage.transform.SetParent(inventorySlotGroup.itemSlots[emptySlotNum].gameObject.transform);  // ������ ������ �θ� ����
            itemIconDrag.SetNromalState(); // ������ ������ ��ġ ����

            itemIconCount.SetCountText();  // ������ �������� ���� ������ TMP ����
            itemIconCount.ShowItemCount(itemCount);    // ������ TMP�� ���� ������ �������� �� �����ֱ�

            if (this.gameObject.GetComponent<ItemInfo>().itemName == "Rope")    // ������ ȹ���ϸ� ���� ������ ��ư Ȱ��ȭ
            {
                throwRopeButton.interactable = true;
            }
            else if (this.gameObject.GetComponent<ItemInfo>().itemName == "Key")    // ���踦 ȹ���ϸ� ���� Ż�� ��ư Ȱ��ȭ
            {
                collision.gameObject.GetComponent<PrisonManager>().keyCount++;
                escapePrisonButton.interactable = true;
            }
        }

        inventorySlotGroup.NowResourceCount();  // �κ��丮�� �ڿ� �� ����

        // �������� �ֿ��� ���(������ �ڿ��� ���߰ų� ���� �� ĭ�� ���������) �ٴ��� ������ ����
        if (addInventory || findEmptySlot)
        {
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
            escapePrisonButton = GameObject.Find("EscapePrisonButton").GetComponent<Button>();
        }
    }
}
