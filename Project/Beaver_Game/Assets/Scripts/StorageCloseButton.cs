using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageCloseButton : MonoBehaviour
{
    public void OnClickCloseButton()    // Ã¢°í Ã¢ ´Ý±â
    {
        this.transform.localPosition = new Vector3(2000.0f, 0.0f, 0.0f);
        this.transform.parent.localPosition = new Vector3(2000.0f, 0.0f, 0.0f);
    }
}
