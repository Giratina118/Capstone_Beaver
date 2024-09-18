using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GetResourceManager : MonoBehaviourPunCallbacks
{
    public ItemIndex itemIndex; // 아이템 목록
    private int getResourceNum = 0; // 아이템 번호
    private Transform resourceItemPos;  // 아이템 위치
    public Sprite[] getResourceSprite;  // 자원 채취 화면 배경 이미지들
    public Animator beaverWorkAnimator; // 자원 채취 비버 애니메이션

    public GetResourceScrollbar resourceScrollbar;
    public SoundEffectManager soundEffectManager;

    //public NetworkManager networkManager;


    public void CloseGetResourceScreen()   // 자원 채취 화면 우상단의 X 버튼 클릭, 자원 채취 화면 안 보이도록 하기
    {
        this.gameObject.transform.localPosition = new Vector3(-2000, 0, 0);
        beaverWorkAnimator.SetBool("Work", false);  // 자원 채취 애니메이션 멈춤 -> idle 상태로

        soundEffectManager.StopGetResourceSound();
    }

    public void GetResourceActive(int resourceNum, Transform resourceDropPos)   // 자원 채취 화면 설정
    {
        this.transform.GetChild(0).GetComponent<Image>().sprite = getResourceSprite[resourceNum];   // 배경 이미지 설정
        beaverWorkAnimator.SetBool("Work", true);   // 애니메이션 켜기
        getResourceNum = resourceNum;       // 해당 자원의 번호
        resourceItemPos = resourceDropPos;  // 해당 자원 채집하는 곳의 위치

        resourceScrollbar.SetScrollbar(getResourceNum);
        soundEffectManager.PlayGetResourceSound(getResourceNum);
    }

    public void OnClickButtonInGetResource()    // 자원 채취 버튼 클릭
    {
        int resourceResult = resourceScrollbar.StopScrolling();
        //networkManager.CreateItem(itemIndex.items[getResourceNum].gameObject.name, resourceItemPos.position);   // 자원 생성

        for (int i = 0; i < resourceResult; i++)
        {
            //Vector3 pos = this.transform.position;
            //float range = 1.5f;

            Vector3 rand = Random.insideUnitCircle * 1.5f;

            PhotonNetwork.Instantiate(itemIndex.items[getResourceNum].gameObject.name, resourceItemPos.position + rand, Quaternion.identity);

            
        }

        /*
        GameObject cloneobj = GameObject.Instantiate(obj);

        Vector3 pos = this.transform.position;
        float range = 1.5f;

        Vector3 rand = Random.insideUnitCircle * range;
        pos = pos + rand;


        cloneobj.transform.position = pos;
        */

        /*
        GameObject newResource = PhotonNetwork.Instantiate(itemIndex.items[getResourceNum].gameObject.name, Vector3.zero, Quaternion.identity);
        newResource.transform.position = resourceItemPos.position;
        */

        CloseGetResourceScreen();   // 자원 채취 화면 닫기
    }

    void Start()
    {

    }

    void Update()
    {
        
    }
}
