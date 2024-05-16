using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerInfo : MonoBehaviour
{
    public int[] requiredResourceOfTowers = new int[4]; // ����ž(Ÿ��) ���ۿ� �ʿ��� �ڿ�
    public float remainComunicationTime = 20.0f;    // ��� �ð� ����

    public GameObject gauge = null; // ��� ������

    [SerializeField]
    private float gaugePlusYPos = 0.8f; // �������� ž�� �߽ɺ��� ���� ���� ��ġ�ϵ���


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
