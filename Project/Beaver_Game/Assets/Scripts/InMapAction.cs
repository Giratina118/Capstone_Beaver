using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InMapAction : MonoBehaviour
{
    public GameObject productionImage;  // ������ ���� ȭ��
    public GetResourceManager getResourceManager;   // �ڿ� ä�� ȭ��
    public Button actionButton;     // �׼� ��ư
    private string tagName = "";    // ���� ���ϰ� �ִ� ���� �±��� �̸� ����
    private GameObject damGameObject = null;    // ���� ��ġ�� �ִ� ��
    private Transform ResourcePos;  // ���� ���ϰ��ִ� ���� ��ġ
    public InventorySlotGroup storageSlotGroup; // â�� �κ��丮



    private void OnTriggerEnter2D(Collider2D collision) // ��ư Ȱ��ȭ
    {

        // �׼� ��ư���� ��ȣ�ۿ� �� �� �ִ� ���� ��ġ���� ���
        if (collision.gameObject.transform.tag == "Forest" || collision.gameObject.transform.tag == "Mud" || collision.gameObject.transform.tag == "Stone" || collision.gameObject.transform.tag == "Dump" 
            || collision.gameObject.transform.tag == "Storage" || collision.gameObject.transform.tag == "Dam" || collision.gameObject.tag == "ProductionCenter")
        {
            // �׼� ��ư Ȱ��ȭ �� ����, ���߿��� ��ġ�� ���� ���� ��ư �׸� �ٲ��
            Color buttonColor = actionButton.gameObject.GetComponent<Image>().color;
            buttonColor.a = 200;
            actionButton.gameObject.GetComponent<Image>().color = buttonColor;
            actionButton.interactable = true;

            tagName = collision.gameObject.tag; // ��ġ�� ���� �±� ����

            if (collision.gameObject.transform.tag == "Dam")    // �￡ ��ġ������ ��� �ش� ���� ���� ����
            {
                damGameObject = collision.gameObject;
            }
            else    // ���� �ƴ� ��� �ش� ��ġ ����(�ڿ� ���� �� �ش� ��ġ�� �����ϱ� ����)
            {
                ResourcePos = collision.transform;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)  // ��ư ��Ȱ��ȭ
    {

        // �׼� ��ư���� ��ȣ�ۿ� �� �� �ִ� ������ ����� ���
        if (collision.gameObject.transform.tag == "Mud" || collision.gameObject.transform.tag == "Forest" || collision.gameObject.transform.tag == "Stone" || collision.gameObject.transform.tag == "Dump" 
            || collision.gameObject.transform.tag == "Storage" || collision.gameObject.transform.tag == "Dam" || collision.gameObject.tag == "ProductionCenter")
        {
            // ��ư ��Ȱ��ȭ
            Color buttonColor = actionButton.GetComponent<Image>().color;
            buttonColor.a = 100;
            actionButton.GetComponent<Image>().color = buttonColor;
            actionButton.interactable = false;

            tagName = "";
            
            if (collision.gameObject.transform.tag == "Dam")    // �￡ �־��� ��� �� �Ǽ��� ����, ������ ������� ����
            {
                if (this.gameObject.GetComponent<SpyBoolManager>().isSpy())
                {
                    damGameObject.GetComponent<DamManager>().obstract = 0.0f;
                }
                else
                {
                    damGameObject.GetComponent<DamManager>().accelerate = 0.0f;
                }
            }
        }
    }

    public void OnClickActionButton()   // �׼� ��ư Ŭ��
    {

        switch (tagName)
        {
            // �ڿ� ä���ϴ� �� ���� ���� ��쿡�� �ڿ� ä�� ȭ�� ���� �ڿ��� ���� �����ϱ�
            case "Mud":
                getResourceManager.GetResourceActive(0, ResourcePos);   // �ڿ� ���� ����
                getResourceManager.gameObject.transform.localPosition = Vector3.zero;   // �ڿ� ä�� ȭ�� ����
                break;
            case "Forest":
                getResourceManager.GetResourceActive(1, ResourcePos);
                getResourceManager.gameObject.transform.localPosition = Vector3.zero;
                break;
            case "Stone":
                getResourceManager.GetResourceActive(2, ResourcePos);
                getResourceManager.gameObject.transform.localPosition = Vector3.zero;
                break;
            case "Dump":
                getResourceManager.GetResourceActive(3, ResourcePos);
                getResourceManager.gameObject.transform.localPosition = Vector3.zero;
                break;
            case "Storage":
                storageSlotGroup.gameObject.transform.parent.localPosition = Vector3.zero;  // �κ��丮 ȭ�� ����
                storageSlotGroup.gameObject.transform.localPosition = new Vector3(-350.0f, 0.0f, 0.0f); // â�� ȭ�� ����
                break;
            case "Dam":
                if (!damGameObject.GetComponent<DamManager>().buildComplete)    // �� �ϰ� ���� ��쿡�� ��ȣ�ۿ�
                {
                    if (damGameObject.GetComponent<DamManager>().buildNow)  // �� �Ǽ� ���̶��
                    {
                        if (this.gameObject.GetComponent<SpyBoolManager>().isSpy()) // ������ ����� �� �Ǽ� ����
                        {
                            damGameObject.GetComponent<DamManager>().ObstructBuild();
                        }
                        else    // �Ϲ� ����� �� �Ǽ� ����
                        {
                            damGameObject.GetComponent<DamManager>().AccelerateBuild();
                        }
                    }
                    else    // �� �Ǽ� ���� ���̶�� �� �Ǽ� ����
                    {
                        damGameObject.GetComponent<DamManager>().DamCreate();
                    }
                }
                break;
            case "ProductionCenter":
                productionImage.transform.localPosition = Vector3.zero; // ���� ȭ�� ����
                break;
            default:
                break;
        }
    }


    void Start()
    {
        productionImage = GameObject.Find("ProductionImage");
        getResourceManager = GameObject.Find("GetResourceBackground").GetComponent<GetResourceManager>();
        actionButton = GameObject.Find("ActionButton").GetComponent<Button>();
        storageSlotGroup = GameObject.Find("StorageSlots").GetComponent<InventorySlotGroup>();
        actionButton.onClick.AddListener(OnClickActionButton);
    }

    void Update()
    {
        
    }
}
