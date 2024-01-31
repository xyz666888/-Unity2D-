using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Scroll : Prop
{
    //暴怒属性
    public int rageAttributeChange;
    //战术属性
    public int tacticalAttributeChange;
    //生存属性
    public int survialAttributeChange;
    /**
     * 卷轴效果方法
     * 碰撞触发且按F时调用
     */
    public override void PickedEffect()
    {
        List<int> attribute = PlayerAttribute.Instance.attribute;
        if (attribute.Count != 3)
        {
            Debug.Log("属性数量不对");
            return;
        }
        PlayerAttribute.Instance.attribute[0] += rageAttributeChange;
        PlayerAttribute.Instance.attribute[1] += tacticalAttributeChange;
        PlayerAttribute.Instance.attribute[2] += survialAttributeChange;
        foreach (Weapons weapon in PlayerAttribute.Instance.weapons)
        {
            weapon.GetComponentInChildren<Weapons>().AttackPowerChanged();
        }
        ScoreManager.Instance.AttributeUpdate(PlayerAttribute.Instance.attribute);
        Destroy(gameObject);
    }
           
}
