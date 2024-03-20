using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InMapAction : MonoBehaviour
{
    public GetResourceManager getResourceManager;
    PlayerResourceManager PlayerResourceManager;
    public Button actionButtonImage;
    private string tagName = "";
    private GameObject damGameObject = null;

    private Transform ResourcePos;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.transform.tag == "Forest" || collision.gameObject.transform.tag == "Mud" || collision.gameObject.transform.tag == "Stone" ||
            collision.gameObject.transform.tag == "Dump" || collision.gameObject.transform.tag == "Storage" || collision.gameObject.transform.tag == "Dam")
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
            else
            {
                ResourcePos = collision.transform;
            }

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.transform.tag == "Mud" || collision.gameObject.transform.tag == "Forest" || collision.gameObject.transform.tag == "Stone" || 
            collision.gameObject.transform.tag == "Dump" || collision.gameObject.transform.tag == "Storage" || collision.gameObject.transform.tag == "Dam")
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
                getResourceManager.gameObject.SetActive(true);
                getResourceManager.GetResourceActive(1, ResourcePos);
                break;
            case "Mud":
                PlayerResourceManager.PlayerResourceCountChange(1, 1);
                getResourceManager.gameObject.SetActive(true);
                getResourceManager.GetResourceActive(0, ResourcePos);
                break;
            case "Dump":
                PlayerResourceManager.PlayerResourceCountChange(2, 1);
                getResourceManager.gameObject.SetActive(true);
                getResourceManager.GetResourceActive(3, ResourcePos);
                break;
            case "Stone":
                PlayerResourceManager.PlayerResourceCountChange(2, 1);
                getResourceManager.gameObject.SetActive(true);
                getResourceManager.GetResourceActive(2, ResourcePos);
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
