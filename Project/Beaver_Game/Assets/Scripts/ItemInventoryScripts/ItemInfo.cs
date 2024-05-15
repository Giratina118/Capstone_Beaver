using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfo : MonoBehaviour
{
    [SerializeField]
    private int itemIndexNumber;
    public int itemCategory;    // 0: 자원, 1: 머리 장비, 2: 손 장비, 3: 발 장비 or 몸에 부착하는 장비
    public int[] requiredResourceOfItem = new int[4];
    public string itemName;
    public string itemInformation;

    public int GetItemIndexNumber()
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
