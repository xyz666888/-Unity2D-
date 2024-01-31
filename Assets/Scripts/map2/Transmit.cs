using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Transmit : MonoBehaviour
{
    private Animator anim;  
    public Transform backDoor;      //另一端位置
    public bool isDoor;     //是否进入门范围
    public bool isLight = false;    //是否点亮传送门
    private Transform playerTransform;      //玩家位置

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
    /// 判断是否触发门
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
    /// 判断是否离开门
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
    /// 实现传送功能，即将玩家移动到另一端位置
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
