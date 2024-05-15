using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlayerMove : MonoBehaviour
{
    public float moveSpeed = 10.0f;

    void Start()
    {
        
    }

    void Update()
    {
        float yPlus = 0.0f;
        float yMinus = 0.0f;
        float xPlus = 0.0f;
        float xMinus = 0.0f;

        if (Input.GetKey(KeyCode.I))
            yPlus = 1.0f;
        if (Input.GetKey(KeyCode.K))
            yMinus = 1.0f;
        if (Input.GetKey(KeyCode.L))
            xPlus = 1.0f;
        if (Input.GetKey(KeyCode.J))
            xMinus = 1.0f;

        transform.Translate((xPlus - xMinus) * Time.deltaTime * moveSpeed, (yPlus - yMinus) * Time.deltaTime * moveSpeed, 0.0f);
    }
}
