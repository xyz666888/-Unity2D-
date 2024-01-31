using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructionWings : Skill
{
    [Header("��ʱ�Լ���")]
    //����֮��
    public GameObject destructionWings;
    //��ʱ��ʱ��
    public float delayTime;
    //���ð뾶
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
    /// ����֮���������Χ�Ե�������˺�
    /// ����֮�����һ����ʱ�����ʧ
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
        Vector3 pos = transform_.position + new Vector3(0, 1.6f, 0);
        GameObject wings = Instantiate(destructionWings, pos, Quaternion.identity);

        // ���⻷����Ϊ������Ӷ���
        wings.transform.SetParent(transform_);

        //��delayTime���ݻٻ���⻷
        Destroy(wings, delayTime);

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
