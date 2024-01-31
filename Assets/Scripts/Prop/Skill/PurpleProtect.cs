using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �������Լ��ܣ�����⻷
/// ����⻷�ܹ�������������˺�
/// ���ʹ�û���⻷�󣬻���������Χ����һ������⻷
/// ����⻷����һ����ʱ�����ʧ
/// ����ڻ���⻷��ʧǰ������ʹ���������ܣ������޷�ʹ�û���⻷
/// ����ڻ���⻷��ʧǰ�������ƶ��������޷�ʹ�û���⻷
/// ����ڻ���⻷��ʧǰ�����Թ����������޷�ʹ�û���⻷
/// ����ڻ���⻷��ʧǰ��������Ծ�������޷�ʹ�û���⻷
/// </summary>
/// <author>Xiao_Yanzhe</author>
/// <see cref="https://gitee.com/Xiao_pluto"/>
public class PurpleProtect : Skill
{
    [Header("��ʱ�Լ���")]
    //����⻷
    public GameObject protectiveRing;
    //��ʱ��ʱ��
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
    /// ����⻷�ܹ�������������˺�
    /// ���ʹ�û���⻷�󣬻���������Χ����һ������⻷
    /// ����⻷����һ����ʱ�����ʧ
    /// </summary>
    public override void CastSkill()
    {
        base.CastSkill();
        // �����Ϸ�����Ƿ�
        if (!gameObject.activeInHierarchy)
        {
            // �����Ϸ�����Ƿǻ�ģ��ȼ�����
            gameObject.SetActive(true);
        }

        transform_ = PlayerAttribute.Instance.GetComponent<Transform>();
        PlayerAttribute.Instance.IsGuard = true;
        Vector3 pos = transform_.position;
        GameObject ring = Instantiate(protectiveRing, pos, Quaternion.identity);

        // ���⻷����Ϊ������Ӷ���
        ring.transform.SetParent(transform_);

        //��delayTime���ݻٻ���⻷
        Destroy(ring, delayTime);

        // �� delayTime ���ִ�� ResetGuardStatusAfterDelay ����
        Invoke(nameof(ResetGuardStatusAfterDelay), delayTime);
    }

    /// <summary>
    /// �� delayTime ���ִ���������
    /// �� IsGuard ����Ϊ false
    /// </summary>
    private void ResetGuardStatusAfterDelay()
    {
        // �� IsGuard ����Ϊ false
        PlayerAttribute.Instance.IsGuard = false;
        

    }
}
