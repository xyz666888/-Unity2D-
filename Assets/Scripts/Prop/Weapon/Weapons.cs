using System.Collections;
using System.Collections.Generic;

using UnityEngine;

/// <summary>
/// /// ������
/// ������ӵ�������Ļ�������
/// ÿ���������̳��������
/// </summary>
public class Weapons : Prop
{
    [Header("��������")]
    //��������
    public string weaponName;
    //������ǩ
    public string Tag;
    // ����Զ�����
    public bool isRemote;
    //����������
    public int attackPower;
    //�ж���Χ
    public const float distance = 0.5f;
    [TextArea]
    //��������
    public string description;
    [Header("�������")]
    //�������
    public float attackInterval;
    [Header("���޽�ս����")]
    //��ս�������루Զ��������Ч��
    public float attackDistance;
    //Ⱥ��Group/����Single
    public string attackType;
    [Header("�ɹ����ı�ǩ")]
    //�ɹ����ı�ǩ
    public string[] attackTag = new string[2];
    [Header("����Զ������")]
    //ʵ�����Ķ��󣬽���Զ������
    public GameObject remoteGameObject;

    // Start is called before the first frame update
    void Start()
    {
        //DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    /// <summary>
    /// �������ı�
    /// </summary>
    public void AttackPowerChanged()
    {
        switch (Tag)
        {
            case "Rage":
                this.attackPower = (int)((float)this.attackPower * (PlayerAttribute.Instance.attribute[0] * (1 + 0.15f)));
                break; 
            case "Tactical":
                this.attackPower = (int)((float)this.attackPower * (PlayerAttribute.Instance.attribute[1] * (1 + 0.15f)));
                break;
            case "Survial":
                this.attackPower = (int)((float)this.attackPower * (PlayerAttribute.Instance.attribute[2] * (1 + 0.15f)));
                break;
        }
    }

    public override void PickedEffect()
    {
        if (PlayerAttribute.Instance.weapons.Count == 0)
        {
            PlayerAttribute.Instance.weapons.Add(this);
            SpriteRenderer spriteRenderer = this.GetComponent<SpriteRenderer>();
            spriteRenderer.sortingOrder = -100;
            this.gameObject.SetActive(false);
        }
        else if (PlayerAttribute.Instance.weapons.Count == 1 && PlayerAttribute.Instance.weapons[0].weaponName != this.weaponName)
        {
            PlayerAttribute.Instance.weapons.Add(this);
            SpriteRenderer spriteRenderer = this.GetComponent<SpriteRenderer>();
            spriteRenderer.sortingOrder = -100;
            this.gameObject.SetActive(false);
        }
        else
        {
            if (PlayerAttribute.Instance.weapons[0].weaponName != this.weaponName && PlayerAttribute.Instance.weapons[1].weaponName != this.weaponName)
            {
                ChangeInventory.Instance.CreatNewItem(this);
                SpriteRenderer spriteRenderer = this.GetComponent<SpriteRenderer>();
                spriteRenderer.sortingOrder = -100;
                this.gameObject.SetActive(false);
            }
                
        }

    }



}
