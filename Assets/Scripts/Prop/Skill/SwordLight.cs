using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 每次攻击都会产生一道光剑击向敌人
/// </summary>
public class SwordLight : Skill
{
    [Header("瞬时性技能")]
    //光环
    public GameObject swordLight;
    //动画时间
    public float animTime;
    private GameObject[] enemy;
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
        enemy = GameObject.FindGameObjectsWithTag("Monster");
        foreach (GameObject e in enemy)
        {
            Vector3 pos = e.transform.position;
            GameObject light = Instantiate(swordLight, pos, Quaternion.identity);
            light.transform.SetParent(e.transform);
            Bad b = e.GetComponent<Bad>();
            b.TakeDamage(base.skillPower);
            Destroy(light, animTime);
        }
    }
}
