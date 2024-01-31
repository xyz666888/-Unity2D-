using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Coin : Prop
{
    /**
     * ���Ч������
     * ʹplayer��coins��������
     */
    public override void PickedEffect()
    {
        //������Ҫ�ı䱻ʰȡ���Ч����Ŀǰ�ǽ�player�Ľ�����Լ�һ�������ҪUI��һ�Ĺ���������д
        PlayerAttribute.Instance.goldCoins++;
        ScoreManager.Instance.CoinUpdate(PlayerAttribute.Instance.goldCoins);
        Destroy(gameObject);
    }
}
