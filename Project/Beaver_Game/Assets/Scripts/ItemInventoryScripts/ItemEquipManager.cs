using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemEquipManager : MonoBehaviour
{
    public GameObject player;


    public void SetItemEffect(int itemNum, bool isEquip)
    {
        switch (itemNum)
        {
            case 6:
                break;
            case 7:
                if (isEquip)
                {
                    player.GetComponent<PlayerMove>().moveSpeed = 12.0f;
                    //player.GetComponent<PlayerMove>().navMeshAgent.speed *= 1.2f;
                }
                else
                {
                    player.GetComponent<PlayerMove>().moveSpeed = 10.0f;
                    //player.GetComponent<PlayerMove>().navMeshAgent.speed /= 1.2f;
                }
                break;
            case 8:
                break;
            case 9:
                break;
            case 10:
                break;
            case 11:
                break;
            case 12:
                break;
            case 13:
                break;
            case 14:
                break;
            case 15:
                break;
            case 16:
                break;
            case 17:
                break;
            case 18:
                break;
            case 19:
                break;
            case 20:
                break;
            case 21:
                break;
            case 22:
                break;
            default:
                break;
        }
    }



    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
