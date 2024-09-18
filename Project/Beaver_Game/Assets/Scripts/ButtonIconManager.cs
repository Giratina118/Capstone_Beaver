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
    

    public void SetButtonIcons(bool isSpy)
    {
        
        if (isSpy)
        {
            actionButton.gameObject.GetComponent<Image>().sprite = spyButtonSprites[0];
            demolishTowerButton.gameObject.GetComponent<Image>().sprite = spyButtonSprites[9];
            buildTowerComunicationButton.gameObject.GetComponent<Image>().sprite = spyButtonSprites[10];
            throwRopeButton.gameObject.GetComponent<Image>().sprite = spyButtonSprites[4];
            escapePrisonButton.gameObject.GetComponent<Image>().sprite = spyButtonSprites[5];

            useButtonSprites = spyButtonSprites;
        }
        else
        {
            actionButton.gameObject.GetComponent<Image>().sprite = citizenButtonSprites[0];
            demolishTowerButton.gameObject.GetComponent<Image>().sprite = citizenButtonSprites[9];
            throwRopeButton.gameObject.GetComponent<Image>().sprite = citizenButtonSprites[4];
            escapePrisonButton.gameObject.GetComponent<Image>().sprite = citizenButtonSprites[5];

            useButtonSprites = citizenButtonSprites;
        }
        
    }

    public void ChangeActionButtonIcon(int btnNum)
    {
        actionButton.gameObject.GetComponent<Image>().sprite = useButtonSprites[btnNum];
    }

    public void ChangeBuildTowerComunicationButton(bool doComunication)
    {
        if (doComunication)
        {
            buildTowerComunicationButton.gameObject.GetComponent<Image>().sprite = useButtonSprites[11];
        }
        else
        {
            buildTowerComunicationButton.gameObject.GetComponent<Image>().sprite = useButtonSprites[10];
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
