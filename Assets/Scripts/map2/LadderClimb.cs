using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ʵ�������������ӵĹ���
/// </summary>
public class LadderClimb : MonoBehaviour
{
    public float climbSpeed = 3f;

    private bool isClimbing;    //����״̬

    private void Start()
    {
        isClimbing = false;
    }

    /// <summary>
    /// ��������ʱ���������в���
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            float inputVertical = Input.GetAxis("Vertical");

            //����ֱ���벻Ϊ0����������״̬Ϊ�棬���޸Ľ�ɫ����
            if (inputVertical != 0f)
            {
                isClimbing = true;
                Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
                //�����ٶ�ʸ���޸�Ϊ��ֱ����������Ϊ0
                rb.velocity = new Vector2(rb.velocity.x, inputVertical * climbSpeed);
                rb.gravityScale = 0f;
            }
            else
            {
                //��ֱ����Ϊ0ʱ����ɫ��һ���ٶ����»���
                isClimbing = false;
                Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
                rb.velocity = new Vector2(rb.velocity.x, 0f);
                rb.gravityScale = 1f;
            }
        }
    }

    /// <summary>
    /// �˳���ײ��ʱ�޸�����״̬���������
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

