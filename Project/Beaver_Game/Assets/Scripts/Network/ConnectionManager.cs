using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConnectionManager : MonoBehaviourPunCallbacks
{
    // Start는 처음 프레임 업데이트 전에 호출됩니다.
    void Start()
    {
    }

    // 서버에 접속하는 버튼을 클릭했을 때 호출되는 메소드
    public void OnClickConnect()
    {
        // 서버에 접속
        // Appid, 지역, 서버에 요청을 함
        PhotonNetwork.ConnectUsingSettings();
    }

    // 서버에 성공적으로 연결되었을 때 호출되는 콜백 메소드
    public override void OnConnected()
    {
        base.OnConnected();
        // 로그 확인용
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);
    }

    // 마스터 서버에 성공적으로 연결되었을 때 호출되는 콜백 메소드
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);

        // 로비에 참가 요청
        PhotonNetwork.JoinLobby();
    }
    
    // 로비에 성공적으로 참가했을 때 호출되는 콜백 메소드
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        // 로그 확인용
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);

        // 로비 씬 로드
        PhotonNetwork.LoadLevel("LobbyScene");
    }
}
