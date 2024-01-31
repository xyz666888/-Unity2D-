using Assets.DeadCell.Scripts.Interface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家属性类
/// 用来记录玩家的属性
/// 包括血量，金币数量，暴怒，技巧，生存属性
/// 同时记录武器池和技能库
/// 玩家的受伤，死亡，拾取物品，属性变化等都在这里实现
/// </summary>
/// <author>Xiao_Yanzhe</author>
/// <see cref="https://gitee.com/Xiao_pluto"/>
public class PlayerAttribute : MonoBehaviour
{
    // Start is called before the first frame update
    //血量
    public int maxBlood = 100;
    public int currentBlood = 100;
    public int goldCoins = 0;
    public int bloodBottles = 0;
    //属性
    public List<int> attribute;
    //武器池
    public List<Weapons> weapons = new List<Weapons>();
    //技能库
    public List<Skill> skills = new List<Skill>();

    public Inventory inventory;
    public GameObject DeadCanvas;
    public GameObject dragonEffect;
    public GameObject dragonNPC;
    private bool isGround = true;
    private bool isGuard = false;
    public bool canClimb = false;
    public bool isClimbing = false;
    private Animator animator;
    private static PlayerAttribute instance;
    private PlayerController player;
    [Header("武器背包，用于交换武器")]
    public GameObject WeaponBag;
    private bool isOpened_W = false;
    [Header("技能背包，用于交换技能")]
    public GameObject SkillBag;
    private bool isOpened_S = false;
    /// <summary>
    /// 单例模式
    /// 通过这个方法获取PlayerAttribute实例
    /// </summary>
    public static PlayerAttribute Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerAttribute>();
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(PlayerAttribute).ToString();
                    instance = obj.AddComponent<PlayerAttribute>();
                }
            }
            return instance;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public bool IsGround
    { 
        get { return isGround; } 
    }

    public bool IsGuard
    {
        get { return isGuard; }
        set { isGuard = value; }
    }

    void Start()
    {
        //初始化属性
        animator = GetComponent<Animator>(); 
        player = GetComponent<PlayerController>();
        attribute = new List<int> {1, 1, 1};
        
    }

    // Update is called once per frame
    void Update()
    {
        print(this.canClimb);
    }

    /// <summary>
    ///  受伤方法
    /// 如果是防御状态则不受伤
    /// 如果被击飞则播放跌落动画
    /// 否则播放受伤动画
    /// 如果血量为0则播放死亡动画
    /// </summary>
    /// <param name="hurt"></param>
    /// <param name="isFall"></param>
    public void Hurt(int hurt, bool isFall)
    {
        if (isGuard)
        {
            return;
        }
        this.currentBlood -= hurt;
        Blood.Instance.BloodSumUpdate(this.currentBlood);
        if (isFall)
        {
            animator.SetTrigger("Fall");
        }
        else
        {
            animator.SetTrigger("Hurt");
        }
        if(currentBlood <= 20)
        {
            Vector3 _pos = this.transform.position;
            Instantiate(dragonNPC,new Vector3(_pos.x, _pos.y + 5, _pos.z), Quaternion.identity);
            GameObject boss = GameObject.FindGameObjectWithTag("Monster");
            Vector3 pos = boss.transform.position;
            var dragon = Instantiate(dragonEffect, new Vector3(pos.x, pos.y + 1, pos.z), Quaternion.identity);
            dragon.transform.SetParent(boss.transform);

        }
        if (currentBlood <= 0)
        {
            //死亡
            animator.SetTrigger("Die");

        }
    }

    /// <summary>
    ///  血量改变
    /// </summary>
    /// <param name="attribute"></param>
    public void BloodChange(List<int> attribute)
    {
        if (attribute.Count != 3)
        {
            Debug.Log("属性数量不对");
            return;
        }
        this.currentBlood = (int)((attribute[0] * 0.60 + 1) * 100);
        this.currentBlood = (int)((attribute[1] * 0.32 + 1) * 100);
        this.currentBlood = (int)((attribute[2] * 0.70 + 1) * 100);
    }

    /// <summary>
    /// 卷轴引起属性的变化
    /// </summary>
    /// <param name="change"></param>
    public void AttributeChange(List<int> change)
    {
        if (change.Count != 3)
        {
            Debug.Log("属性数量不对");
            return;
        }
        this.attribute[0] += change[0];
        this.attribute[1] += change[1];
        this.attribute[2] += change[2];

        foreach(Weapons weapon in weapons)
        {
            weapon.GetComponentInChildren<Weapons>().AttackPowerChanged();
        }
    }

    public void ChangeWeaponSkill()
    {
        player.UpdateWeaponSkill();
        WeaponManager.Instance.WeaponUpdate(weapons);
        SkillManager.Instance.SkillStudy(skills);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        //如果碰到地面
        if (collision.collider.tag == "Ground")
        {
            isGround = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //如果离开地面
        if (collision.collider.tag == "Ground")
        {
            isGround = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("LadderTop"))
        {
            Debug.Log("玩家已经爬完了楼梯");
            this.canClimb = false;
            this.isClimbing = false;

        }
        if (other.gameObject.CompareTag("Ladder"))
        {
            Debug.Log("玩家可以开始爬楼梯了");
            this.canClimb = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ladder"))
        {
            Debug.Log("玩家离开了楼梯");
            this.canClimb = false;
            this.isClimbing = false;
        }
    }
}