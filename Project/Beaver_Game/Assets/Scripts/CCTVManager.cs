using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable] //반드시 필요
public class CCTVCameraNum //행에 해당되는 이름
{
    public Transform[] cameraTransforms;
}

public class CCTVManager : MonoBehaviour
{
    public Camera cctvCamera;
    public Image cctvImageBackground;
    //public RawImage cctvRawImage;
    //public Texture cameraRendererTexture;

    public CCTVCameraNum[] cctvTransform;
    private int nowCameraNum = 0;
    private int nowCameraPosNum = 0;

    public void ShowCCTV(int cctvNum)
    {
        nowCameraNum = cctvNum;
        nowCameraPosNum = 0;
        cctvCamera.transform.position = cctvTransform[cctvNum].cameraTransforms[0].position;
        //cctvRawImage.texture = cameraRendererTexture;
        cctvImageBackground.gameObject.SetActive(true);
    }

    public void OnClickSwitchCameraButton(bool right)
    {
        if (right)
        {
            nowCameraPosNum++;
            if (nowCameraPosNum >= cctvTransform[nowCameraNum].cameraTransforms.Length)
                nowCameraPosNum = 0;

            cctvCamera.transform.position = cctvTransform[nowCameraNum].cameraTransforms[nowCameraPosNum].position;
        }
        else
        {
            nowCameraPosNum--;
            if (nowCameraPosNum < 0)
                nowCameraPosNum = cctvTransform[nowCameraNum].cameraTransforms.Length - 1;

            cctvCamera.transform.position = cctvTransform[nowCameraNum].cameraTransforms[nowCameraPosNum].position;
        }
    }

    public void CloseCCTV()
    {
        cctvImageBackground.gameObject.SetActive(false);
    }


    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
