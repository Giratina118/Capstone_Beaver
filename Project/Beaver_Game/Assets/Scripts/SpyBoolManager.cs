using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpyBoolManager : MonoBehaviour
{
    [SerializeField]
    private bool is_Spy = false;    // ������ ����

    public SpyBeaverAction spyAction;
    private ShowRole showRole;
    public ButtonIconManager btnManager;
    public GameObject towerPriceObject;


    public bool isSpy() // ������ ���� Ȯ��
    {
        return is_Spy;
    }

    public void SetSpyBool(bool isSpy)
    {
        Debug.Log("������ ����: " + isSpy);
        is_Spy = isSpy;
        SpyManager();
    }


    public void SpyManager()
    {
        Debug.Log("SpyManager called. is_Spy: " + is_Spy);

        spyAction.enabled = is_Spy;
        btnManager.buildTowerComunicationButton.gameObject.SetActive(is_Spy);
        showRole.SetShowRuleImage(is_Spy);
        btnManager.SetButtonIcons(is_Spy);

        towerPriceObject.SetActive(is_Spy);
    }

    void Start()
    {
        if (!this.gameObject.GetPhotonView().IsMine)
            return;

        btnManager = GameObject.Find("Buttons").GetComponent<ButtonIconManager>();
        showRole = GameObject.Find("ShowRoleImage").GetComponent<ShowRole>();
        showRole.gameObject.SetActive(false);
        towerPriceObject = GameObject.Find("TowerPrice");
        towerPriceObject.SetActive(false);
    }
}
