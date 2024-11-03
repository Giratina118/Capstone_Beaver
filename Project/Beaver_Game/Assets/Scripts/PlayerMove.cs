using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMove : MonoBehaviourPunCallbacks, IPunObservable
{
    public float moveSpeed = 10.0f;  // 설정된 현재 이동 속도
    public float runSpeed = 10.0f; // 육상 이동 속도
    public float swimSpeed = 6.0f;  // 수영 속도
    public bool leftRightChange = false;    // 좌우 반전 여부
    public RopeManager ropeManager; // 로프 예비 선
    Animator animator;  // 비버 애니메이션
    private Rigidbody2D playerRigidbody2D;

    private Vector3 remotePosition;
    public SoundEffectManager soundEffectManager;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!this.gameObject.GetPhotonView().IsMine)
            return;

        if (collision.gameObject.tag == "Water")
        {
            moveSpeed = swimSpeed;
            animator.SetBool("InWater", true);
            EquippedItemPos();

            soundEffectManager.SetPlayerAudioClip(2);
            soundEffectManager.PlayPalyerAudio();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!this.gameObject.GetPhotonView().IsMine)
            return;

        if (collision.gameObject.tag == "Water")
        {
            moveSpeed = runSpeed;
            animator.SetBool("InWater", false);
            EquippedItemPos();

            soundEffectManager.SetPlayerAudioClip(1);
            soundEffectManager.PlayPalyerAudio();
        }
    }

    public void EquippedItemPos()   // 장착 아이템 위치
    {
        if (this.transform.childCount > 2)  // 
        {
            for (int i = 2; i < this.transform.childCount; i++)
            {
                Transform nowItem = this.transform.GetChild(i);
                ItemInfo nowItemInfo = nowItem.gameObject.GetComponent<ItemInfo>();

                if (animator.GetBool("InWater"))
                {
                    nowItem.localPosition = nowItemInfo.swimPos;
                    nowItem.localRotation = Quaternion.Euler(nowItemInfo.swimRot);
                    nowItem.localScale = nowItemInfo.swimScale;
                }
                else if (animator.GetBool("Walk"))
                {
                    nowItem.localPosition = nowItemInfo.walkPos;
                    nowItem.localRotation = Quaternion.Euler(nowItemInfo.walkRot);
                    nowItem.localScale = nowItemInfo.walkScale;
                }
                else
                {
                    nowItem.localPosition = nowItemInfo.normalPos;
                    nowItem.localRotation = Quaternion.Euler(nowItemInfo.normalRot);
                    nowItem.localScale = nowItemInfo.normalScale;
                }
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)   // 쓰는 권한이 있냐(내꺼냐)
        {   // 순서 바뀌면 안 됨
            stream.SendNext(playerRigidbody2D.position);
            stream.SendNext(playerRigidbody2D.velocity);
        }
        else    // 받는 사람 읽어오기
        {
            playerRigidbody2D.position = (Vector3)stream.ReceiveNext();
            playerRigidbody2D.velocity = (Vector3)stream.ReceiveNext();

            float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.timestamp));    // 딜레이 계산
            playerRigidbody2D.position += playerRigidbody2D.velocity * lag;
        }
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        ropeManager = this.transform.GetChild(0).GetComponent<RopeManager>();
        playerRigidbody2D = this.GetComponent<Rigidbody2D>();

        if (this.GetComponent<PhotonView>().IsMine)
            soundEffectManager = GameObject.Find("SoundEffectManager").GetComponent<SoundEffectManager>();
    }

    void Update()
    {
        if (this.GetComponent<PhotonView>().IsMine)
        {
            float moveX = Input.GetAxis("Horizontal"); // x축 이동
            float moveY = Input.GetAxis("Vertical");   // y축 이동

            if (!animator.GetBool("Walk") && (moveX != 0.0f || moveY != 0.0f)) // 애니메이터 설정
            {
                animator.SetBool("Walk", true);
                EquippedItemPos();  // 장비 위치 조정
                soundEffectManager.PlayPalyerAudio();
            }
            else if (animator.GetBool("Walk") && (moveX == 0.0f && moveY == 0.0f))
            {
                animator.SetBool("Walk", false);
                EquippedItemPos();  // 장비 위치 조정
                soundEffectManager.StopPlayerAudio();
            }

            if (!animator.GetBool("InWater") && (moveX != 0.0f || moveY != 0.0f) && soundEffectManager.playerAudioSource.clip != soundEffectManager.audioClips[1])
            {
                soundEffectManager.SetPlayerAudioClip(1);
                soundEffectManager.PlayPalyerAudio();
            }

            if (moveX < 0.0f && !leftRightChange)   // 좌우 반전 설정
            {
                leftRightChange = true;
                this.transform.localScale = new Vector3(this.transform.localScale.x * -1, this.transform.localScale.y, this.transform.localScale.z);
                ropeManager.ThrowRopeLineLeftRightChange();
            }
            else if (moveX > 0.0f && leftRightChange)
            {
                leftRightChange = false;
                this.transform.localScale = new Vector3(this.transform.localScale.x * -1, this.transform.localScale.y, this.transform.localScale.z);
                ropeManager.ThrowRopeLineLeftRightChange();
            }

            playerRigidbody2D.velocity = new Vector3(moveX, moveY, 0.0f).normalized * moveSpeed;
        }
    }
}
