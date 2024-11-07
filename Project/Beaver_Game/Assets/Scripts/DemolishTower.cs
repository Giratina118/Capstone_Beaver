using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DemolishTower : MonoBehaviourPunCallbacks
{
    private bool onTower = false;   // 타워 위에 있는지 여부
    private GameObject tower = null;    // 현재 접하고 있는 타워
    public Button demolishTowerButton;  // 타워 철거 버튼
    public GetResourceManager getResourceManager;   // 타워 철거 후 자원 돌려받기 위함
    public TimerManager timerManager;   // 타워 철거에 따른 시간 복구 위함
    public ItemIndex itemIndex; // 아이템 목록

    [SerializeField]
    private float increaseTime = 20.0f;


    private void OnTriggerEnter2D(Collider2D collision) // 타워 위에 있으면 버튼 활성화
    {
        if (!this.gameObject.GetPhotonView().IsMine)
            return;

        if (collision.gameObject.tag == "Tower")
        {
            onTower = true;
            tower = collision.gameObject;
            demolishTowerButton.interactable = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)  // 타워 위에 없으면 버튼 비활성화
    {
        if (!this.gameObject.GetPhotonView().IsMine)
            return;

        if (collision.gameObject.tag == "Tower")
        {
            onTower = false;
            demolishTowerButton.interactable = false;
        }
    }

    public void OnClickDemolishTowerButton()    // 타워 위에 있다면 파괴
    {
        if (!this.GetComponent<PhotonView>().IsMine)
            return;

        if (onTower)
        {
            for (int i = 0; i < 4; i++)
            {
                //getResourceManager.GetResourceActive(i, tower.gameObject.transform);
                for (int j = 0; j < tower.GetComponent<TowerInfo>().requiredResourceOfTowers[i] / 2; j++)
                {
                    Vector3 rand = Random.insideUnitCircle * 1.5f;
                    PhotonNetwork.Instantiate(itemIndex.items[i].gameObject.name, tower.transform.position + rand, Quaternion.identity);
                }
            }

            if (PhotonNetwork.IsMasterClient)
            {
                timerManager.TowerTime(-increaseTime);
            }
            else
            {
                timerManager.timerPhotonView.RPC("TowerTime", RpcTarget.MasterClient, -increaseTime);
            }
            this.photonView.RPC("DestroyTower", RpcTarget.All, tower.GetComponent<PhotonView>().ViewID);
        }
    }

    [PunRPC]
    public void DestroyTower(int towerViewID)
    {
        GameObject targetTower = PhotonView.Find(towerViewID).gameObject;
        Destroy(targetTower.GetComponent<TowerInfo>().gauge);
        Destroy(targetTower);
    }

    void Start()
    {
        if (!this.gameObject.GetPhotonView().IsMine)
            return;

        demolishTowerButton = GameObject.Find("DemolishTowerButton").GetComponent<Button>();
        getResourceManager = GameObject.Find("GetResourceBackground").GetComponent<GetResourceManager>();
        timerManager = GameObject.Find("Timer").GetComponent<TimerManager>();
        demolishTowerButton.onClick.AddListener(OnClickDemolishTowerButton);
        itemIndex = GameObject.Find("ItemManager").GetComponent<ItemIndex>();
    }
}
