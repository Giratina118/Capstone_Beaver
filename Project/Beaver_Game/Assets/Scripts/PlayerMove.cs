using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 10.0f; // 이동 속도
    public bool leftRightChange = false;    // 좌우 반전 여부
    public RopeManager ropeManager; // 로프 예비 선
    Animator animator;  // 비버 애니메이션
    public NavMeshAgent navMeshAgent;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!this.gameObject.GetPhotonView().IsMine)
            return;

        if (collision.gameObject.tag == "Water" && navMeshAgent != null)
        {
            navMeshAgent.speed *= 0.6f;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!this.gameObject.GetPhotonView().IsMine)
            return;

        if (collision.gameObject.tag == "Water" && navMeshAgent != null)
        {
            navMeshAgent.speed /= 0.6f;
        }
    }



    void EquippedItemPos()   // 손톱 아이템 위치
    {
        if (this.transform.childCount > 2)  // 
        {
            for (int i = 2; i < this.transform.childCount; i++)
            {
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
            }
        }
    }

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        ropeManager = this.transform.GetChild(0).GetComponent<RopeManager>();
        
        
        //navMeshAgent.updatePosition = false;
        //navMeshAgent.updateRotation = false;
        
        
    }

    void Update()
    {
        if (!this.GetComponent<PhotonView>().IsMine)    // 자신의 캐릭터만 움직이도록
        {
            return;
        }

        float moveX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime; // x축 이동
        float moveY = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;   // y축 이동

        if (moveX != 0.0f || moveY != 0.0f) // 애니메이터 설정
        {
            animator.SetBool("Walk", true);
        }
        else
        {
            animator.SetBool("Walk", false);
        }
        EquippedItemPos();  // 장비 위치 조정

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



        Vector3 movement = new Vector3(moveX, moveY, 0.0f);

        // Rigidbody2D를 사용하여 방향키로 이동합니다.
        //rb.velocity = movement * moveSpeed;

        // NavMeshAgent에도 이동 목적지를 설정합니다.
        if (movement != Vector3.zero)
        {
            Vector3 moveDestination = transform.position + new Vector3(moveX, moveY, 0.0f) * 5.0f;
            navMeshAgent.SetDestination(moveDestination);
        }
        else
        {
            navMeshAgent.SetDestination(transform.position);
        }


        Vector3 agentPosition = navMeshAgent.transform.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(agentPosition, out hit, 1.0f, NavMesh.AllAreas))
        {
            int areaIndex = hit.mask;
            Debug.Log("현재 위치의 NavMesh 영역 값: " + areaIndex);
        }

    }
}
