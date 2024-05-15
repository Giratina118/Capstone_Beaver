using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductionCloseButton : MonoBehaviour
{

    public void OnClickCloseButton()
    {
        this.transform.localPosition = new Vector3(0.0f, 1000.0f, 0.0f);
        this.transform.GetChild(2).localPosition = new Vector3(300.0f, 1000.0f, 0.0f);
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
