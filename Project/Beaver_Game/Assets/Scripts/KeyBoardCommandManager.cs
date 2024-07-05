using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBoardCommandManager : MonoBehaviour
{
    public GameObject inventoryObj; // 인벤토리

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))    // 왼쪽 시프트 키로 인벤토리 열고 닫기
        {
            if (inventoryObj.transform.localPosition != Vector3.zero)
            {
                inventoryObj.transform.localPosition = Vector3.zero;
            }
            else
            {
                inventoryObj.transform.localPosition = new Vector3(2000, 0, 0);
            }

            if (inventoryObj.transform.GetChild(1).localPosition == new Vector3(-350.0f, 0.0f, 0.0f))   // 창고가 열려있으면 인벤토리 닫으면서 같이 닫기
            {
                inventoryObj.transform.GetChild(1).localPosition = new Vector3(2000.0f, 0.0f, 0.0f);
            }
        }
    }
}
