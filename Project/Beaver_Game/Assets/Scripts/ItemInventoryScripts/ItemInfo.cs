using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;

public class ItemInfo : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private int itemIndexNumber;    // 아이템 고유 번호(도감 번호)
    public int itemCategory;    // 아이템 종류, 0: 자원, 1: 머리 장비, 2: 손 장비, 3: 발 장비 or 몸에 부착하는 장비
    public int[] requiredResourceOfItem = new int[4];   // 아이템 제작에 필요한 자원 수
    public string itemName;     // 아이템 이름
    public string itemInformation;  // 아이템 설명글

    public Vector3 normalPos;
    public Vector3 normalRot;
    public Vector3 normalScale;

    public Vector3 walkPos;
    public Vector3 walkRot;
    public Vector3 walkScale;

    public Vector3 swimPos;
    public Vector3 swimRot;
    public Vector3 swimScale;


    public int GetItemIndexNumber() // 아이템 번호 불러오기
    {
        return itemIndexNumber;
    }

    [PunRPC]
    public void equipItemSet(int equipPlayerViewID)
    {
        this.gameObject.GetComponent<ItemCollisionManager>().enabled = false;

        this.transform.SetParent(PhotonView.Find(equipPlayerViewID).gameObject.transform);
        this.transform.localScale = Vector3.one;
        this.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 11; // 캐릭터보다 위에 보이게 하기 위해서 조정(캐릭터는 10)

        this.gameObject.layer = this.gameObject.transform.parent.gameObject.layer;  // 장착한 아이템이 인벤토리의 아이템 장착화면에 보이도록 Layer 변경
    }

    [PunRPC]
    public void equipItemDestroy()
    {
        Destroy(this.gameObject);
    }
}
