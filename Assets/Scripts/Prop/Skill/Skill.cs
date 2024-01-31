using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 技能父类
/// 技能父类拥有技能的基本属性
/// 每个技能都继承自这个类
/// 通过重写 CastSkill 方法来实现技能的效果
/// </summary>
///  <author>Xiao_Yanzhe</author>
/// <see cref="https://gitee.com/Xiao_pluto"/>
public class Skill : Prop
{
    [Header("技能属性")]
    public string skillName;
    [TextArea]
    public string description;
    //仅适用近战技能
    public int skillPower;
    public bool isRemote;
    public int skillCool;
    private int skillAttribute;
    public string Tag;
    public bool isPicked;
    public float destroyDelay = 10f;

    //技能全部是群攻

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    ///  CastSkill 方法
    /// 每个技能都需要重写这个方法
    /// 在这里实现技能的效果
    /// </summary>
    public virtual void CastSkill()
    {
        // 在这里实现技能的效果
        SkillManager.Instance.SkillUse(this);
    }

    /// <summary>
    /// 改变技能的威力
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
