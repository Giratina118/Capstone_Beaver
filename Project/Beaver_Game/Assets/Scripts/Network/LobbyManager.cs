using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public InputField inputRoomName; // 방 이름을 입력받는 InputField
    public Button btnJoin;           // 방 참가 버튼
    public Button btnCreate;         // 방 생성 버튼
    public InputField inputMaxPlayers; // 최대 인원수를 입력받는 InputField

    void Start()
    {
        // 초기에는 버튼들을 비활성화
        btnJoin.interactable = false;
        btnCreate.interactable = false;

        // InputField의 값이 변경될 때 호출되는 리스너 추가
        inputRoomName.onValueChanged.AddListener(OnRoomNameValueChanged);
        inputMaxPlayers.onValueChanged.AddListener(OnMaxPlayerValueChanged);
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
        PhotonNetwork.LoadLevel("SampleScene"); // 방에 참가하면 "SampleScene" 로드
    }

    // 방 참가 실패 시 호출되는 콜백 메소드
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        print("OnJoinRoomFailed, " + returnCode + ", " + message);
        // 필요 시 실패 처리 (예: 사용자에게 오류 메시지 표시)
    }
}