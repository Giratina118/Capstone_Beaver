using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public InputField inputRoomName; // 방 이름을 입력받는 InputField
    public Button btnJoin;           // 방 참가 버튼
    public Button btnCreate;         // 방 생성 버튼
    public InputField inputMaxPlayers; // 최대 인원수를 입력받는 InputField
    public GameObject roomListItem;
    public Transform rtContent;

    Dictionary<string, RoomInfo> dicRoomInfo = new Dictionary<string, RoomInfo>();


    void Start()
    {
        // 초기에는 버튼들을 비활성화
        btnJoin.interactable = false;
        btnCreate.interactable = false;

        // InputField의 값이 변경될 때 호출되는 리스너 추가
        inputRoomName.onValueChanged.AddListener(OnRoomNameValueChanged);
        inputMaxPlayers.onValueChanged.AddListener(OnMaxPlayerValueChanged);


        /*
        // 방을 나간 후 로비에 자동으로 입장하도록 JoinLobby() 호출
        if (PhotonNetwork.IsConnectedAndReady)
        {
            if (!PhotonNetwork.InLobby)
            {
                Debug.Log("아아아");
                PhotonNetwork.JoinLobby(); // 로비로 다시 입장
            }
        }
        else
        {
            // 마스터 서버에 연결되어 있지 않다면 연결을 시도
            PhotonNetwork.ConnectUsingSettings();
        }

        if (PhotonNetwork.NetworkClientState == ClientState.Disconnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
        else if (PhotonNetwork.NetworkClientState == ClientState.ConnectedToMasterServer)
        {
            if (!PhotonNetwork.InLobby)
            {
                PhotonNetwork.JoinLobby(); // 로비로 입장
            }
        }
        */
    }

    // 방에서 나왔을때 다시 로비 연결 업데이트
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("마스터 서버에 연결되었습니다.");
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby(); // 로비에 입장
        }
    }

    // 로비에 성공적으로 입장한 후 호출되는 콜백
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        print("OnJoinedLobby");
    }


    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);
        //Content에 자식으로 붙어있는 Item을 다 삭제
        DeleteRoomListItem();
        //dicRoomInfo 변수를 roomList를 이용해서 갱신
        UpdateRoomListItem(roomList);
        //dicRoom을 기반으로 roomListItem을 만들자
        CreateRoomListItem();

    }
    void SelectRoomItem(string roomName)
    {
        inputRoomName.text = roomName;
    }
    void DeleteRoomListItem()
    {

        foreach (Transform tr in rtContent)
        {
            Destroy(tr.gameObject);
        }
    }
    void UpdateRoomListItem(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            //dicRoomInfo에 info 의 방이름으로 되어있는 key값이 존재하는가
            if (dicRoomInfo.ContainsKey(info.Name))
            {
                //만약에 방이 삭제되었으면?
                if (info.RemovedFromList)
                {
                    dicRoomInfo.Remove(info.Name); //삭제
                    continue;
                }
            }
            dicRoomInfo[info.Name] = info; //추가
        }
    }
    void CreateRoomListItem()
    {
        foreach (RoomInfo info in dicRoomInfo.Values)
        {
            //방 정보 생성과 동시에 ScrollView-> Content의 자식으로 하자
            GameObject go = Instantiate(roomListItem, rtContent);
            //생성된 item에서 RoomListItem 컴포넌트를 가져온다.
            RoomListItem item = go.GetComponent<RoomListItem>();
            //가져온 컴포넌트가 가지고 있는 SetInfo 함수 실행
            item.SetInfo(info.Name, info.PlayerCount, info.MaxPlayers);
            //item 클릭되었을 때 호출되는 함수 등록
            item.onDelegate = SelectRoomItem;
        }
    }
    void OnNameValueChanged(string s)
    {
        btnJoin.interactable = s.Length > 0;
        if (inputRoomName.text == "")
            btnCreate.interactable = false;
    }
    void OnPlayerValueChange(string s)
    {
        btnCreate.interactable = s.Length > 0;
        if (inputMaxPlayers.text == "")
            btnCreate.interactable = false;
    }






    // 방을 생성하는 메소드
    public void CreateRoom()
    {
        string roomName = inputRoomName.text; // 입력받은 방 이름
        byte maxPlayers;
        
        // 입력받은 최대 인원수를 byte 타입으로 변환, 실패 시 기본값 5 설정
        if (!byte.TryParse(inputMaxPlayers.text, out maxPlayers))
        {
            maxPlayers = 5;
        }

        // 방 옵션 설정
        RoomOptions roomOptions = new RoomOptions
        {
            MaxPlayers = maxPlayers, // 최대 인원수 설정
            IsVisible = true         // 방이 공개되도록 설정
        };

        // 방 생성 요청
        PhotonNetwork.CreateRoom(roomName, roomOptions);
    }

    // 방 참가 메소드
    public void JoinRoom()
    {
        string roomName = inputRoomName.text; // 입력받은 방 이름
        PhotonNetwork.JoinRoom(roomName);     // 방 참가 요청
    }

    // 방 이름이 변경될 때 호출되는 메소드
    private void OnRoomNameValueChanged(string room)
    {
        bool isInteractable = !string.IsNullOrEmpty(room); // 방 이름이 비어있지 않은지 확인
        btnJoin.interactable = isInteractable;             // 방 이름이 있을 때만 참가 버튼 활성화
        btnCreate.interactable = isInteractable && !string.IsNullOrEmpty(inputMaxPlayers.text); // 방 이름과 최대 인원수가 있을 때만 생성 버튼 활성화
    }

    // 최대 인원수가 변경될 때 호출되는 메소드
    private void OnMaxPlayerValueChanged(string max)
    {
        btnCreate.interactable = !string.IsNullOrEmpty(max) && !string.IsNullOrEmpty(inputRoomName.text); // 최대 인원수와 방 이름이 있을 때만 생성 버튼 활성화
    }




    // 방 생성 성공 시 호출되는 콜백 메소드
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print("OnCreatedRoom");
    }

    // 방 생성 실패 시 호출되는 콜백 메소드
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        print("OnCreateRoomFailed, " + returnCode + " , " + message);
        // 필요 시 실패 처리 (예: 사용자에게 오류 메시지 표시)
    }

    // 방 참가 성공 시 호출되는 콜백 메소드
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print("OnJoinedRoom");
        PhotonNetwork.LoadLevel("MainGameScene"); // 방에 참가하면 "MainGameScene" 로드

    }

    // 방 참가 실패 시 호출되는 콜백 메소드
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        print("OnJoinRoomFailed, " + returnCode + ", " + message);
        // 필요 시 실패 처리 (예: 사용자에게 오류 메시지 표시)
    }



    public void OnClickBackToTitleButton()
    {
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby(); // 로비에서 나가는 작업 시작
        }
        else
        {
            PhotonNetwork.Disconnect(); // 바로 서버에서 나가기
        }
    }

    // 로비를 떠난 후 호출되는 콜백
    public override void OnLeftLobby()
    {
        base.OnLeftLobby();
        print("OnLeftLobby");
        PhotonNetwork.Disconnect(); // 로비를 떠난 후 서버 연결 끊기
    }

    // 서버 연결이 끊긴 후 호출되는 콜백
    public override void OnDisconnected(Photon.Realtime.DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        print("OnDisconnected");
        SceneManager.LoadScene("ConnectionScene"); // 서버 연결이 끊긴 후 씬 전환
    }

}