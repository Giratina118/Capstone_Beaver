using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProductionToggle : MonoBehaviour
{
    private ProductionManager productionManager = null;
    private ItemIndex itemIndex = null;
    public int itemNumber;
    private GameObject itemCreateObject;


    public void OnChangedToggle()   // 이 아이템이 선택되면 그 정보를 보냄(제작 정보에 띄우기 위해)
    {
        if (this.gameObject.GetComponent<Toggle>().isOn)
        {
            Debug.Log(this.gameObject + "  " + itemCreateObject + "  ");

            itemCreateObject.transform.localPosition = new Vector3(300.0f, 0.0f, 0.0f);
            productionManager.SetSelectedItemmInfo(itemNumber);
        }
    }

    void Start()
    {
        productionManager = GameObject.Find("SelectedItemBackground").GetComponent<ProductionManager>();
        itemCreateObject = GameObject.Find("SelectedItemBackground");
        itemIndex = GameObject.Find("ItemManager").GetComponent<ItemIndex>();
        this.transform.GetChild(1).GetComponent<Image>().sprite = itemIndex.items[itemNumber].GetComponent<SpriteRenderer>().sprite;
        this.transform.GetChild(1).GetComponent<Image>().preserveAspect = true;
    }
}
