using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class SkillManager : MonoBehaviour
{
    //ʹ��˫�ؼ�����������̰߳�ȫ
    private static SkillManager instance = null;
    private static readonly object padlock = new object();
    private SkillManager() { }

    public static SkillManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SkillManager>();
                lock (padlock)
                {
                    if (instance == null)
                    {
                        GameObject skillManagerObject = new GameObject("SkillManager");
                        instance = skillManagerObject.AddComponent<SkillManager>();
                    }
                }
            }
            return instance;
        }
    }

    //����ü����б�
    //���ݼ��ܸ����жϸ���
    //���洢��ʱ�������ƣ����ں��������ͷ���ȴ�ж�
    private List<string> list = new List<string>();
    
   
    public void SkillStudy(List<Skill> skill)
    {
        list.Add("1");
        list.Add("2");
        //���ú����ı似��ͼ��
        if (skill.Count == 0)
        {
            list.Clear();
            return;
        }
        if (skill.Count == 1)
        {
            list[0] = skill[0].skillName;
            closeSkillCD.Instance.Study(skill[0]);
        }
        else
        {
            list[0] = skill[0].skillName;
            list[1] = skill[1].skillName;
            closeSkillCD.Instance.Study(skill[0]);
            distantSkillCD.Instance.Study(skill[1]);
        }
    }

    //ʹ�ü��ܣ����ݴ��뼼���ж�
    public void SkillUse(Skill skill)
    {
        float cdtime = (float)skill.skillCool;//�Ӹü������Ͷ����л�ȡ����ȴʱ��
        string name = skill.skillName;

        //�ж���U��I �ĸ�������Ҫ�ͷţ�������ȴ
        if (name == list[0])
        {
            closeSkillCD.Instance.Skill(cdtime);
        }
        else if (name == list[1])
        {
            distantSkillCD.Instance.Skill(cdtime);
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
