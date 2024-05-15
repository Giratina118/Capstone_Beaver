using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{

    void Start()
    {
        Screen.SetResolution(1920, 1080, false);
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 5 }, null, null);
    }

    public override void OnJoinedRoom()
    {
        GameObject CreatedBeaver = PhotonNetwork.Instantiate("PlayerBeaver", Vector3.zero, Quaternion.identity);
    }
    

}
