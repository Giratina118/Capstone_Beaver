using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IDropHandler
{

    //public GraphicRaycaster graphicRaycaster;
    //private List<RaycastResult> raycastResult = new List<RaycastResult>();

    

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("44444" + this.gameObject.name);
        Debug.Log(eventData.pointerDrag);


        if (this.transform.childCount > 0)
        {
            this.transform.GetChild(0).GetComponent<ItemDrag>().ItemChange(eventData.pointerDrag.GetComponent<ItemDrag>().normalPos, eventData.pointerDrag.GetComponent<ItemDrag>().normalParent);
        }

        eventData.pointerDrag.GetComponent<ItemDrag>().ItemDrop(this.transform.position, this.transform);
    }


    void Start()
    {
        
        
    }

    void Update()
    {
        
    }
}
