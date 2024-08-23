using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetResourceScrollbar : MonoBehaviour
{
    public RectTransform ScrollbarRandRangeImage;   // 스크롤바에서 성공 범위를 측정해주는 노랑, 초록 사각형 이미지
    private float randRange;    // 성공 범위
    private Scrollbar scrollbar;    // 스크롤바
    private bool scrollingForward = true;   // 스크롤바 진행 방향(한 쪽에 닿으면 반대로 진행하도록)
    private float scrollSpeed = 0.5f;   // 스크롤바 스피드

    private bool scrolling = false; // 스크롤바가 움직이고 있는지

    public void SetScrollbar()  // 스크롤바의 초기값, 목표 랜덤값 설정
    {
        scrollbar.value = 0.0f; // 초기값
        randRange = Random.value;   // 목표 랜덤값 설정
        ScrollbarRandRangeImage.localPosition = new Vector3(randRange * 1000.0f, -50.0f, 0.0f); // 목표 영역 이미지 위치
        scrolling = true;

        StartCoroutine(AutoScrolling());
    }

    public int StopScrolling()
    {
        scrolling = false;  // 스크롤바 진행 정지
        StopCoroutine(AutoScrolling());

        // 어디서 멈췄는지에 따라 획득 자원 수 다르게
        if (scrollbar.value >= randRange - 0.05f && scrollbar.value <= randRange + 0.05f)
        {
            return 2;
        }
        else if (scrollbar.value >= randRange - 0.15f && scrollbar.value <= randRange + 0.15f)
        {
            return 1;
        }
        else
        {
            return 0;
        }
        
    }


    IEnumerator AutoScrolling() // 스크롤바 좌우로 계속 이동
    {
        while (scrolling)
        {
            if (scrollingForward)   // 한 쪽 끝에 닿으면 반대쪽으로 진행하도록
            {
                scrollbar.value += scrollSpeed * Time.deltaTime;
                if (scrollbar.value >= 1.0f)
                {
                    scrollbar.value = 1.0f;
                    scrollingForward = false;
                }
            }
            else
            {
                scrollbar.value -= scrollSpeed * Time.deltaTime;
                if (scrollbar.value <= 0.0f)
                {
                    scrollbar.value = 0.0f;
                    scrollingForward = true;
                }
            }
            yield return null;
        }
    }

    void Start()
    {
        scrollbar = this.GetComponent<Scrollbar>();

    }

    void Update()
    {

    }
}
