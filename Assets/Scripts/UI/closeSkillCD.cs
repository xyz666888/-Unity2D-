using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
public class closeSkillCD : MonoBehaviour
{
    public GameObject im;//ͼ��
    public Image mask;  //�����µ�Image
    public Text cd;  //�����µ�Text
    public Text closeKey;//�����ͷŰ���
    private bool control;
    public float cdtime;  //��ȴʱ������Ϊ3��
    private float cultime = 0f;  //���°����󾭹���ʱ��

    //ʹ��˫�ؼ�����������̰߳�ȫ
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
        //��ʼ��ʱ����ť��û�б����µģ�����Ӧ�ô���EndSkill״̬
        EndSkill();
        closeKey.text = string.Empty;
    }

    //�ж��Ƿ���ѧϰ�ü���
    //����Ϊ�����б�
    //�����ڲ���Ҫ��ȡ�������ƣ�����洢�ļ�������ƥ�䣬�õ���Ҫ�ļ���ͼ��
    public void Study(Skill skill)
    {
        closeKey.text = "U";
        im.GetComponent<Image>().sprite = skill.gameObject.GetComponent<SpriteRenderer>().sprite;//����ͼƬ
    }

    public void Skill(float time)
    {
        control = true;//�жϱ�־Ϊ��
        cdtime = time;
        Console.WriteLine("1�����ͷ�");
        Update();
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
    //���ܿ�ʼ�󣬸���ͼ�Σ�Text��ʾ�ĵ���ʱ��ʼΪ3.
    public void StartSkill()
    {
        mask.fillAmount = 1;
        cd.text = cdtime.ToString();
    }

    //���ܽ��������ָ���Ϊ0��Text����ʾ���ݣ�ͬʱ����ť�����º󾭹���ʱ��ָ���0
    public void EndSkill()
    {
        mask.fillAmount = 0;
        cd.text = string.Empty;
        cultime = 0;
        control = false;//�жϱ�־��Ϊ��
    }
}