using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EscapeMapClick : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Transform escapeTransform;   // �ش� ������ Ŭ���� �� ������ Ż�� ��ġ
    Image mapPartImage;


    public void OnPointerClick(PointerEventData eventData)  // �÷��̾ Ż�� ��ġ�� ����
    {
        this.gameObject.transform.parent.gameObject.GetComponent<MapImages>().player.transform.position = escapeTransform.position;
        mapPartImage.color = Color.white;
        this.transform.parent.position = new Vector3(0.0f, -1200.0f, 0.0f); // ������ �� ���̵��� ȭ�� ������ �̵�
    }

    public void OnPointerEnter(PointerEventData eventData)  // ���콺�� �� ���� ���� ������ �ٸ� ������ ���� ǥ��
    {
        Color color = new Color(1.0f, 0.8f, 0.8f);
        mapPartImage.color = color;
    }

    public void OnPointerExit(PointerEventData eventData)   // ���콺�� ��ġ�� �ִٰ� ���������� ���� ������ �ǵ���
    {
        mapPartImage.color = Color.white;
    }

    void Start()
    {
        mapPartImage = this.gameObject.GetComponent<Image>();
    }
}
