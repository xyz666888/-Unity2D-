using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
public class distantSkillCD : MonoBehaviour
{
    public GameObject im;//ͼ��
    public Image mask;  //������ȴ����
    public Text cd;  //�����µ�Text
    public Text distantKey;//�����ͷŰ���
    private bool control;//���Ʊ�־
    public float cdtime;  //��ȴʱ��
    private float cultime = 0f;  //���°����󾭹���ʱ��

    //ʹ��˫�ؼ�����������̰߳�ȫ
    private static distantSkillCD instance = null;
    private static readonly object padlock = new object();
    private distantSkillCD() { }

    public static distantSkillCD Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<distantSkillCD>();
                lock (padlock)
                {
                    if (instance == null)
                    {
                        GameObject distantSkillCDObject = new GameObject("distantSkillCD");
                        instance = distantSkillCDObject.AddComponent<distantSkillCD>();
                    }
                }
            }
            return instance;
        }
    }


    // Use this for initialization
    void Start()
    {
        //��ʼ��ʱ����ť��û�б����µģ�����Ӧ�ô���EndSkill״̬
        EndSkill();  
        distantKey.text = string.Empty;
    }

    //����Ϊ�����б�
    //�����ڲ���Ҫ��ȡ�������ƣ�����洢�ļ�������ƥ�䣬�õ���Ҫ�ļ���ͼ��
    public void Study(Skill skill)
    {
        distantKey.text = "I";//��ʾ�ü��ܰ���

        //�жϸü��ܵĶ�Ӧͼ��
        im.GetComponent<Image>().sprite = skill.gameObject.GetComponent<SpriteRenderer>().sprite;//����ͼƬ
    }

    //����Ϊ�������ȴʱ��
    public void Skill(float time)
    {
        control = true;//�жϱ�־Ϊ��
        cdtime = time;
        Console.WriteLine("2�����ͷ�");
        Update();
    }

    //���ܿ�ʼ�󣬸���ͼ�Σ�Text��ʾ�ĵ���ʱ��ʼΪ3.
    private void StartSkill()
    {
        mask.fillAmount = 1;
        cd.text = cdtime.ToString();
    }

    //���ܽ��������ָ���Ϊ0��Text����ʾ���ݣ�ͬʱ����ť�����º󾭹���ʱ��ָ���0
    private void EndSkill()
    {
        mask.fillAmount = 0;
        cd.text = string.Empty;
        cultime = 0;
        control = false;//�жϱ�־��Ϊ��
    }

    // Update is called once per frame
    void Update()
    {
        if (control == true)
        {
            //���������ʱ���𲽼��٣�ֱ��Ϊ0
            //ͬʱText�е���ֵҲҪ���£�������������ʾ
            StartSkill();
            if (mask.fillAmount > 0f && mask.fillAmount <= 1f)
            {
                cultime += Time.deltaTime;
                mask.fillAmount = (cdtime - cultime) / cdtime;

                cd.text = Mathf.CeilToInt((cdtime - cultime)).ToString();
            }
            //һ����ȴʱ�䵽�ˣ�Ҳ�������ָ������Ϊ0ʱ��ִ��EndSkill������ˢ�¼���״̬
            if (mask.fillAmount == 0)
            {
                EndSkill();
            }
        }
    }
}