using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Transmit : MonoBehaviour
{
    private Animator anim;  
    public Transform backDoor;      //��һ��λ��
    public bool isDoor;     //�Ƿ�����ŷ�Χ
    public bool isLight = false;    //�Ƿ����������
    private Transform playerTransform;      //���λ��

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void SetAnimation()
    {
        anim.SetBool("isLight", isLight);
    }

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            EnterDoor();
        }
    }

    /// <summary>
    /// �ж��Ƿ񴥷���
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.GetType().ToString() == "UnityEngine.BoxCollider2D")
        {
            isDoor = true;
        }
    }

    /// <summary>
    /// �ж��Ƿ��뿪��
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.GetType().ToString() == "UnityEngine.BoxCollider2D")
        {
            isDoor = false;
        }
    }

    /// <summary>
    /// ʵ�ִ��͹��ܣ���������ƶ�����һ��λ��
    /// </summary>
    private void EnterDoor()
    {
        if (isDoor)
        {
            if (isLight)
            {
                playerTransform.position = backDoor.position;
            }
            else
            {
                isLight = true;
            }
            SetAnimation();
        }
    }
}
