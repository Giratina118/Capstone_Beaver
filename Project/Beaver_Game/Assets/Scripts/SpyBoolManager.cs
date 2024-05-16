using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpyBoolManager : MonoBehaviour
{
    [SerializeField]
    private bool is_Spy = false;    // 스파이 여부

    public Button buildTowerButton;
    SpyBeaverAction spyAction;

    public bool isSpy() // 스파이 여부 확인
    {
        return is_Spy;
    }

    public void OnClickSpyChangeButton()    // 테스트용, 스파이인지 시민인지 현재 상태를 바꿈
    {
        is_Spy = !is_Spy;
        SpyManager(is_Spy);
    }

    public void SpyManager(bool isSpy)
    {
        if (isSpy)  // 스파이라면 스파이만 할 수 있는 행동 켜기
        {
            spyAction.enabled = true;
            buildTowerButton.gameObject.SetActive(true);
        }
        else    // 스파이가 아니라면 스파이만 할 수 있는 행동 끄기
        {
            spyAction.enabled = false;
            buildTowerButton.gameObject.SetActive(false);;
        }
    }

    void Start()
    {
        spyAction = GetComponent<SpyBeaverAction>();
        buildTowerButton = GameObject.Find("SpyChangeButton").GetComponent<Button>();
    }

    void Update()
    {
        
    }
}
