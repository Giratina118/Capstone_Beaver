using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerInfo : MonoBehaviour
{
    public int[] requiredResourceOfTowers = new int[4]; // 접파탑(타워) 제작에 필요한 자원
    public float remainComunicationTime = 20.0f;    // 통신 시간 제한

    public GameObject gauge = null; // 통신 게이지

    [SerializeField]
    private float gaugePlusYPos = 0.8f; // 게이지가 탑의 중심보다 조금 위에 위치하도록


    void Start()
    {
        
    }

    void Update()
    {
        if (gauge != null && gauge.activeSelf)  // 게이지 위치 조정(UI라서)
        {
            gauge.transform.position = Camera.main.WorldToScreenPoint(new Vector3(this.transform.position.x, this.transform.position.y + gaugePlusYPos, 0.0f));
        }


        
    }
}
