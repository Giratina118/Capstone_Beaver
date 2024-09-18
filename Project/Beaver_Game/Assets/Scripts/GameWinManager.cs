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
    public Sprite[] gameEndingSprites; 
    public TMP_Text gameEndingText;
    public bool doingGame = true;

    public void DamCountCheck() // 댐 수 체크
    {
        if (++damCount >= 2)    // 댐이 5개 지어지면 끝
        {
            Debug.Log("1111");
            GameEnding(false);
        }
    }

    [PunRPC]
    public void TowerCountCheck()   // 타워 수 체크
    {
        if (towers.transform.childCount >= 2)  // 타워가 맵에 10개 이상 동시에 존재하면 끝
        {
            Debug.Log("2222");
            GameEnding(true);
        }
    }

    [PunRPC]
    public void TimeCheck() // 시간 체크, 시간이 0이 되면 이 함수로 들어옴
    {
        Debug.Log("3333");
        GameEnding(true);
    }

    public void GameEnding(bool spyWin) // 게임 결과
    {
        Debug.Log("4444");
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
                gameEndingText.text = "You Win";
            }
            else
            {
                Debug.Log("시민 비버 win");
                Debug.Log("당신의 패배");

                gameEndingImage.sprite = gameEndingSprites[0];
                gameEndingText.text = "You Lose";
            }
        }
        else
        {
            if (spyWin)
            {
                Debug.Log("스파이 비버 win");
                Debug.Log("당신의 패배");

                gameEndingImage.sprite = gameEndingSprites[1];
                gameEndingText.text = "You Lose";
            }
            else
            {
                Debug.Log("시민 비버 win");
                Debug.Log("당신의 승리");

                gameEndingImage.sprite = gameEndingSprites[0];
                gameEndingText.text = "You Win";
            }
        }


        gameEndingImage.gameObject.SetActive(true);

        //GoWaitingRoom();
    }

    /*  미완, 게임 종료 후 로비로 나가지 않고 대기방으로 돌아오도록
    public void GoWaitingRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = true;
            PhotonNetwork.CurrentRoom.IsVisible = true;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
    */

    void Start()
    {

    }

    void Update()
    {
        
    }
}
