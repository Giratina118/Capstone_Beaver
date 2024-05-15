using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWinManager : MonoBehaviour
{
    [SerializeField]
    private int damCount = 0;
    public GameObject towers;
    public SpyBoolManager spyBoolManager;

    public void DamCountCheck()
    {
        if (++damCount >= 5)
        {
            GameEnding(false);
        }
    }

    public void TowerCountCheck()
    {
        if (towers.transform.childCount >= 10)
        {
            GameEnding(true);
        }
    }

    public void TimeCheck()
    {
        GameEnding(true);
    }

    public void GameEnding(bool spyWin)
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
