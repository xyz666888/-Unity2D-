using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ÿ�ι����������һ���⽣�������
/// </summary>
public class SwordLight : Skill
{
    [Header("˲ʱ�Լ���")]
    //�⻷
    public GameObject swordLight;
    //����ʱ��
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
