using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameWinManager : MonoBehaviour
{
    [SerializeField]
    private int damCount = 0;   // 지어진 댐 수
    public GameObject towers;   // 지어진 타워를 모아둔 오브젝트(자식 수가 지어진 타워 수)
    public SpyBoolManager spyBoolManager;   // 스파이인지 여부
    public Image gameEndingImage;   // 게임 엔딩 화면
    public Image gameEndingTextImage;   // 게임 결과 텍스트 이미지
    public Sprite[] gameEndingSprites; 
    public Sprite[] gameEndingTextImages;   // 0: 승리, 1: 패배
    public TMP_Text gameEndingText;
    public bool doingGame = true;

    public Image gameEndingTextBG;
    public Color[] resultColors; // 0: 승리, 1: 패배

    public void DamCountCheck() // 댐 수 체크
    {
        if (++damCount >= 5)    // 댐이 5개 지어지면 끝
        {
            GameEnding(false);
        }
    }

    [PunRPC]
    public void TowerCountCheck()   // 타워 수 체크
    {
        if (towers.transform.childCount >= 10)  // 타워가 맵에 10개 이상 동시에 존재하면 끝
        {
            GameEnding(true);
        }
    }

    [PunRPC]
    public void TimeCheck() // 시간 체크, 시간이 0이 되면 이 함수로 들어옴
    {
        GameEnding(true);
    }

    public void GameEnding(bool spyWin) // 게임 결과
    {
        if (!doingGame)
            return;

        doingGame = false;

        if (spyBoolManager.isSpy())
        {
            if (spyWin)
            {
                Debug.Log("스파이 비버 win");
                Debug.Log("당신의 승리");

                gameEndingImage.sprite = gameEndingSprites[1];
                gameEndingTextImage.sprite = gameEndingTextImages[0];
                gameEndingTextBG.color = resultColors[0];
            }
            else
            {
                Debug.Log("시민 비버 win");
                Debug.Log("당신의 패배");

                gameEndingImage.sprite = gameEndingSprites[0];
                gameEndingTextImage.sprite = gameEndingTextImages[1];
                gameEndingTextBG.color = resultColors[1];
            }
        }
        else
        {
            if (spyWin)
            {
                Debug.Log("스파이 비버 win");
                Debug.Log("당신의 패배");

                gameEndingImage.sprite = gameEndingSprites[1];
                gameEndingTextImage.sprite = gameEndingTextImages[1];
                gameEndingTextBG.color = resultColors[1];
            }
            else
            {
                Debug.Log("시민 비버 win");
                Debug.Log("당신의 승리");

                gameEndingImage.sprite = gameEndingSprites[0];
                gameEndingTextImage.sprite = gameEndingTextImages[0];
                gameEndingTextBG.color = resultColors[0];
            }
        }
        gameEndingImage.gameObject.SetActive(true);
    }
}
