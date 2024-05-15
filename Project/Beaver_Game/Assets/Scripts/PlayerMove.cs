using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 10.0f;
    public bool leftRightChange = false;
    public RopeManager ropeManager;
    Animator animator;


    void SetClawPos()
    {
        if (this.transform.childCount > 3)
        {
            for (int i = 3; i < this.transform.childCount; i++)
            {
                if (this.transform.GetChild(i).gameObject.GetComponent<ItemInfo>().GetItemIndexNumber() == 18)
                {
                    if (animator.GetBool("Walk"))
                    {
                        this.transform.GetChild(i).localPosition = new Vector3(-1.25f, -3.3f, 0.0f);
                        this.transform.GetChild(i).localRotation = Quaternion.Euler(0.0f, 0.0f, -40.0f);
                    }
                    else
                    {
                        this.transform.GetChild(i).localPosition = new Vector3(1.25f, -3.3f, 0.0f);
                        this.transform.GetChild(i).localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                    }
                    break;
                }
            }
        }
    }

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float moveY = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        if (moveX != 0.0f || moveY != 0.0f)
        {
            animator.SetBool("Walk", true);
        }
        else
        {
            animator.SetBool("Walk", false);
        }
        SetClawPos();

        if (moveX < 0.0f && !leftRightChange)
        {
            leftRightChange = true;
            this.transform.localScale = new Vector3(this.transform.localScale.x * -1, this.transform.localScale.y, this.transform.localScale.z);
            ropeManager.ThrowRopeLineLeftRightChange();
        }
        else if (moveX > 0.0f && leftRightChange)
        {
            leftRightChange = false;
            this.transform.localScale = new Vector3(this.transform.localScale.x * -1, this.transform.localScale.y, this.transform.localScale.z);
            ropeManager.ThrowRopeLineLeftRightChange();
        }

        transform.Translate(new Vector3(moveX, moveY, 0.0f));

    }
}
