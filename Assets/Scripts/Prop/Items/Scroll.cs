using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Scroll : Prop
{
    //��ŭ����
    public int rageAttributeChange;
    //ս������
    public int tacticalAttributeChange;
    //��������
    public int survialAttributeChange;
    /**
     * ����Ч������
     * ��ײ�����Ұ�Fʱ����
     */
    public override void PickedEffect()
    {
        List<int> attribute = PlayerAttribute.Instance.attribute;
        if (attribute.Count != 3)
        {
            Debug.Log("������������");
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
