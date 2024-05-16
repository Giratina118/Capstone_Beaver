using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfo : MonoBehaviour
{
    [SerializeField]
    private int itemIndexNumber;    // 아이템 고유 번호(도감 번호)
    public int itemCategory;    // 아이템 종류, 0: 자원, 1: 머리 장비, 2: 손 장비, 3: 발 장비 or 몸에 부착하는 장비
    public int[] requiredResourceOfItem = new int[4];   // 아이템 제작에 필요한 자원 수
    public string itemName;     // 아이템 이름
    public string itemInformation;  // 아이템 설명글

    public int GetItemIndexNumber() // 아이템 번호 불러오기
    {
        return itemIndexNumber;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
