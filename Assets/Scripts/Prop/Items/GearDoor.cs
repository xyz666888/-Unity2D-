using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearDoor : MonoBehaviour
{
    //动画控制器
    private Animator animator;
    //判断是否被打开
    private bool isopened;
    //动画等待时间
    private int seconds;
    //使用协程的引用
    private Coroutine openCoroutine;


    // Start is called before the first frame update
    void Start()
    {
        //初始化动画控制器，标志，动画需要等待的时间
        animator = GetComponent<Animator>();
        isopened = false;
        seconds = 2;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenGearDoor()
    {
        if(!isopened)
        {
            //播放开门动画
            animator.SetTrigger("openning");
            openCoroutine = StartCoroutine(Opening());
        }
    }

    IEnumerator Opening()
    {
        //等待动画播放完毕
        yield return new WaitForSeconds(seconds);
        //摧毁对象
        Destroy(gameObject);
    }
}
