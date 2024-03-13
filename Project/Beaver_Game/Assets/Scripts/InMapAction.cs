using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InMapAction : MonoBehaviour
{
    PlayerResourceManager PlayerResourceManager;
    public Button actionButtonImage;
    private string tagName = "";
    private GameObject damGameObject = null;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.transform.tag == "Forest" || collision.gameObject.transform.tag == "Mud" || collision.gameObject.transform.tag == "Dump" || 
            collision.gameObject.transform.tag == "Storage" || collision.gameObject.transform.tag == "Dam")
        {
            Color buttonColor = actionButtonImage.GetComponent<Image>().color;
            buttonColor.a = 200;
            actionButtonImage.GetComponent<Image>().color = buttonColor;

            // 위치한 곳에 따라 버튼 그림 바뀌게

            tagName = collision.gameObject.tag;
            Debug.Log(tagName);
            actionButtonImage.interactable = true;

            if (collision.gameObject.transform.tag == "Dam")
            {
                damGameObject = collision.gameObject;
            }

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.transform.tag == "Forest" || collision.gameObject.transform.tag == "Mud" || collision.gameObject.transform.tag == "Dump" || 
            collision.gameObject.transform.tag == "Storage" || collision.gameObject.transform.tag == "Dam")
        {
            Color buttonColor = actionButtonImage.GetComponent<Image>().color;
            buttonColor.a = 100;
            actionButtonImage.GetComponent<Image>().color = buttonColor;

            tagName = "";
            Debug.Log(tagName);
            actionButtonImage.interactable = false;
        }
    }

    public void OnClickActionButton()
    {
        switch (tagName)
        {
            case "Forest":
                PlayerResourceManager.PlayerResourceCountChange(0, 1);
                break;
            case "Mud":
                PlayerResourceManager.PlayerResourceCountChange(1, 1);
                break;
            case "Dump":
                PlayerResourceManager.PlayerResourceCountChange(2, 1);
                break;
            case "Storage":
                PlayerResourceManager.StoreResource();
                break;
            case "Dam":
                damGameObject.GetComponent<DamManager>().DamCreate(gameObject.GetComponent<PlayerResourceManager>());
                break;
            default:
                break;
        }
        
    }


    void Start()
    {
        PlayerResourceManager = gameObject.GetComponent<PlayerResourceManager>();
    }

    void Update()
    {
        
    }
}
