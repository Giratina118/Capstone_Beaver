using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpyBoolManager : MonoBehaviour
{
    [SerializeField]
    private bool is_Spy = false;

    public Button buildTowerButton;
    SpyBeaverAction spyAction;

    public bool isSpy()
    {
        return is_Spy;
    }

    public void OnClickSpyChangeButton()    // test
    {
        is_Spy = !is_Spy;
        SpyManager(is_Spy);
    }

    public void SpyManager(bool isSpy)
    {
        if (isSpy)
        {
            spyAction.enabled = true;
            buildTowerButton.gameObject.SetActive(true);
        }
        else
        {
            spyAction.enabled = false;
            buildTowerButton.gameObject.SetActive(false);;
        }
    }

    void Start()
    {
        spyAction = GetComponent<SpyBeaverAction>();
    }

    void Update()
    {
        
    }
}
