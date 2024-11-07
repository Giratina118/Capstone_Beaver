using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerInfo : MonoBehaviourPunCallbacks
{
    public int[] requiredResourceOfTowers = new int[4]; // 접파탑(타워) 제작에 필요한 자원
    public float remainComunicationTime = 20.0f;    // 통신 시간 제한

    public GameObject gauge = null; // 통신 게이지

    [SerializeField]
    private float gaugePlusYPos = 0.8f; // 게이지가 탑의 중심보다 조금 위에 위치하도록

    private Transform cnavasGaugesTransform; // 통신 게이지의 부모 위치
    private Transform towerParentTransfotm;
    public Animator comunicationEffect;


    [PunRPC]
    public void SetGauge(int gaugeViewID)
    {
        Debug.Log(gaugeViewID);
        cnavasGaugesTransform = GameObject.Find("Gauges").transform;
        towerParentTransfotm = GameObject.Find("Towers").transform;
        this.transform.SetParent(towerParentTransfotm);

        PhotonView gaugePhotonView = PhotonView.Find(gaugeViewID);
        if (gaugePhotonView != null)
        {
            gauge = gaugePhotonView.gameObject; // gauge 객체의 부모 설정 또는 기타 초기화 작업 수행
            gauge.transform.SetParent(cnavasGaugesTransform);
        }

        if (!gaugePhotonView.IsMine)
        {
            gaugePhotonView.gameObject.SetActive(false);
        }
    }

    [PunRPC]
    public void UpdateFillAmountofGauge(float remainTime)
    {
        gauge.transform.GetChild(2).gameObject.GetComponent<Image>().fillAmount = 1 - remainTime / 20.0f; // 통신 게이지, 수치가 점차 올라감, 최대가 1.0, 최소 0.0
    }

    [PunRPC]
    public void SetTowerComunicationEffect(bool isComunicate)
    {
        comunicationEffect.gameObject.SetActive(isComunicate);
        comunicationEffect.SetBool("isComunicate", isComunicate);
    }

    void Update()
    {
        if (gauge != null && gauge.activeSelf)  // 게이지 위치 조정(UI라서)
        {
            gauge.transform.position = Camera.main.WorldToScreenPoint(new Vector3(this.transform.position.x, this.transform.position.y + gaugePlusYPos, 0.0f));
        } 
    }
}
