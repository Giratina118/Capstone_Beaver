using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class ItemCount : MonoBehaviourPunCallbacks
{
    // 인벤토리, 창고 등에서의 아이템 수를 관리
    public int count;   // 아이템 수
    private TMP_Text countText; // 아이템 수 출력하는 텍스트
    

    public int ItemCountHalf()  // 아이템 숫자 반으로 나누기, 마우스 오른쪽 버튼을 통해 인벤토리에서 들고있는 아이템 수를 반으로 할때 사용
    {
        if (count <= 1)
            return 0;
        int temp = count - count / 2;
        ShowItemCount(-temp);
        return temp;
    }

    [PunRPC]
    public void ShowItemCount(int addCount) // 해당 아이템의 수 출력
    {
        count += addCount;
        countText.text = count.ToString();
    }

    public void SetCountText()  // 아이템과 그 아이템의 수를 적을 TMP 연결
    {
        countText = this.transform.GetChild(0).GetComponent<TMP_Text>();
    }

    void Start()
    {
        SetCountText();
    }
}
