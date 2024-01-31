using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���뷢����
/// ���뷢�����ܹ���������Χ�������һ�������Ĺ���
/// �����䷢���ȥ
/// ÿ֧������ͬ�����˺�����ǿ
/// </summary>
///  <author>Xiao_Yanzhe</author>
/// <see cref="https://gitee.com/Xiao_pluto"/>
public class RainOfArrows : Skill
{
    [Header("˲ʱ�Լ���")]
    public GameObject arrowPrefab; // ������Ԥ����
    public int arrowCount; // Ҫ���ɵĹ���������
    public float radius; // �������ɵİ뾶
    private SpriteRenderer spriteRenderer; // ����� SpriteRenderer

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// ���뷢��
    /// ��������Χ�������һ�������Ĺ���
    /// �����䷢���ȥ
    /// ÿ֧������ͬ�����˺�����ǿ
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
                // �ڵ�λԲ���������һ����
                Vector2 randomPoint = Random.insideUnitCircle * radius;

                // �������ת��Ϊ�������������
                spawnPosition = transform.position + new Vector3(randomPoint.x, randomPoint.y, 0);

                // ������ɵ�λ���Ƿ���������ײ���ص�
            } while (Physics2D.OverlapCircle(spawnPosition, 0.1f, LayerMask.GetMask("Ground")));

            // ʵ��������
            GameObject arrow = Instantiate(arrowPrefab, spawnPosition, Quaternion.identity);

            // ���ü��ķ���
            SpriteRenderer arrowSpriteRenderer = arrow.GetComponent<SpriteRenderer>();
            if (arrowSpriteRenderer != null)
            {
                arrowSpriteRenderer.flipX = spriteRenderer.flipX;
            }
        }
    }
}
