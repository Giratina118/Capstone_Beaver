using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipCharacterCamera : MonoBehaviour
{
    // 아이템 장작화면에 모습을 보여주기 위한 카메라, 장착 가능한 도구을 획득한 후 Layer를 EquipItem으로 해두기, Player와 EquipItem Layer만을 보여주는 카메라

    [SerializeField]
    private Transform playerTransform;

    void Start()
    {
        
    }

    void Update()
    {
        transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, this.transform.position.z);    // 카메라 위치 조정
    }
}
