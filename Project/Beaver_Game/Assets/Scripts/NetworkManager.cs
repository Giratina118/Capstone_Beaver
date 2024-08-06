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


        
        Vector3 startPos = new Vector3(25.0f, 5.0f, 0.0f);
        GameObject createdBeaver = PhotonNetwork.Instantiate("PlayerBeaver", startPos, Quaternion.identity);    // 플레이어 비버 생성
        if (createdBeaver.GetPhotonView().IsMine)
        {
            cinemachineVirtualCamera.Follow = createdBeaver.transform;  // 플레이어와 시네머신 카메라 연결
            cinemachineVirtualCamera.LookAt = createdBeaver.transform;
        }
        
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 5 }, null, null);
    }

    public override void OnJoinedRoom()
    {
        Vector3 startPos = new Vector3(25.0f, 5.0f, 0.0f);
        GameObject createdBeaver = PhotonNetwork.Instantiate("PlayerBeaver", startPos, Quaternion.identity);    // 플레이어 비버 생성
        if (createdBeaver.GetPhotonView().IsMine)
        {
            cinemachineVirtualCamera.Follow = createdBeaver.transform;  // 플레이어와 시네머신 카메라 연결
            cinemachineVirtualCamera.LookAt = createdBeaver.transform;
        }
        
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
