using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetResourceManager : MonoBehaviour
{
    public ItemIndex itemIndex;

    private int getResourceNum = 0;
    private Transform resourceItemPos;
    private bool getRecourceActiveOn = false;

    public void GetResourceActive(int resourceNum, Transform resourceDropPos)
    {
        getResourceNum = resourceNum;
        resourceItemPos = resourceDropPos;
        switch (resourceNum)
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            default:
                break;
        }
        getRecourceActiveOn = true;

    }

    void Start()
    {
        
    }

    void Update()
    {
        if (getRecourceActiveOn && Input.GetKeyDown(KeyCode.Space))
        {
            getRecourceActiveOn = false;

            GameObject newResource = Instantiate(itemIndex.items[getResourceNum].gameObject);
            newResource.transform.position = resourceItemPos.position;



            this.gameObject.SetActive(false);
        }
    }
}
