using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeCollision : MonoBehaviour
{
    //private Transform prisonTransform;  // 감옥 위치
    //public Vector3 direction = Vector3.zero;    
    private InventorySlotGroup inventorySlotGroup;  // 인벤토리
    public int ropeIndexNum = 4;   // 로프의 아이템 번호가 바뀌면 바꿔줘야 함
    public float lifeTime = 5.0f;   // 로프가 날아가는 시간

    /*
    public void SetDirection(Vector3 dir)
    {
        direction = dir;
    }
    */

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")   // 던진 로프가 플레이어에 맞으면
        {
            Player targetPlayer = PhotonNetwork.CurrentRoom.GetPlayer(collision.gameObject.GetPhotonView().ViewID);
            //PhotonView photonView = collision.gameObject.GetPhotonView();
            collision.gameObject.GetPhotonView().RPC("CaughtByRope", targetPlayer);

            //collision.gameObject.transform.position = prisonTransform.position; // 맞은 플레리어를 감옥으로 위프
            //collision.gameObject.GetComponent<PrisonManager>().CaughtByRope();  // 감옥의 시스템 작동

            if (this.gameObject.GetPhotonView().IsMine)
                inventorySlotGroup.UseItem(ropeIndexNum, 1, false);  // 로프의 아이템 번호(4), 1개 사용

            Destroy(this.gameObject);   // 던진 로프 없앰
        }
    }

    void Start()
    {
        inventorySlotGroup = GameObject.Find("InventorySlots").GetComponent<InventorySlotGroup>();
        //prisonTransform = GameObject.Find("Prison").transform;

        Destroy(this.gameObject, lifeTime); // 생긴(던져진) 직후 lifeTime이 다 되면 이 로프를 삭제 
    }

    void Update()
    {
        //this.transform.Translate(direction * Time.deltaTime * 5.0f);

        this.transform.Translate(Vector3.left * Time.deltaTime * 5.0f); // 라이프타임
    }
}
