using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 人物前面发射爆炸光波
/// 范围内的敌人受到大量伤害
/// </summary>
public class LightExplosion : Skill
{
    [Header("瞬时性技能")]
    //光环
    public GameObject lightExplosion;
    //动画时间
    public float animTime;
    public float radius;
    public string[] attackTag;
    private GameObject[] enemy;
    private Transform ownerTransform;
    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void CastSkill()
    {
        base.CastSkill();
        Vector3 pos = PlayerAttribute.Instance.transform.position;
        spriteRenderer = PlayerAttribute.Instance.gameObject.GetComponent<SpriteRenderer>();
        Vector3 newPos = new Vector3(pos.x + (spriteRenderer.flipX ? -1 : 1) * 3f, pos.y + 1.6f, pos.z);
        lightExplosion.GetComponent<SpriteRenderer>().flipX = !spriteRenderer.flipX;
        GameObject light = Instantiate(lightExplosion, newPos, Quaternion.identity);
        Destroy(light, animTime);
        enemy = FindEnemy();
        if (enemy == null || enemy.Length == 0)
        {
            return;
        }
        foreach (GameObject e in enemy)
        {
            Bad b = e.GetComponent<Bad>();
            b.TakeDamage(base.skillPower);
        }

    }

    private GameObject[] FindEnemy()
    {
        ownerTransform = PlayerAttribute.Instance.gameObject.transform;
        spriteRenderer = PlayerAttribute.Instance.gameObject.GetComponent<SpriteRenderer>();
        var colliders = Physics2D.OverlapCircleAll(ownerTransform.position, radius);
        if (colliders == null || colliders.Length == 0)
        {
            return null;
        }
        var allCanBeSelected = colliders.FindAll(c => (Array.IndexOf(attackTag, c.tag) >= 0));
        if (allCanBeSelected == null || allCanBeSelected.Length == 0)
        {
            return null;
        }
        allCanBeSelected = allCanBeSelected.Where(c => (c.gameObject.GetComponent<Transform>().position.x -
            ownerTransform.position.x) * (spriteRenderer.flipX ? -1 : 1) > 0).ToArray();
        return allCanBeSelected.Select(a => a.gameObject).ToArray();
    }
}
