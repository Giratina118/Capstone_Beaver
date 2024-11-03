using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMove : MonoBehaviourPunCallbacks, IPunObservable
{
    public float moveSpeed = 10.0f;  // ������ ���� �̵� �ӵ�
    public float runSpeed = 10.0f; // ���� �̵� �ӵ�
    public float swimSpeed = 6.0f;  // ���� �ӵ�
    public bool leftRightChange = false;    // �¿� ���� ����
    public RopeManager ropeManager; // ���� ���� ��
    Animator animator;  // ��� �ִϸ��̼�
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

    public void EquippedItemPos()   // ���� ������ ��ġ
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
        if (stream.IsWriting)   // ���� ������ �ֳ�(������)
        {   // ���� �ٲ�� �� ��
            stream.SendNext(playerRigidbody2D.position);
            stream.SendNext(playerRigidbody2D.velocity);
        }
        else    // �޴� ��� �о����
        {
            playerRigidbody2D.position = (Vector3)stream.ReceiveNext();
            playerRigidbody2D.velocity = (Vector3)stream.ReceiveNext();

            float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.timestamp));    // ������ ���
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
            float moveX = Input.GetAxis("Horizontal"); // x�� �̵�
            float moveY = Input.GetAxis("Vertical");   // y�� �̵�

            if (!animator.GetBool("Walk") && (moveX != 0.0f || moveY != 0.0f)) // �ִϸ����� ����
            {
                animator.SetBool("Walk", true);
                EquippedItemPos();  // ��� ��ġ ����
                soundEffectManager.PlayPalyerAudio();
            }
            else if (animator.GetBool("Walk") && (moveX == 0.0f && moveY == 0.0f))
            {
                animator.SetBool("Walk", false);
                EquippedItemPos();  // ��� ��ġ ����
                soundEffectManager.StopPlayerAudio();
            }

            if (!animator.GetBool("InWater") && (moveX != 0.0f || moveY != 0.0f) && soundEffectManager.playerAudioSource.clip != soundEffectManager.audioClips[1])
            {
                soundEffectManager.SetPlayerAudioClip(1);
                soundEffectManager.PlayPalyerAudio();
            }

            if (moveX < 0.0f && !leftRightChange)   // �¿� ���� ����
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
