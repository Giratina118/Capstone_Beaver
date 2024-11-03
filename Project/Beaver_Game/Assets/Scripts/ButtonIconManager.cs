using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonIconManager : MonoBehaviour
{
    public Sprite[] citizenButtonSprites;
    public Sprite[] spyButtonSprites;
    public Sprite[] useButtonSprites;
    // 버튼 설명 0: 기본 액션, 1: 채집, 2: 제작, 3: 창고, 4: 로프, 5: 열쇠, 6: 댐 건설, 7: 댐 건설 가속/감속, 8: 댐 완공, 9:천파탑 철거, 10: 전파탑 건설(스파이만), 11: 전파탑 통신(스파이만)
    
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
