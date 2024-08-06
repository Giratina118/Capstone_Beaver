using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomListItem : MonoBehaviour
{
    public TMP_Text roomInfo;


    //클릭되었을때 호출되는 함수
    public Action<string> onDelegate;

    public void SetInfo(string roomName, int currPlayer, int maxPlayer)
    {
        name = roomName;
        roomInfo.text = roomName + '(' + currPlayer + '/' + maxPlayer + ')';
    }

    public void OnClick()
    {
        //만약 onDelegate 에 무언가 들어있다면 실행
        if (onDelegate != null)
        {
            onDelegate(name);
        }
        ////InputRoomName 찾아오기
        GameObject go = GameObject.Find("InputRoomName");
        ////찾아온 게임오브젝트에서 InputField 컴포넌트 가져오기
        InputField inputField = go.GetComponent<InputField>();
        ////가져온 컴포넌트에서 text 값을 나의 이름으로 셋팅하기
        inputField.text = name;
    }


    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
