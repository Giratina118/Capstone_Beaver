using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;

public class DamManager : MonoBehaviour
{
    public int[] requiredResources = new int[3]; // 0: ³ª¹«, 1: ÁøÈë, 2: °­Ã¶
    public ResourceManager resourceManager;



    public void DamCreate(PlayerResourceManager playerResource)
    {
        bool damCreateBool = true;

        for (int i = 0; i < 3; i++)
        {
            if (requiredResources[i] > playerResource.playerResourceCountInts[i] + resourceManager.resourceCountInts[i])
            {
                damCreateBool = false;
                break;
            }
        }

        if (damCreateBool)
        {
            int[] remainNum = new int[3] {0, 0, 0};

            for (int i = 0; i < 3; i++)
            {
                remainNum[i] = 0;
                if (requiredResources[i] > playerResource.playerResourceCountInts[i])
                {
                    remainNum[i] = requiredResources[i] - playerResource.playerResourceCountInts[i];
                }

                playerResource.PlayerResourceCountChange(i, -requiredResources[i] + remainNum[i]);
                requiredResources[i] = 0;
            }
            resourceManager.StorageResourceCountChange(-remainNum[0], -remainNum[1], -remainNum[2]);

            
            Color damColor =  gameObject.GetComponent<SpriteRenderer>().color;
            damColor.a = 150;
            gameObject.GetComponent<SpriteRenderer>().color = damColor;
            
        }

    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
