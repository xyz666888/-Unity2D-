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
    private bool control = false;//����һ�����������ж� Update �ں����Ƿ���Ҫ����ִ��
    public float currentBlood = 1f;
    public Color bloodColor = Color.green;
    public float bloodMoveSpeed;

    //ʹ��˫�ؼ�����������̰߳�ȫ
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
        //��ʼ��Ѫ��ͼ�꣬��ȡ���
        image = GetComponent<Image>();
        image.color = bloodColor;
        image.fillAmount = currentBlood;
    }

    //����Ѫ���仯����ý�Ҫ�ﵽ��Ŀ��Ѫ��
    public void BloodUpdate(int target) 
    {
        //����ת������ֵת��Ϊ 0-1 ֮��ĵ����ȸ�����
        targetBlood = (float)target / 100;
        //׼��ִ�� Update����֡����Ѫ����Ϣ
        control = true;
        Update();
    }

    // Update is called once per frame
    void Update()
    {       
        if (control)//Ϊ��
        {
            //�ϸ����������߲������
            //�����жϾ���ֵ��С�ķ�ʽֹͣѪ���任
            if (Math.Abs(targetBlood - currentBlood) > 0.0001)
            {
                currentBlood = Mathf.Lerp(currentBlood, targetBlood, bloodMoveSpeed * Time.deltaTime);
                image.fillAmount = currentBlood;
            }
            else
            {
                control = false;//�����жϱ�־
            }
        }
    }
}

