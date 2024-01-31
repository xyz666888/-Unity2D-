using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 实现人物攀爬梯子的功能
/// </summary>
public class LadderClimb : MonoBehaviour
{
    public float climbSpeed = 3f;

    private bool isClimbing;    //攀爬状态

    private void Start()
    {
        isClimbing = false;
    }

    /// <summary>
    /// 触发梯子时，持续进行操作
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            float inputVertical = Input.GetAxis("Vertical");

            //若垂直输入不为0，设置攀爬状态为真，并修改角色刚体
            if (inputVertical != 0f)
            {
                isClimbing = true;
                Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
                //刚体速度矢量修改为垂直，重力设置为0
                rb.velocity = new Vector2(rb.velocity.x, inputVertical * climbSpeed);
                rb.gravityScale = 0f;
            }
            else
            {
                //垂直输入为0时，角色以一定速度向下滑动
                isClimbing = false;
                Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
                rb.velocity = new Vector2(rb.velocity.x, 0f);
                rb.gravityScale = 1f;
            }
        }
    }

    /// <summary>
    /// 退出碰撞体时修改攀爬状态与刚体属性
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            isClimbing = false;
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.gravityScale = 1f;
        }
    }
}

