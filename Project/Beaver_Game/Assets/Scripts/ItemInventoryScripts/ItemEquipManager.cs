using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemEquipManager : MonoBehaviour
{
    public GameObject player;
    public GetResourceScrollbar resourceScrollbar;
    public DamManager[] damManagers;

    // 장비 아이템 착용 시 효과
    public void SetItemEffect(int itemNum, bool isEquip)
    {
        switch (itemNum)
        {
            case 6:
                if (isEquip)
                {
                    for (int i = 0; i < damManagers.Length; i++)
                    {
                        damManagers[i].dameGaugeSpeedRate = 2.0f;
                    }
                }
                else
                {
                    for (int i = 0; i < damManagers.Length; i++)
                    {
                        damManagers[i].dameGaugeSpeedRate = 1.0f;
                    }
                }
                break;
            case 7:
                if (isEquip)
                {
                    player.GetComponent<PlayerMove>().runSpeed = 13.0f;
                }
                else
                {
                    player.GetComponent<PlayerMove>().runSpeed = 10.0f;
                }
                break;
            case 8:
                if (isEquip)
                {
                    player.GetComponent<PlayerMove>().swimSpeed = 9.0f;
                }
                else
                {
                    player.GetComponent<PlayerMove>().swimSpeed = 6.0f;
                }
                break;
            case 9:
                if (isEquip)
                {
                    resourceScrollbar.equipItemLevel[0] = 1;
                }
                else
                {
                    resourceScrollbar.equipItemLevel[0] = 0;
                }
                break;
            case 10:
                if (isEquip)
                {
                    resourceScrollbar.equipItemLevel[0] = 2;
                }
                else
                {
                    resourceScrollbar.equipItemLevel[0] = 0;
                }
                break;
            case 11:
                if (isEquip)
                {
                    resourceScrollbar.equipItemLevel[0] = 3;
                }
                else
                {
                    resourceScrollbar.equipItemLevel[0] = 0;
                }
                break;
            case 12:
                if (isEquip)
                {
                    resourceScrollbar.equipItemLevel[1] = 1;
                }
                else
                {
                    resourceScrollbar.equipItemLevel[1] = 0;
                }
                break;
            case 13:
                if (isEquip)
                {
                    resourceScrollbar.equipItemLevel[1] = 2;
                }
                else
                {
                    resourceScrollbar.equipItemLevel[1] = 0;
                }
                break;
            case 14:
                if (isEquip)
                {
                    resourceScrollbar.equipItemLevel[1] = 3;
                }
                else
                {
                    resourceScrollbar.equipItemLevel[1] = 0;
                }
                break;
            case 15:
                if (isEquip)
                {
                    resourceScrollbar.equipItemLevel[2] = 1;
                    resourceScrollbar.equipItemLevel[3] = 1;
                }
                else
                {
                    resourceScrollbar.equipItemLevel[2] = 0;
                    resourceScrollbar.equipItemLevel[3] = 0;
                }
                break;
            case 16:
                if (isEquip)
                {
                    resourceScrollbar.equipItemLevel[2] = 2;
                    resourceScrollbar.equipItemLevel[3] = 2;
                }
                else
                {
                    resourceScrollbar.equipItemLevel[2] = 0;
                    resourceScrollbar.equipItemLevel[3] = 0;
                }
                break;
            case 17:
                if (isEquip)
                {
                    resourceScrollbar.equipItemLevel[2] = 3;
                    resourceScrollbar.equipItemLevel[3] = 3;
                }
                else
                {
                    resourceScrollbar.equipItemLevel[2] = 0;
                    resourceScrollbar.equipItemLevel[3] = 0;
                }
                break;
            case 18:
                if (isEquip)
                {
                    resourceScrollbar.equipItemLevel[0] = 3;
                    resourceScrollbar.equipItemLevel[1] = 3;
                    resourceScrollbar.equipItemLevel[2] = 3;
                    resourceScrollbar.equipItemLevel[3] = 3;
                }
                else
                {
                    resourceScrollbar.equipItemLevel[0] = 0;
                    resourceScrollbar.equipItemLevel[1] = 0;
                    resourceScrollbar.equipItemLevel[2] = 0;
                    resourceScrollbar.equipItemLevel[3] = 0;
                }
                break;
            case 19:
                if (isEquip)
                {
                    resourceScrollbar.fourleafCloverBonus = 2;
                }
                else
                {
                    resourceScrollbar.fourleafCloverBonus = 0;
                }
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
