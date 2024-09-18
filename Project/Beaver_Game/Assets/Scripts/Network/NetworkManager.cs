using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Cinemachine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public CinemachineVirtualCamera cinemachineVirtualCamera;
    //public Vector3 waitingRoomPos;
    public Transform waitingRoomPos;
    public Transform[] startPos = new Transform[5];
    public TimerManager timerManager;

    public TMP_Text playerCountText;
    public Button gameStartButton;
    public Button outRoomButton;

    private GameObject myBeaver = null;

    public CinemachineManager cinemachineManager;
    public GameObject player;
    public ShowRole showRole;

    public DamManager[] damManagers;

    public bool onGame = false;

    void Start()
    {
        Screen.SetResolution(1920, 1080, false);    // 화면 가로세로 설정
        //PhotonNetwork.ConnectUsingSettings();

        
        GameObject createdBeaver = PhotonNetwork.Instantiate("PlayerBeaver", waitingRoomPos.position, Quaternion.identity);    // 플레이어 비버 생성
        myBeaver = createdBeaver;
        if (createdBeaver.GetPhotonView().IsMine)
        {
            cinemachineVirtualCamera.Follow = createdBeaver.transform;  // 플레이어와 시네머신 카메라 연결
            cinemachineVirtualCamera.LookAt = createdBeaver.transform;
        }
        
        if (PhotonNetwork.IsMasterClient)
        {
            gameStartButton.gameObject.SetActive(true);
            gameStartButton.interactable = false;
        }

        UpdatePlayerCount();

        // 엔딩 후 다시 돌아왔을 때를 위한 인원수 체크
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            gameStartButton.interactable = true;
        }

    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 5 }, null, null);
    }

    public override void OnJoinedRoom()
    {
        if (myBeaver = null)
        {
            GameObject createdBeaver = PhotonNetwork.Instantiate("PlayerBeaver", waitingRoomPos.position, Quaternion.identity);    // 플레이어 비버 생성
            if (createdBeaver.GetPhotonView().IsMine)
            {
                cinemachineVirtualCamera.Follow = createdBeaver.transform;  // 플레이어와 시네머신 카메라 연결
                cinemachineVirtualCamera.LookAt = createdBeaver.transform;
            }
        }
        
        
    }


    // 플레이어가 방에 들어왔을 때 호출되는 콜백
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerCount();

        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            gameStartButton.interactable = true;
        }
    }

    // 플레이어가 방에서 나갔을 때 호출되는 콜백
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerCount();

        if (PhotonNetwork.IsMasterClient && !onGame)
        {
            gameStartButton.gameObject.SetActive(true);

            if (PhotonNetwork.CurrentRoom.PlayerCount != PhotonNetwork.CurrentRoom.MaxPlayers)
            {
                gameStartButton.interactable = false;
            }
        }
    }

    // 플레이어 수 표시
    private void UpdatePlayerCount()
    {
        if (PhotonNetwork.InRoom && playerCountText != null)
        {
            // TextMeshPro 업데이트
            playerCountText.text = "Player Count\n" + PhotonNetwork.CurrentRoom.PlayerCount + " / " + PhotonNetwork.CurrentRoom.MaxPlayers;
        }
    }

    // 게임 시작 버튼
    public void OnClickGameStartButton()
    {
        //player.GetComponent<PhotonView>().RPC("SetSpyBool", RpcTarget.All, true);
        //myBeaver.GetComponent<PhotonView>().RPC("SetSpyBool", RpcTarget.All, false);
        //showRole.SetShowRuleImage(false);

        this.gameObject.GetPhotonView().RPC("CallSetSpyBool", RpcTarget.All, false);

        if (PhotonNetwork.IsMasterClient)
        {
            timerManager.SetTimerOn();
            gameStartButton.gameObject.SetActive(false);
            PhotonNetwork.CurrentRoom.IsOpen = false;   // 현재 방에 플레이어가 들어오지 못 하도록
            PhotonNetwork.CurrentRoom.IsVisible = false;    // 현재 방이 표시되지 않도록 숨기기

            int spyNum = Random.Range(0, PhotonNetwork.CurrentRoom.PlayerCount);

            Debug.Log("Player Count: " + PhotonNetwork.CurrentRoom.PlayerCount);
            Debug.Log("Spy Number: " + spyNum);
            Debug.Log("Selected Player: " + PhotonNetwork.PlayerList[spyNum].NickName); // 플레이어의 NickName 확인
            Debug.Log("TagObject: " + PhotonNetwork.PlayerList[spyNum].TagObject);

            //player.GetComponent<PhotonView>().RPC("SetSpyBool", PhotonNetwork.PlayerList[spyNum], true);
            this.gameObject.GetPhotonView().RPC("CallSetSpyBool", PhotonNetwork.PlayerList[spyNum], true);

            for(int i = 0; i < damManagers.Length; i++)
            {
                damManagers[i].DamRandomPrice();
            }

            //showRole.SetShowRuleImage(true);
        }


        this.gameObject.GetPhotonView().RPC("SetStartSetting", RpcTarget.All);
    }

    [PunRPC]
    public void CallSetSpyBool(bool isSpy)
    {
        Debug.Log("스파이 결정");
        myBeaver.GetComponent<SpyBoolManager>().SetSpyBool(isSpy);
    }

    public void OnClickOutRoomButton()
    {
        if (PhotonNetwork.InRoom)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;   // 현재 방에 플레이어가 들어오지 못 하도록
                PhotonNetwork.CurrentRoom.IsVisible = false;    // 현재 방이 표시되지 않도록 숨기기
            }

            PhotonNetwork.LeaveRoom();
        }
    }

    // 방을 성공적으로 나간 후 호출되는 콜백
    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        print("OnLeftRoom");

        SceneManager.LoadScene("LobbyScene");

        /*
        // 방을 나간 후 로비에 자동으로 입장하도록 JoinLobby() 호출
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby(); // 로비로 다시 입장
        }
        else
        {
            SceneManager.LoadScene("LobbyScene");
        }
        */
    }

    // 로비에 성공적으로 입장한 후 호출되는 콜백
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        print("OnJoinedLobby");
        SceneManager.LoadScene("LobbyScene"); // 로비에 입장한 후에 씬 전환
    }


    [PunRPC]
    public void SetStartSetting()
    {
        showRole.gameObject.SetActive(true);

        myBeaver.transform.position = startPos[myBeaver.layer - 6].position;
        playerCountText.gameObject.SetActive(false);
        outRoomButton.gameObject.SetActive(false);
        cinemachineManager.SetCameraRange(1);   // 카메라 범위를 대기방에서 게임 맵으로 변경
        onGame = true;
    }

    /*
    public void CreateItem(string itemName, Vector3 createPos)    // 자원 채취칸에서 버튼 누르면 자원 아이템 필드에 생성
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
