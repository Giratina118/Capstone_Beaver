using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonIconManager : MonoBehaviour
{
    public Sprite[] citizenButtonSprites;
    public Sprite[] spyButtonSprites;
    public Sprite[] useButtonSprites;
    // ��ư ���� 0: �⺻ �׼�, 1: ä��, 2: ����, 3: â��, 4: ����, 5: ����, 6: �� �Ǽ�, 7: �� �Ǽ� ����/����, 8: �� �ϰ�, 9:õ��ž ö��, 10: ����ž �Ǽ�(�����̸�), 11: ����ž ���(�����̸�)
    
    public Button actionButton;
    public Button demolishTowerButton;
    public Button buildTowerComunicationButton;
    public Button throwRopeButton;
    public Button escapePrisonButton;

    public Image actionButtonImage;
    public Image buildTowerComunicationButtonImage;

    public void SetButtonIcons(bool isSpy)
    {
        actionButtonImage = actionButton.gameObject.GetComponent<Image>();
        if (isSpy)
        {
            actionButtonImage.sprite = spyButtonSprites[0];
            buildTowerComunicationButtonImage = buildTowerComunicationButton.gameObject.GetComponent<Image>();
            buildTowerComunicationButtonImage.sprite = spyButtonSprites[10];
            demolishTowerButton.gameObject.GetComponent<Image>().sprite = spyButtonSprites[9];
            throwRopeButton.gameObject.GetComponent<Image>().sprite = spyButtonSprites[4];
            escapePrisonButton.gameObject.GetComponent<Image>().sprite = spyButtonSprites[5];

            useButtonSprites = spyButtonSprites;
        }
        else
        {
            actionButtonImage.sprite = citizenButtonSprites[0];
            demolishTowerButton.gameObject.GetComponent<Image>().sprite = citizenButtonSprites[9];
            throwRopeButton.gameObject.GetComponent<Image>().sprite = citizenButtonSprites[4];
            escapePrisonButton.gameObject.GetComponent<Image>().sprite = citizenButtonSprites[5];

            useButtonSprites = citizenButtonSprites;
        }
        
    }

    public void ChangeActionButtonIcon(int btnNum)
    {
        actionButtonImage.sprite = useButtonSprites[btnNum];
    }

    public void ChangeBuildTowerComunicationButton(bool doComunication)
    {
        if (doComunication)
        {
            buildTowerComunicationButtonImage.sprite = useButtonSprites[11];
        }
        else
        {
            buildTowerComunicationButtonImage.sprite = useButtonSprites[10];
        }
    }
}
