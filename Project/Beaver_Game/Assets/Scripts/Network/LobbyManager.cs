using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public InputField inputRoomName; // �� �̸��� �Է¹޴� InputField
    public Button btnJoin;           // �� ���� ��ư
    public Button btnCreate;         // �� ���� ��ư
    public InputField inputMaxPlayers; // �ִ� �ο����� �Է¹޴� InputField

    void Start()
    {
        // �ʱ⿡�� ��ư���� ��Ȱ��ȭ
        btnJoin.interactable = false;
        btnCreate.interactable = false;

        // InputField�� ���� ����� �� ȣ��Ǵ� ������ �߰�
        inputRoomName.onValueChanged.AddListener(OnRoomNameValueChanged);
        inputMaxPlayers.onValueChanged.AddListener(OnMaxPlayerValueChanged);
    }

    // ���� �����ϴ� �޼ҵ�
    public void CreateRoom()
    {
        string roomName = inputRoomName.text; // �Է¹��� �� �̸�
        byte maxPlayers;
        
        // �Է¹��� �ִ� �ο����� byte Ÿ������ ��ȯ, ���� �� �⺻�� 5 ����
        if (!byte.TryParse(inputMaxPlayers.text, out maxPlayers))
        {
            maxPlayers = 5;
        }

        // �� �ɼ� ����
        RoomOptions roomOptions = new RoomOptions
        {
            MaxPlayers = maxPlayers, // �ִ� �ο��� ����
            IsVisible = true         // ���� �����ǵ��� ����
        };

        // �� ���� ��û
        PhotonNetwork.CreateRoom(roomName, roomOptions);
    }

    // �� ���� �޼ҵ�
    public void JoinRoom()
    {
        string roomName = inputRoomName.text; // �Է¹��� �� �̸�
        PhotonNetwork.JoinRoom(roomName);     // �� ���� ��û
    }

    // �� �̸��� ����� �� ȣ��Ǵ� �޼ҵ�
    private void OnRoomNameValueChanged(string room)
    {
        bool isInteractable = !string.IsNullOrEmpty(room); // �� �̸��� ������� ������ Ȯ��
        btnJoin.interactable = isInteractable;             // �� �̸��� ���� ���� ���� ��ư Ȱ��ȭ
        btnCreate.interactable = isInteractable && !string.IsNullOrEmpty(inputMaxPlayers.text); // �� �̸��� �ִ� �ο����� ���� ���� ���� ��ư Ȱ��ȭ
    }

    // �ִ� �ο����� ����� �� ȣ��Ǵ� �޼ҵ�
    private void OnMaxPlayerValueChanged(string max)
    {
        btnCreate.interactable = !string.IsNullOrEmpty(max) && !string.IsNullOrEmpty(inputRoomName.text); // �ִ� �ο����� �� �̸��� ���� ���� ���� ��ư Ȱ��ȭ
    }









    // �� ���� ���� �� ȣ��Ǵ� �ݹ� �޼ҵ�
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print("OnCreatedRoom");
    }

    // �� ���� ���� �� ȣ��Ǵ� �ݹ� �޼ҵ�
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        print("OnCreateRoomFailed, " + returnCode + " , " + message);
        // �ʿ� �� ���� ó�� (��: ����ڿ��� ���� �޽��� ǥ��)
    }

    // �� ���� ���� �� ȣ��Ǵ� �ݹ� �޼ҵ�
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print("OnJoinedRoom");
        PhotonNetwork.LoadLevel("SampleScene"); // �濡 �����ϸ� "SampleScene" �ε�
    }

    // �� ���� ���� �� ȣ��Ǵ� �ݹ� �޼ҵ�
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        print("OnJoinRoomFailed, " + returnCode + ", " + message);
        // �ʿ� �� ���� ó�� (��: ����ڿ��� ���� �޽��� ǥ��)
    }
}