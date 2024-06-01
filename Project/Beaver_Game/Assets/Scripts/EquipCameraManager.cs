using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipCameraManager : MonoBehaviour
{
    public RenderTexture renderTexture;
    private RawImage equipCharacterImage;

    void Start()
    {
        if (!this.transform.parent.gameObject.GetPhotonView().IsMine)
            return;
        this.gameObject.transform.parent.gameObject.layer = 5 + this.gameObject.transform.parent.gameObject.GetComponent<PhotonView>().OwnerActorNr;
        this.gameObject.GetComponent<Camera>().enabled = true;
        this.gameObject.GetComponent<Camera>().cullingMask = (1 << this.gameObject.transform.parent.gameObject.layer);
        this.gameObject.GetComponent<Camera>().targetTexture = renderTexture;
        equipCharacterImage = GameObject.Find("EquipCharacterImage").GetComponent<RawImage>();
        equipCharacterImage.texture = renderTexture;
    }

    void Update()
    {
        
    }
}
