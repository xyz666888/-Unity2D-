using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructionWings : Skill
{
    [Header("延时性技能")]
    //毁灭之翼
    public GameObject destructionWings;
    //延时的时间
    public float delayTime;
    //作用半径
    public float radius;
    private Transform transform_;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyProp", destroyDelay);
        transform_ = PlayerAttribute.Instance.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 毁灭之翼可以在周围对敌人造成伤害
    /// 毁灭之翼会在一定的时间后消失
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
        Vector3 pos = transform_.position + new Vector3(0, 1.6f, 0);
        GameObject wings = Instantiate(destructionWings, pos, Quaternion.identity);

        // 将光环设置为人物的子对象
        wings.transform.SetParent(transform_);

        //在delayTime秒后摧毁护体光环
        Destroy(wings, delayTime);

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
