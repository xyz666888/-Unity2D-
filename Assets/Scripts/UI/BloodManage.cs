using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class BloodManage : MonoBehaviour
{
    Image image;
    private float targetBlood;
    private bool control = false;//引入一个控制量，判断 Update 内函数是否需要继续执行
    public float currentBlood = 1f;
    public Color bloodColor = Color.green;
    public float bloodMoveSpeed;

    //使用双重检查锁定尝试线程安全
    private static BloodManage instance = null;
    private static readonly object padlock = new object();
    private BloodManage() { }

    public static BloodManage Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<BloodManage>();
                lock (padlock)
                {
                    if (instance == null)
                    {
                        GameObject bloodManageObject = new GameObject("BloodManage");
                        instance = bloodManageObject.AddComponent<BloodManage>();
                    }
                }
            }
            return instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //初始化血条图标，获取组件
        image = GetComponent<Image>();
        image.color = bloodColor;
        image.fillAmount = currentBlood;
    }

    //更新血条变化，获得将要达到的目标血量
    public void BloodUpdate(int target) 
    {
        //类型转换，将值转换为 0-1 之间的单精度浮点型
        targetBlood = (float)target / 100;
        //准备执行 Update，逐帧更新血条信息
        control = true;
        Update();
    }

    // Update is called once per frame
    void Update()
    {       
        if (control)//为真
        {
            //严格意义上两者不会相等
            //采用判断绝对值大小的方式停止血条变换
            if (Math.Abs(targetBlood - currentBlood) > 0.0001)
            {
                currentBlood = Mathf.Lerp(currentBlood, targetBlood, bloodMoveSpeed * Time.deltaTime);
                image.fillAmount = currentBlood;
            }
            else
            {
                control = false;//更改判断标志
            }
        }
    }
}

