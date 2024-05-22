using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemolishTower : MonoBehaviourPunCallbacks
{
    private bool onTower = false;   // 타워 위에 있는지 여부
    private GameObject tower = null;    // 현재 접하고 있는 타워
    public Button demolishTowerButton;  // 타워 철거 버튼
    public GetResourceManager getResourceManager;   // 타워 철거 후 자원 돌려받기 위함
    public TimerManager timerManager;   // 타워 철거에 따른 시간 복구 위함

    [SerializeField]
    private float increaseTime = 20.0f;


    private void OnTriggerEnter2D(Collider2D collision) // 타워 위에 있으면 버튼 활성화
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

    private void OnTriggerExit2D(Collider2D collision)  // 타워 위에 없으면 버튼 비활성화
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

    public void OnClickDemolishTowerButton()    // 터워 위에 있다면 파괴
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

            //timerManager.TowerTime(-increaseTime);  // 시간 복구
            GameObject.Destroy(tower);  // 타워 파괴
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
