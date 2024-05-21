using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EscapeMapClick : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public GameObject player;   // 플레이어
    public Transform escapeTransform;   // 해당 영역을 클릭할 시 워프할 탈출 위치


    public void OnPointerClick(PointerEventData eventData)  // 플레이어를 탈출 위치로 워프
    {
        this.gameObject.transform.GetComponent<MapImages>().player.transform.position = escapeTransform.position;
        this.gameObject.GetComponent<Image>().color = Color.white;
        this.transform.parent.position = new Vector3(0.0f, -1200.0f, 0.0f); // 지도가 안 보이도록 화면 밖으로 이동
        //this.transform.parent.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)  // 마우스가 이 영역 위에 있을때 다른 색으로 강조 표시
    {
        Color color = new Color(1.0f, 0.8f, 0.8f);
        this.gameObject.GetComponent<Image>().color = color;
    }

    public void OnPointerExit(PointerEventData eventData)   // 마우스가 위치해 있다가 떨어졌을때 원래 색으로 되돌림
    {
        this.gameObject.GetComponent<Image>().color = Color.white;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
