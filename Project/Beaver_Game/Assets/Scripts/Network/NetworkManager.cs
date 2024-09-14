using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Cinemachine;
using TMPro;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public CinemachineVirtualCamera cinemachineVirtualCamera;
    public Vector3 waitingRoomPos;
    public Transform[] startPos = new Transform[5];
    public TimerManager timerManager;

    public TMP_Text playerCountText;
    public Button gameStartButton;

    private GameObject myBeaver;


    void Start()
    {
        Screen.SetResolution(1920, 1080, false);    // ȭ�� ���μ��� ����
        PhotonNetwork.ConnectUsingSettings();

        
        GameObject createdBeaver = PhotonNetwork.Instantiate("PlayerBeaver", waitingRoomPos, Quaternion.identity);    // �÷��̾� ��� ����
        myBeaver = createdBeaver;
        if (createdBeaver.GetPhotonView().IsMine)
        {
            cinemachineVirtualCamera.Follow = createdBeaver.transform;  // �÷��̾�� �ó׸ӽ� ī�޶� ����
            cinemachineVirtualCamera.LookAt = createdBeaver.transform;
        }
        
        if (PhotonNetwork.IsMasterClient)
        {
            gameStartButton.gameObject.SetActive(true);
            gameStartButton.interactable = false;
        }

        UpdatePlayerCount();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 5 }, null, null);
    }

    public override void OnJoinedRoom()
    {

        GameObject createdBeaver = PhotonNetwork.Instantiate("PlayerBeaver", waitingRoomPos, Quaternion.identity);    // �÷��̾� ��� ����
        if (createdBeaver.GetPhotonView().IsMine)
        {
            cinemachineVirtualCamera.Follow = createdBeaver.transform;  // �÷��̾�� �ó׸ӽ� ī�޶� ����
            cinemachineVirtualCamera.LookAt = createdBeaver.transform;
        }
        
    }


    // �÷��̾ �濡 �����ų� ������ �� ȣ��Ǵ� �ݹ�
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerCount();

        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            gameStartButton.interactable = true;
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerCount();

        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount != PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            gameStartButton.interactable = false;
        }
    }

    private void UpdatePlayerCount()
    {
        if (PhotonNetwork.InRoom && playerCountText != null)
        {
            // TextMeshPro ������Ʈ
            playerCountText.text = "Player Count\n" + PhotonNetwork.CurrentRoom.PlayerCount + " / " + PhotonNetwork.CurrentRoom.MaxPlayers;
        }
    }

    public void OnClickGameStartButton()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            timerManager.SetTimerOn();
            gameStartButton.gameObject.SetActive(false);
        }

        this.gameObject.GetPhotonView().RPC("SetStartSetting", RpcTarget.All);
    }

    [PunRPC]
    public void SetStartSetting()
    {
        myBeaver.transform.position = startPos[myBeaver.layer - 6].position;
        playerCountText.gameObject.SetActive(false);
    }

    /*
    public void CreateItem(string itemName, Vector3 createPos)    // �ڿ� ä��ĭ���� ��ư ������ �ڿ� ������ �ʵ忡 ����
    {
        GameObject newItem = PhotonNetwork.Instantiate(itemName, createPos, Quaternion.identity);
        //newResource.transform.position = resourcePos;
    }
    */

    /*
    public void StorageResource()
    {
        //PhotonNetwork.
    }
    */

}
