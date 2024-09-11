using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWinManager : MonoBehaviour
{
    [SerializeField]
    private int damCount = 0;   // 지어진 댐 수
    public GameObject towers;   // 지어진 타워를 모아둔 오브젝트(자식 수가 지어진 타워 수)
    public SpyBoolManager spyBoolManager;   // 스파이인지 여부

    public void DamCountCheck() // 댐 수 체크
    {
        if (++damCount >= 5)    // 댐이 5개 지어지면 끝
        {
            GameEnding(false);
        }
    }

    public void TowerCountCheck()   // 타워 수 체크
    {
        if (towers.transform.childCount >= 10)  // 타워가 맵에 10개 이상 동시에 존재하면 끝
        {
            GameEnding(true);
        }
    }

    public void TimeCheck() // 시간 체크, 시간이 0이 되면 이 함수로 들어옴
    {
        GameEnding(true);
    }

    public void GameEnding(bool spyWin) // 게임 결과
    {
        if (spyBoolManager.isSpy())
        {
            if (spyWin)
            {
                Debug.Log("스파이 비버 win");
                Debug.Log("당신의 승리");
            }
            else
            {
                Debug.Log("시민 비버 win");
                Debug.Log("당신의 패배");
            }
        }
        else
        {
            if (spyWin)
            {
                Debug.Log("스파이 비버 win");
                Debug.Log("당신의 패배");
            }
            else
            {
                Debug.Log("시민 비버 win");
                Debug.Log("당신의 승리");
            }
        }
    }


    void Start()
    {

    }

    void Update()
    {
        
    }
}
