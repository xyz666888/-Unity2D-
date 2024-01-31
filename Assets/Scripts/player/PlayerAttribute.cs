using Assets.DeadCell.Scripts.Interface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���������
/// ������¼��ҵ�����
/// ����Ѫ���������������ŭ�����ɣ���������
/// ͬʱ��¼�����غͼ��ܿ�
/// ��ҵ����ˣ�������ʰȡ��Ʒ�����Ա仯�ȶ�������ʵ��
/// </summary>
/// <author>Xiao_Yanzhe</author>
/// <see cref="https://gitee.com/Xiao_pluto"/>
public class PlayerAttribute : MonoBehaviour
{
    // Start is called before the first frame update
    //Ѫ��
    public int maxBlood = 100;
    public int currentBlood = 100;
    public int goldCoins = 0;
    public int bloodBottles = 0;
    //����
    public List<int> attribute;
    //������
    public List<Weapons> weapons = new List<Weapons>();
    //���ܿ�
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
    [Header("�������������ڽ�������")]
    public GameObject WeaponBag;
    private bool isOpened_W = false;
    [Header("���ܱ��������ڽ�������")]
    public GameObject SkillBag;
    private bool isOpened_S = false;
    /// <summary>
    /// ����ģʽ
    /// ͨ�����������ȡPlayerAttributeʵ��
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
        //��ʼ������
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
    ///  ���˷���
    /// ����Ƿ���״̬������
    /// ����������򲥷ŵ��䶯��
    /// ���򲥷����˶���
    /// ���Ѫ��Ϊ0�򲥷���������
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
            //����
            animator.SetTrigger("Die");

        }
    }

    /// <summary>
    ///  Ѫ���ı�
    /// </summary>
    /// <param name="attribute"></param>
    public void BloodChange(List<int> attribute)
    {
        if (attribute.Count != 3)
        {
            Debug.Log("������������");
            return;
        }
        this.currentBlood = (int)((attribute[0] * 0.60 + 1) * 100);
        this.currentBlood = (int)((attribute[1] * 0.32 + 1) * 100);
        this.currentBlood = (int)((attribute[2] * 0.70 + 1) * 100);
    }

    /// <summary>
    /// �����������Եı仯
    /// </summary>
    /// <param name="change"></param>
    public void AttributeChange(List<int> change)
    {
        if (change.Count != 3)
        {
            Debug.Log("������������");
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
        //�����������
        if (collision.collider.tag == "Ground")
        {
            isGround = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //����뿪����
        if (collision.collider.tag == "Ground")
        {
            isGround = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("LadderTop"))
        {
            Debug.Log("����Ѿ�������¥��");
            this.canClimb = false;
            this.isClimbing = false;

        }
        if (other.gameObject.CompareTag("Ladder"))
        {
            Debug.Log("��ҿ��Կ�ʼ��¥����");
            this.canClimb = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ladder"))
        {
            Debug.Log("����뿪��¥��");
            this.canClimb = false;
            this.isClimbing = false;
        }
    }
}