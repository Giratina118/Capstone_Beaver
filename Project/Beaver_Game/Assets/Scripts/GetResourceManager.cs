using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetResourceManager : MonoBehaviour
{
    public ItemIndex itemIndex;
    private int getResourceNum = 0;
    private Transform resourceItemPos;
    public Sprite[] getResourceSprite;
    public Animator beaverWorkAnimator;


    public void OnClickCancleButton()
    {
        this.gameObject.transform.localPosition = new Vector3(-2000, 0, 0);
        beaverWorkAnimator.SetBool("Work", false);
    }

    public void GetResourceActive(int resourceNum, Transform resourceDropPos)
    {
        this.transform.GetChild(0).GetComponent<Image>().sprite = getResourceSprite[resourceNum];
        beaverWorkAnimator.SetBool("Work", true);
        getResourceNum = resourceNum;
        resourceItemPos = resourceDropPos;
    }

    public void OnClickButtonInGetResource()
    {
        GameObject newResource = Instantiate(itemIndex.items[getResourceNum].gameObject);
        newResource.transform.position = resourceItemPos.position;

        this.gameObject.transform.localPosition = new Vector3(-2000, 0, 0);
        beaverWorkAnimator.SetBool("Work", false);
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
