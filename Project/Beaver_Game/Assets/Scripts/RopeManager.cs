using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RopeManager : MonoBehaviourPunCallbacks
{
    public GameObject ropePrefab;   // 로프(던져지는 모습) 프리팹
    public Button throwRopeButton;  // 로프 던지기 버튼
    private GameObject nowRope = null;

    public void ThrowRopeLineLeftRightChange()  // 좌우 반전에 따른 로프 던지기 조준선 조정
    {
        this.transform.localScale = new Vector3(this.transform.localScale.x * -1, this.transform.localScale.y, this.transform.localScale.z);
    }

    public void OnClickThrowRopeButton()    // 로프 던지기 버튼 클릭하면 조준선 나타남
    {
        if (this.nowRope == null)
            transform.GetChild(0).gameObject.SetActive(true);
    }

    void Start()
    {
        if (!this.gameObject.transform.parent.gameObject.GetPhotonView().IsMine)
            return;

        // 로트 던지기 버튼 누르면 OnClickThrowRopeButton 함수가 작동되도록 연결
        throwRopeButton = GameObject.Find("ThrowRopeButton").GetComponent<Button>();
        throwRopeButton.onClick.AddListener(OnClickThrowRopeButton);
        //throwRopeButton.gameObject.SetActive(false);
        throwRopeButton.enabled = false;
    }

    void Update()
    {
        if (this.transform.GetChild(0).gameObject.activeSelf && this.transform.parent.gameObject.GetPhotonView().IsMine)   // 조준선이 나타난 상태라면
        {
            // 조준선이 마우스를 따라 회전하도록
            Vector3 rot = Camera.main.ScreenToWorldPoint(Input.mousePosition) - this.transform.position + Vector3.forward * 10;
            float angle = Mathf.Atan2(rot.y, rot.x) * Mathf.Rad2Deg;
            this.transform.rotation = Quaternion.Euler(0, 0, angle);

            // 마우스 좌클릭하면 로프 던짐
            if (Input.GetMouseButtonDown(0))
            {
                nowRope = PhotonNetwork.Instantiate(ropePrefab.name, this.transform.position + rot.normalized * 1.5f, Quaternion.Euler(0, 0, angle + 180.0f));
                //GameObject newRope = Instantiate(ropePrefab);   // 로프(던져지는 모습) 생성
                //newRope.transform.position = this.transform.position + rot.normalized * 3.5f;   // 로프 위치 조정, 3.5f는 자기 자신에게 맞지 않게 하려고
                //newRope.GetComponent<RopeCollision>().SetDirection(rot.normalized);
                //newRope.transform.localRotation = Quaternion.Euler(0, 0, angle + 180.0f);   // 로프 각도 조정

                if (this.transform.localRotation.eulerAngles.z < 90.0f || this.transform.localRotation.eulerAngles.z > 270.0f)  // 로프의 위 아래 이미지가 뒤집히지 않도록 조정
                {
                    nowRope.transform.localScale = new Vector3(0.25f, -0.2f, 0.0f);
                }

                transform.GetChild(0).gameObject.SetActive(false);  // 조준선 없애기
            }
            
        }
    }
}
