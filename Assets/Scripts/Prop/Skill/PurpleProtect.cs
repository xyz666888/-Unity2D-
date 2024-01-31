using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 生存属性技能：护体光环
/// 护体光环能够保护玩家免受伤害
/// 玩家使用护体光环后，会在自身周围产生一个护体光环
/// 护体光环会在一定的时间后消失
/// 玩家在护体光环消失前，可以使用其他技能，但是无法使用护体光环
/// 玩家在护体光环消失前，可以移动，但是无法使用护体光环
/// 玩家在护体光环消失前，可以攻击，但是无法使用护体光环
/// 玩家在护体光环消失前，可以跳跃，但是无法使用护体光环
/// </summary>
/// <author>Xiao_Yanzhe</author>
/// <see cref="https://gitee.com/Xiao_pluto"/>
public class PurpleProtect : Skill
{
    [Header("延时性技能")]
    //护体光环
    public GameObject protectiveRing;
    //延时的时间
    public float delayTime;
    private Transform transform_;
    // Start is called before the first frame update
    void Start()
    {
        transform_ = PlayerAttribute.Instance.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    /// <summary>
    /// 护体光环能够保护玩家免受伤害
    /// 玩家使用护体光环后，会在自身周围产生一个护体光环
    /// 护体光环会在一定的时间后消失
    /// </summary>
    public override void CastSkill()
    {
        base.CastSkill();
        // 检查游戏对象是否活动
        if (!gameObject.activeInHierarchy)
        {
            // 如果游戏对象是非活动的，先激活它
            gameObject.SetActive(true);
        }

        transform_ = PlayerAttribute.Instance.GetComponent<Transform>();
        PlayerAttribute.Instance.IsGuard = true;
        Vector3 pos = transform_.position;
        GameObject ring = Instantiate(protectiveRing, pos, Quaternion.identity);

        // 将光环设置为人物的子对象
        ring.transform.SetParent(transform_);

        //在delayTime秒后摧毁护体光环
        Destroy(ring, delayTime);

        // 在 delayTime 秒后执行 ResetGuardStatusAfterDelay 方法
        Invoke(nameof(ResetGuardStatusAfterDelay), delayTime);
    }

    /// <summary>
    /// 在 delayTime 秒后执行这个方法
    /// 将 IsGuard 设置为 false
    /// </summary>
    private void ResetGuardStatusAfterDelay()
    {
        // 将 IsGuard 设置为 false
        PlayerAttribute.Instance.IsGuard = false;
        

    }
}
