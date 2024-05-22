using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemolishTower : MonoBehaviourPunCallbacks
{
    private bool onTower = false;   // Ÿ�� ���� �ִ��� ����
    private GameObject tower = null;    // ���� ���ϰ� �ִ� Ÿ��
    public Button demolishTowerButton;  // Ÿ�� ö�� ��ư
    public GetResourceManager getResourceManager;   // Ÿ�� ö�� �� �ڿ� �����ޱ� ����
    public TimerManager timerManager;   // Ÿ�� ö�ſ� ���� �ð� ���� ����

    [SerializeField]
    private float increaseTime = 20.0f;


    private void OnTriggerEnter2D(Collider2D collision) // Ÿ�� ���� ������ ��ư Ȱ��ȭ
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

    private void OnTriggerExit2D(Collider2D collision)  // Ÿ�� ���� ������ ��ư ��Ȱ��ȭ
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

    public void OnClickDemolishTowerButton()    // �Ϳ� ���� �ִٸ� �ı�
    {
        if (onTower)
        {
            /*
            for (int i = 0; i < 3; i++)
            {
                this.GetComponent<PlayerResourceManager>().PlayerResourceCountChange(i, tower.GetComponent<TowerInfo>().requiredResourceOfTowers[i] / 2);
            }
            */

            for (int i = 0; i < 4; i++)
            {
                getResourceManager.GetResourceActive(i, tower.gameObject.transform);
                for (int j = 0; j < tower.GetComponent<TowerInfo>().requiredResourceOfTowers[i] / 2; j++)
                {
                    getResourceManager.OnClickButtonInGetResource();
                }
            }

            if (PhotonNetwork.IsMasterClient)
            {
                timerManager.TowerTime(-increaseTime);
                //timerManager.timerPhotonView.RPC("TowerTime", RpcTarget.All, -increaseTime);
            }
            else
            {
                timerManager.timerPhotonView.RPC("TowerTime", RpcTarget.MasterClient, -increaseTime);
            }

            //timerManager.TowerTime(-increaseTime);  // �ð� ����
            GameObject.Destroy(tower);  // Ÿ�� �ı�
        }
    }



    void Start()
    {
        demolishTowerButton = GameObject.Find("DemolishTowerButton").GetComponent<Button>();
        getResourceManager = GameObject.Find("GetResourceBackground").GetComponent<GetResourceManager>();
        timerManager = GameObject.Find("Timer").GetComponent<TimerManager>();
        demolishTowerButton.onClick.AddListener(OnClickDemolishTowerButton);

    }

    void Update()
    {
        
    }
}
