using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineManager : MonoBehaviour
{
    public PolygonCollider2D[] cameraRanges;


    public void SetCameraRange(int num)
    {
        this.GetComponent<CinemachineConfiner>().m_BoundingShape2D = cameraRanges[num];
    }
}
