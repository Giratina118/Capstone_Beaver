using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomListItem : MonoBehaviour
{
    public TMP_Text roomInfo;
    public Action<string> onDelegate;   //Ŭ���Ǿ����� ȣ��Ǵ� �Լ�


    public void SetInfo(string roomName, int currPlayer, int maxPlayer)
    {
        name = roomName;
        roomInfo.text = roomName + '(' + currPlayer + '/' + maxPlayer + ')';

        if (maxPlayer == 0)
            Destroy(this.gameObject);
    }

    public void OnClick()
    {
        if (onDelegate != null) //���� onDelegate �� ���� ����ִٸ� ����
        {
            onDelegate(name);
        }
        GameObject go = GameObject.Find("InputRoomName");   //InputRoomName ã�ƿ���
        InputField inputField = go.GetComponent<InputField>();  //ã�ƿ� ���ӿ�����Ʈ���� InputField ������Ʈ ��������
        inputField.text = name; //������ ������Ʈ���� text ���� ���� �̸����� �����ϱ�
    }
}
