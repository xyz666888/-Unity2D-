using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Coin : Prop
{
    /**
     * 金币效果方法
     * 使player的coins属性增加
     */
    public override void PickedEffect()
    {
        //根据需要改变被拾取后的效果，目前是将player的金币属性加一，如果需要UI加一的功能在这里写
        PlayerAttribute.Instance.goldCoins++;
        ScoreManager.Instance.CoinUpdate(PlayerAttribute.Instance.goldCoins);
        Destroy(gameObject);
    }
}
