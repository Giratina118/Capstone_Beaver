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
        if (Input.GetKeyDown(KeyCode.Z))
        {
            inventoryObj.SetActive(!inventoryObj.activeSelf);
        }
    }
}
