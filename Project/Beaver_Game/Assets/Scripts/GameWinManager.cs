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
    private int damCount = 0;   // ������ �� ��
    public GameObject towers;   // ������ Ÿ���� ��Ƶ� ������Ʈ(�ڽ� ���� ������ Ÿ�� ��)
    public SpyBoolManager spyBoolManager;   // ���������� ����
    public Image gameEndingImage;   // ���� ���� ȭ��
    public Image gameEndingTextImage;   // ���� ��� �ؽ�Ʈ �̹���
    public Sprite[] gameEndingSprites; 
    public Sprite[] gameEndingTextImages;   // 0: �¸�, 1: �й�
    public TMP_Text gameEndingText;
    public bool doingGame = true;

    public Image gameEndingTextBG;
    public Color[] resultColors; // 0: �¸�, 1: �й�

    public void DamCountCheck() // �� �� üũ
    {
        if (++damCount >= 5)    // ���� 5�� �������� ��
        {
            GameEnding(false);
        }
    }

    [PunRPC]
    public void TowerCountCheck()   // Ÿ�� �� üũ
    {
        if (towers.transform.childCount >= 10)  // Ÿ���� �ʿ� 10�� �̻� ���ÿ� �����ϸ� ��
        {
            GameEnding(true);
        }
    }

    [PunRPC]
    public void TimeCheck() // �ð� üũ, �ð��� 0�� �Ǹ� �� �Լ��� ����
    {
        GameEnding(true);
    }

    public void GameEnding(bool spyWin) // ���� ���
    {
        if (!doingGame)
            return;

        doingGame = false;

        if (spyBoolManager.isSpy())
        {
            if (spyWin)
            {
                Debug.Log("������ ��� win");
                Debug.Log("����� �¸�");

                gameEndingImage.sprite = gameEndingSprites[1];
                gameEndingTextImage.sprite = gameEndingTextImages[0];
                gameEndingTextBG.color = resultColors[0];
            }
            else
            {
                Debug.Log("�ù� ��� win");
                Debug.Log("����� �й�");

                gameEndingImage.sprite = gameEndingSprites[0];
                gameEndingTextImage.sprite = gameEndingTextImages[1];
                gameEndingTextBG.color = resultColors[1];
            }
        }
        else
        {
            if (spyWin)
            {
                Debug.Log("������ ��� win");
                Debug.Log("����� �й�");

                gameEndingImage.sprite = gameEndingSprites[1];
                gameEndingTextImage.sprite = gameEndingTextImages[1];
                gameEndingTextBG.color = resultColors[1];
            }
            else
            {
                Debug.Log("�ù� ��� win");
                Debug.Log("����� �¸�");

                gameEndingImage.sprite = gameEndingSprites[0];
                gameEndingTextImage.sprite = gameEndingTextImages[0];
                gameEndingTextBG.color = resultColors[0];
            }
        }
        gameEndingImage.gameObject.SetActive(true);
    }
}
