using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : Prop
{

    public override void PickedEffect()
    {
        if(PlayerAttribute.Instance.bloodBottles == 3)
        {
            Debug.Log("�޷�ʰȡ��Ѫƿ�����Ѵ�����");
            return;
        }
        PlayerAttribute.Instance.bloodBottles++;
        PotionNumManager.Instance.PotionNumUpdate(PlayerAttribute.Instance.bloodBottles);
        Destroy(gameObject);
    }

    public static void PotionEffect()
    {
        if (PlayerAttribute.Instance.bloodBottles == 0 || PlayerAttribute.Instance.currentBlood == PlayerAttribute.Instance.maxBlood)
            return;
        
        PlayerAttribute.Instance.currentBlood += 20;
        if(PlayerAttribute.Instance.currentBlood > PlayerAttribute.Instance.maxBlood)
            PlayerAttribute.Instance.currentBlood = PlayerAttribute.Instance.maxBlood;
        PlayerAttribute.Instance.bloodBottles--;
        PotionNumManager.Instance.PotionNumUpdate(PlayerAttribute.Instance.bloodBottles);
        Blood.Instance.BloodSumUpdate(PlayerAttribute.Instance.currentBlood);
    }
}
