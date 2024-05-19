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
        Screen.SetResolution(1920, 1080, false);    // 화면 가로세로 설정
        PhotonNetwork.ConnectUsingSettings();
        
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 5 }, null, null);
    }

    public override void OnJoinedRoom()
    {
        GameObject CreatedBeaver = PhotonNetwork.Instantiate("PlayerBeaver", Vector3.zero, Quaternion.identity);    // 플레이어 비버 생성
        cinemachineVirtualCamera.Follow = CreatedBeaver.transform;  // 플레이어와 시네머신 카메라 연결
        cinemachineVirtualCamera.LookAt = CreatedBeaver.transform;
    }
    

    public void CreateResource(string resourceName, Vector3 resourcePos)    // 자원 채취칸에서 버튼 누르면 자원 아이템 필드에 생성
    {
        GameObject newResource = PhotonNetwork.Instantiate(resourceName, resourcePos, Quaternion.identity);
        //newResource.transform.position = resourcePos;
    }

}
