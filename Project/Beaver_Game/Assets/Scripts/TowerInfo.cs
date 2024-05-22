using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerInfo : MonoBehaviourPunCallbacks
{
    public int[] requiredResourceOfTowers = new int[4]; // ����ž(Ÿ��) ���ۿ� �ʿ��� �ڿ�
    public float remainComunicationTime = 20.0f;    // ��� �ð� ����

    public GameObject gauge = null; // ��� ������

    [SerializeField]
    private float gaugePlusYPos = 0.8f; // �������� ž�� �߽ɺ��� ���� ���� ��ġ�ϵ���

    private Transform cnavasGaugesTransform; // ��� �������� �θ� ��ġ
    private Transform towerParentTransfotm;

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
            gauge = gaugePhotonView.gameObject; // gauge ��ü�� �θ� ���� �Ǵ� ��Ÿ �ʱ�ȭ �۾� ����
            gauge.transform.SetParent(cnavasGaugesTransform);
        }

        if (!gaugePhotonView.IsMine)
        {
            gaugePhotonView.gameObject.SetActive(false);
        }

    }


    void Start()
    {
        
    }

    void Update()
    {
        if (gauge != null && gauge.activeSelf)  // ������ ��ġ ����(UI��)
        {
            gauge.transform.position = Camera.main.WorldToScreenPoint(new Vector3(this.transform.position.x, this.transform.position.y + gaugePlusYPos, 0.0f));
        }


        
    }
}
