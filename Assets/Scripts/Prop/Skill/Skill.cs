using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���ܸ���
/// ���ܸ���ӵ�м��ܵĻ�������
/// ÿ�����ܶ��̳��������
/// ͨ����д CastSkill ������ʵ�ּ��ܵ�Ч��
/// </summary>
///  <author>Xiao_Yanzhe</author>
/// <see cref="https://gitee.com/Xiao_pluto"/>
public class Skill : Prop
{
    [Header("��������")]
    public string skillName;
    [TextArea]
    public string description;
    //�����ý�ս����
    public int skillPower;
    public bool isRemote;
    public int skillCool;
    private int skillAttribute;
    public string Tag;
    public bool isPicked;
    public float destroyDelay = 10f;

    //����ȫ����Ⱥ��

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    ///  CastSkill ����
    /// ÿ�����ܶ���Ҫ��д�������
    /// ������ʵ�ּ��ܵ�Ч��
    /// </summary>
    public virtual void CastSkill()
    {
        // ������ʵ�ּ��ܵ�Ч��
        SkillManager.Instance.SkillUse(this);
    }

    /// <summary>
    /// �ı似�ܵ�����
    /// </summary>
    public void SkillPowerChanged()
    {
        switch (Tag)
        {
            case "Rage":
                this.skillPower = (int)((float)this.skillPower * (PlayerAttribute.Instance.attribute[0] * (1 + 0.15f)));
                break;
            case "Tactical":
                this.skillPower = (int)((float)this.skillPower * (PlayerAttribute.Instance.attribute[1] * (1 + 0.15f)));
                break;
            case "Survial":
                this.skillPower = (int)((float)this.skillPower * (PlayerAttribute.Instance.attribute[2] * (1 + 0.15f)));
                break;
        }
    }

    public override void PickedEffect()
    {
        isPicked = true;
        if(PlayerAttribute.Instance.skills.Count == 0)
        {
            PlayerAttribute.Instance.skills.Add(this);
            SpriteRenderer spriteRenderer = this.GetComponent<SpriteRenderer>();
            spriteRenderer.sortingOrder = -100;
        }
        else if (PlayerAttribute.Instance.skills.Count == 1)
        {
            if (PlayerAttribute.Instance.skills[0].skillName != this.skillName)
            {
                PlayerAttribute.Instance.skills.Add(this);
                SpriteRenderer spriteRenderer = this.GetComponent<SpriteRenderer>();
                spriteRenderer.sortingOrder = -100;
            }
        }
        else if (PlayerAttribute.Instance.skills[0].skillName != this.skillName && PlayerAttribute.Instance.skills[1].skillName != this.skillName)
        {
            ChangeInventory.Instance.CreatNewItem(this);
            SpriteRenderer sprite = this.GetComponent<SpriteRenderer>();
            sprite.sortingOrder = -100;
        }

    }

    public void DestroyProp()
    {
        if(!isPicked)
        {
            Destroy(gameObject);
        }
    }

}
