using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class SkillManager : MonoBehaviour
{
    //使用双重检查锁定尝试线程安全
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

    //传入该技能列表
    //根据技能个数判断更改
    //并存储此时技能名称，便于后续技能释放冷却判断
    private List<string> list = new List<string>();
    
   
    public void SkillStudy(List<Skill> skill)
    {
        list.Add("1");
        list.Add("2");
        //调用函数改变技能图标
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

    //使用技能，根据传入技能判断
    public void SkillUse(Skill skill)
    {
        float cdtime = (float)skill.skillCool;//从该技能类型对象中获取的冷却时间
        string name = skill.skillName;

        //判断是U、I 哪个技能需要释放，进行冷却
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
