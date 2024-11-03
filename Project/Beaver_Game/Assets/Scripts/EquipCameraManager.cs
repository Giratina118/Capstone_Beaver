using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipCameraManager : MonoBehaviour
{
    public RenderTexture renderTexture;
    private RawImage equipCharacterImage;
    public GameObject nightLight;


    void Start()
    {
        if (!this.transform.parent.gameObject.GetPhotonView().IsMine)
            return;

        Player localPlayer = PhotonNetwork.LocalPlayer;

        // 현재 방의 모든 플레이어를 가져옵니다.
        Player[] players = PhotonNetwork.PlayerList;

        // 플레이어 배열에서 현재 플레이어의 인덱스를 찾습니다.
        int index = System.Array.IndexOf(players, localPlayer);

        this.gameObject.transform.parent.gameObject.layer = 6 + index;
        this.gameObject.GetComponent<Camera>().enabled = true;
        this.gameObject.GetComponent<Camera>().cullingMask = (1 << this.gameObject.transform.parent.gameObject.layer);
        this.gameObject.GetComponent<Camera>().targetTexture = renderTexture;
        equipCharacterImage = GameObject.Find("EquipCharacterImage").GetComponent<RawImage>();
        equipCharacterImage.texture = renderTexture;

        nightLight.layer = this.transform.parent.gameObject.layer;
        GameObject.Find("Timer").GetComponent<TimerManager>().nightLight = nightLight;
    }
}
