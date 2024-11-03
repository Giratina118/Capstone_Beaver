using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomListItem : MonoBehaviour
{
    public TMP_Text roomInfo;
    public Action<string> onDelegate;   //클릭되었을때 호출되는 함수


    public void SetInfo(string roomName, int currPlayer, int maxPlayer)
    {
        name = roomName;
        roomInfo.text = roomName + '(' + currPlayer + '/' + maxPlayer + ')';

        if (maxPlayer == 0)
            Destroy(this.gameObject);
    }

    public void OnClick()
    {
        if (onDelegate != null) //만약 onDelegate 에 무언가 들어있다면 실행
        {
            onDelegate(name);
        }
        GameObject go = GameObject.Find("InputRoomName");   //InputRoomName 찾아오기
        InputField inputField = go.GetComponent<InputField>();  //찾아온 게임오브젝트에서 InputField 컴포넌트 가져오기
        inputField.text = name; //가져온 컴포넌트에서 text 값을 나의 이름으로 셋팅하기
    }
}
