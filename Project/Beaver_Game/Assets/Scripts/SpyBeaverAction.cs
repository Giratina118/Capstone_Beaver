using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpyBeaverAction : MonoBehaviour
{
    private PlayerResourceManager playerResourceManager;
    public TowerInfo towerInfo;

    public void BuildTower()
    {
        
        if (playerResourceManager.playerResourceCountInts[0] >= towerInfo.requiredResourceOfTowers[0] && playerResourceManager.playerResourceCountInts[1] >= towerInfo.requiredResourceOfTowers[1] && 
            playerResourceManager.playerResourceCountInts[2] >= towerInfo.requiredResourceOfTowers[2])
        {
            for (int i = 0; i < 3; i++)
            {
                playerResourceManager.PlayerResourceCountChange(i, -towerInfo.requiredResourceOfTowers[i]);
            }

            GameObject newTower = GameObject.Instantiate(towerInfo.gameObject);
            newTower.transform.position = this.transform.position;
        }
    }

    void Start()
    {
        playerResourceManager = GetComponent<PlayerResourceManager>();
    }

    void Update()
    {
        
    }
}
