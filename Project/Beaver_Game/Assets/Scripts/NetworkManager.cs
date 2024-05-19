using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Cinemachine;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public CinemachineVirtualCamera cinemachineVirtualCamera;

    void Start()
    {
        Screen.SetResolution(1920, 1080, false);    // ȭ�� ���μ��� ����
        PhotonNetwork.ConnectUsingSettings();
        
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 5 }, null, null);
    }

    public override void OnJoinedRoom()
    {
        GameObject CreatedBeaver = PhotonNetwork.Instantiate("PlayerBeaver", Vector3.zero, Quaternion.identity);    // �÷��̾� ��� ����
        cinemachineVirtualCamera.Follow = CreatedBeaver.transform;  // �÷��̾�� �ó׸ӽ� ī�޶� ����
        cinemachineVirtualCamera.LookAt = CreatedBeaver.transform;
    }
    

    public void CreateResource(string resourceName, Vector3 resourcePos)    // �ڿ� ä��ĭ���� ��ư ������ �ڿ� ������ �ʵ忡 ����
    {
        GameObject newResource = PhotonNetwork.Instantiate(resourceName, resourcePos, Quaternion.identity);
        //newResource.transform.position = resourcePos;
    }

}
