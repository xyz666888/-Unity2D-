using System.Collections;
using System.Collections.Generic;

using UnityEngine;

/// <summary>
/// /// 武器类
/// 武器类拥有武器的基本属性
/// 每个武器都继承自这个类
/// </summary>
public class Weapons : Prop
{
    [Header("武器属性")]
    //武器名称
    public string weaponName;
    //武器标签
    public string Tag;
    // 武器远程与否
    public bool isRemote;
    //武器攻击力
    public int attackPower;
    //判定范围
    public const float distance = 0.5f;
    [TextArea]
    //武器描述
    public string description;
    [Header("攻击间隔")]
    //攻击间隔
    public float attackInterval;
    [Header("仅限近战武器")]
    //近战攻击距离（远程武器无效）
    public float attackDistance;
    //群攻Group/单攻Single
    public string attackType;
    [Header("可攻击的标签")]
    //可攻击的标签
    public string[] attackTag = new string[2];
    [Header("仅限远程武器")]
    //实例化的对象，仅限远程武器
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
    /// 攻击力改变
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
