using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
public class closeSkillCD : MonoBehaviour
{
    public GameObject im;//图像
    public Image mask;  //按键下的Image
    public Text cd;  //按键下的Text
    public Text closeKey;//技能释放按键
    private bool control;
    public float cdtime;  //冷却时间设置为3秒
    private float cultime = 0f;  //按下按键后经过的时间

    //使用双重检查锁定尝试线程安全
    private static closeSkillCD instance = null;
    private static readonly object padlock = new object();
    private closeSkillCD() { }

    public static closeSkillCD Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<closeSkillCD>();
                lock (padlock)
                {
                    if (instance == null)
                    {
                        GameObject closeSkillCDObject = new GameObject("closeSkillCD");
                        instance = closeSkillCDObject.AddComponent<closeSkillCD>();
                    }
                }
            }
            return instance;
        }
    }


    // Use this for initialization
    void Start()
    {
        //初始化时，按钮是没有被按下的，所以应该处于EndSkill状态
        EndSkill();
        closeKey.text = string.Empty;
    }

    //判断是否已学习该技能
    //参数为技能列表
    //函数内部需要获取技能名称，并与存储的技能名字匹配，得到需要的技能图标
    public void Study(Skill skill)
    {
        closeKey.text = "U";
        im.GetComponent<Image>().sprite = skill.gameObject.GetComponent<SpriteRenderer>().sprite;//更换图片
    }

    public void Skill(float time)
    {
        control = true;//判断标志为真
        cdtime = time;
        Console.WriteLine("1技能释放");
        Update();
    }

    // Update is called once per frame
    void Update()
    {
        if (control == true)
        {
            //覆盖面积随时间逐步减少，直到为0
            //同时Text中的数值也要更新，采用整数秒显示
            StartSkill();
            if (mask.fillAmount > 0f && mask.fillAmount <= 1f)
            {
                cultime += Time.deltaTime;
                mask.fillAmount = (cdtime - cultime) / cdtime;

                cd.text = Mathf.CeilToInt((cdtime - cultime)).ToString();
            }
            //一旦冷却时间到了，也就是遮罩覆盖面积为0时，执行EndSkill方法，刷新技能状态
            if (mask.fillAmount == 0)
            {
                EndSkill();
            }
        }
    }
    //技能开始后，覆盖图形，Text显示的倒计时初始为3.
    public void StartSkill()
    {
        mask.fillAmount = 1;
        cd.text = cdtime.ToString();
    }

    //技能结束后，遮罩覆盖为0，Text不显示内容，同时将按钮被按下后经过的时间恢复成0
    public void EndSkill()
    {
        mask.fillAmount = 0;
        cd.text = string.Empty;
        cultime = 0;
        control = false;//判断标志变为假
    }
}