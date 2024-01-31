using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 万剑齐发技能
/// 万剑齐发技能能够在人物周围随机生成一定数量的弓箭
/// 而后将其发射出去
/// 每支箭都有同样的伤害，贼强
/// </summary>
///  <author>Xiao_Yanzhe</author>
/// <see cref="https://gitee.com/Xiao_pluto"/>
public class RainOfArrows : Skill
{
    [Header("瞬时性技能")]
    public GameObject arrowPrefab; // 弓箭的预制体
    public int arrowCount; // 要生成的弓箭的数量
    public float radius; // 弓箭生成的半径
    private SpriteRenderer spriteRenderer; // 人物的 SpriteRenderer

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 万剑齐发：
    /// 在人物周围随机生成一定数量的弓箭
    /// 而后将其发射出去
    /// 每支箭都有同样的伤害，贼强
    /// </summary>
    public override void CastSkill()
    {
        base.CastSkill();
        spriteRenderer = PlayerAttribute.Instance.GetComponent<SpriteRenderer>();
        for (int i = 0; i < arrowCount; i++)
        {
            Vector3 spawnPosition;
            do
            {
                // 在单位圆内随机生成一个点
                Vector2 randomPoint = Random.insideUnitCircle * radius;

                // 将这个点转换为人物的世界坐标
                spawnPosition = transform.position + new Vector3(randomPoint.x, randomPoint.y, 0);

                // 检测生成的位置是否与地面的碰撞体重叠
            } while (Physics2D.OverlapCircle(spawnPosition, 0.1f, LayerMask.GetMask("Ground")));

            // 实例化弓箭
            GameObject arrow = Instantiate(arrowPrefab, spawnPosition, Quaternion.identity);

            // 设置箭的方向
            SpriteRenderer arrowSpriteRenderer = arrow.GetComponent<SpriteRenderer>();
            if (arrowSpriteRenderer != null)
            {
                arrowSpriteRenderer.flipX = spriteRenderer.flipX;
            }
        }
    }
}
