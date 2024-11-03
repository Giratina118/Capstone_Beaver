using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMove : MonoBehaviourPunCallbacks
{
    public float moveSpeed = 10.0f;  // 설정된 현재 이동 속도
    public float runSpeed = 10.0f; // 육상 이동 속도
    public float swimSpeed = 6.0f;  // 수영 속도
    public bool leftRightChange = false;    // 좌우 반전 여부
    public RopeManager ropeManager; // 로프 예비 선
    Animator animator;  // 비버 애니메이션
    private Rigidbody2D playerRigidbody2D;
    //public NavMeshAgent navMeshAgent;

    private Vector3 remotePosition;
    public SoundEffectManager soundEffectManager;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!this.gameObject.GetPhotonView().IsMine)
            return;

        /*
        if (collision.gameObject.tag == "Water" && navMeshAgent != null)
        {
            navMeshAgent.speed *= 0.6f;
        }
        */

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

        /*
        if (collision.gameObject.tag == "Water" && navMeshAgent != null)
        {
            navMeshAgent.speed /= 0.6f;
        }
        */

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
                /*
                // 나중에는 아이템별로 다 넣어서 위치 조정하기
                if (this.transform.GetChild(i).gameObject.GetComponent<ItemInfo>().GetItemIndexNumber() == 18)  // 18번 아이템은 손톱, 현재 이것만 걷는거 따라서 위치 조정 함
                {
                    if (animator.GetBool("Walk"))   // 걷기 상태일때와 아닐때 아이템의 위치 다르게 조정
                    {
                        this.transform.GetChild(i).localPosition = new Vector3(-1.25f, -3.3f, 0.0f);
                        this.transform.GetChild(i).localRotation = Quaternion.Euler(0.0f, 0.0f, -40.0f);
                    }
                    else
                    {
                        this.transform.GetChild(i).localPosition = new Vector3(1.25f, -3.3f, 0.0f);
                        this.transform.GetChild(i).localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                    }
                    break;
                }
                */

                
                ItemInfo nowItemInfo = this.transform.GetChild(i).gameObject.GetComponent<ItemInfo>();


                if (animator.GetBool("InWater"))
                {
                    this.transform.GetChild(i).localPosition = nowItemInfo.swimPos;
                    this.transform.GetChild(i).localRotation = Quaternion.Euler(nowItemInfo.swimRot);
                    this.transform.GetChild(i).localScale = nowItemInfo.swimScale;
                }
                else if (animator.GetBool("Walk"))
                {
                    this.transform.GetChild(i).localPosition = nowItemInfo.walkPos;
                    this.transform.GetChild(i).localRotation = Quaternion.Euler(nowItemInfo.walkRot);
                    this.transform.GetChild(i).localScale = nowItemInfo.walkScale;
                }
                else
                {
                    this.transform.GetChild(i).localPosition = nowItemInfo.normalPos;
                    this.transform.GetChild(i).localRotation = Quaternion.Euler(nowItemInfo.normalRot);
                    this.transform.GetChild(i).localScale = nowItemInfo.normalScale;
                }

                /*
                switch (nowItemInfo.GetItemIndexNumber())
                {
                    case 0:
                        break;
                    case 1:
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                    case 4:
                        break;
                    case 5:
                        break;
                    case 6:
                        break;
                    case 7:     // 이동속도 증가시키는 미끼 달린 모자
                        if (animator.GetBool("Walk"))   // 걷기 상태일때와 아닐때 아이템의 위치 다르게 조정
                        {
                            //this.transform.GetChild(i).localPosition = nowItemInfo.walkPos;
                            //this.transform.GetChild(i).localPosition = new Vector3(0.8f, 5.0f, 0.0f);
                        }
                        else
                        {
                            //this.transform.GetChild(i).localPosition = nowItemInfo.normalPos;
                            //this.transform.GetChild(i).localPosition = new Vector3(1.3f, 5.0f, 0.0f);
                        }
                        break;
                    case 8:
                        break;
                    case 9:
                        break;
                    case 10:
                        break;
                    case 11:
                        break;
                    case 12:
                        break;
                    case 13:
                        break;
                    case 14:
                        break;
                    case 15:
                        break;
                    case 16:
                        break;
                    case 17:
                        break;
                    case 18:    // 손톱
                        if (animator.GetBool("Walk"))   // 걷기 상태일때와 아닐때 아이템의 위치 다르게 조정
                        {
                            this.transform.GetChild(i).localPosition = new Vector3(-1.25f, -3.3f, 0.0f);
                            this.transform.GetChild(i).localRotation = Quaternion.Euler(0.0f, 0.0f, -40.0f);
                        }
                        else
                        {
                            this.transform.GetChild(i).localPosition = new Vector3(1.25f, -3.3f, 0.0f);
                            this.transform.GetChild(i).localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                        }
                        break;
                    case 19:
                        break;
                    case 20:
                        break;
                    case 21:
                        break;
                    case 22:
                        break;
                    default:
                        break;
                }
                */
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
        //navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        ropeManager = this.transform.GetChild(0).GetComponent<RopeManager>();
        playerRigidbody2D = this.GetComponent<Rigidbody2D>();

        if (this.GetComponent<PhotonView>().IsMine)
            soundEffectManager = GameObject.Find("SoundEffectManager").GetComponent<SoundEffectManager>();


        //navMeshAgent.updatePosition = false;
        //navMeshAgent.updateRotation = false;


    }

    void Update()
    {

        if (this.GetComponent<PhotonView>().IsMine)
        {
            // * moveSpeed * Time.deltaTime
            float moveX = Input.GetAxis("Horizontal"); // x축 이동
            float moveY = Input.GetAxis("Vertical");   // y축 이동

            if (!animator.GetBool("Walk") && (moveX != 0.0f || moveY != 0.0f)) // 애니메이터 설정
            {
                animator.SetBool("Walk", true);
                EquippedItemPos();  // 장비 위치 조정

                //soundEffectManager.SetPlayerAudioClip(1);
                soundEffectManager.PlayPalyerAudio();
            }
            else if (animator.GetBool("Walk") && (moveX == 0.0f && moveY == 0.0f))
            {
                animator.SetBool("Walk", false);
                EquippedItemPos();  // 장비 위치 조정
                soundEffectManager.StopPlayerAudio();
                
            }
            //EquippedItemPos();  // 장비 위치 조정

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

            //transform.Translate(new Vector3(moveX, moveY, 0.0f));   // 이동

            playerRigidbody2D.velocity = new Vector3(moveX, moveY, 0.0f).normalized * moveSpeed;

            /*
            Vector3 movement = new Vector3(moveX, moveY, 0.0f);

            // Rigidbody2D를 사용하여 방향키로 이동합니다.
            //rb.velocity = movement * moveSpeed;

            // NavMeshAgent에도 이동 목적지를 설정합니다.
            if (movement != Vector3.zero)
            {
                
                Vector3 moveDestination = transform.position + new Vector3(moveX, moveY, 0.0f) * 5.0f;
                navMeshAgent.SetDestination(moveDestination);
                
                
                rigidbody2D.velocity = new Vector3(moveX, moveY, 0.0f).normalized * moveSpeed;

            }
            else
            {
                navMeshAgent.SetDestination(transform.position);
            }
            */
        }

    }
}
