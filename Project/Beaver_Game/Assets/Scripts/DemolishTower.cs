using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemolishTower : MonoBehaviour
{
    private bool onTower = false;
    private GameObject tower = null;
    public Button demolishTowerButton;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Tower")
        {
            onTower = true;
            tower = collision.gameObject;

            Color buttonColor = demolishTowerButton.gameObject.GetComponent<Image>().color;
            buttonColor.a = 200;
            demolishTowerButton.gameObject.GetComponent<Image>().color = buttonColor;
            demolishTowerButton.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Tower")
        {
            onTower = false;

            Color buttonColor = demolishTowerButton.gameObject.GetComponent<Image>().color;
            buttonColor.a = 100;
            demolishTowerButton.gameObject.GetComponent<Image>().color = buttonColor;
            demolishTowerButton.enabled = false;
        }
    }

    public void OnClickDemolishTowerButton()
    {
        if (onTower)
        {
            for (int i = 0; i < 3; i++)
            {
                this.GetComponent<PlayerResourceManager>().PlayerResourceCountChange(i, tower.GetComponent<TowerInfo>().requiredResourceOfTowers[i] / 2);
            }
            
            GameObject.Destroy(tower);
        }
    }


    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
