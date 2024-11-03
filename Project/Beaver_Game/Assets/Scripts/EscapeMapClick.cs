using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EscapeMapClick : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Transform escapeTransform;   // 해당 영역을 클릭할 시 워프할 탈출 위치
    Image mapPartImage;


    public void OnPointerClick(PointerEventData eventData)  // 플레이어를 탈출 위치로 워프
    {
        this.gameObject.transform.parent.gameObject.GetComponent<MapImages>().player.transform.position = escapeTransform.position;
        mapPartImage.color = Color.white;
        this.transform.parent.position = new Vector3(0.0f, -1200.0f, 0.0f); // 지도가 안 보이도록 화면 밖으로 이동
    }

    public void OnPointerEnter(PointerEventData eventData)  // 마우스가 이 영역 위에 있을때 다른 색으로 강조 표시
    {
        Color color = new Color(1.0f, 0.8f, 0.8f);
        mapPartImage.color = color;
    }

    public void OnPointerExit(PointerEventData eventData)   // 마우스가 위치해 있다가 떨어졌을때 원래 색으로 되돌림
    {
        mapPartImage.color = Color.white;
    }

    void Start()
    {
        mapPartImage = this.gameObject.GetComponent<Image>();
    }
}
