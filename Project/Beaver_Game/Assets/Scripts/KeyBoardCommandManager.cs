using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBoardCommandManager : MonoBehaviour
{
    public GameObject inventoryObj;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (inventoryObj.transform.localPosition != Vector3.zero)
            {
                inventoryObj.transform.localPosition = Vector3.zero;
            }
            else
            {
                inventoryObj.transform.localPosition = new Vector3(2000, 0, 0);
            }

            if (inventoryObj.transform.GetChild(1).localPosition == new Vector3(-350.0f, 0.0f, 0.0f))
            {
                inventoryObj.transform.GetChild(1).localPosition = new Vector3(2000.0f, 0.0f, 0.0f);
            }
        }
    }
}
